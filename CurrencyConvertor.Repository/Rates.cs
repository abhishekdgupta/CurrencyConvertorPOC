using CurrencyConvertor.Utilities;
using CurrencyConvertor.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertor.Repository
{
    public class Rates
    {
        /// <summary>
        /// database repository object
        /// </summary>
        RateRepository repository;

        /// <summary>
        /// Construtor which initializes the repository
        /// </summary>
        public Rates()
        {
            repository = new RateRepository();
        }

        /// <summary>
        /// This method returns the rate details based on the currency code and the amount
        /// </summary>
        /// <param name="requestData">This contains the parameters sent by the user in the request body</param>
        /// <returns>Response object with converted rate details for a currency code to INR</returns>
        public Response GetRates(string requestData)
        {
            var postedData = JsonConvert.DeserializeObject<RatesRequest>(requestData);
            try
            {
                postedData = postedData ?? new RatesRequest();
                if (string.IsNullOrWhiteSpace(postedData.CurrencyCode))
                {
                    postedData.CurrencyCode = "USD"; // Default Currency
                }
                if (postedData.Amount == null)
                {
                    postedData.Amount = 1; // Default Amount
                }

                var acceptedCurrencyCodes = AppSetings.CurrenciesNeeded.Split(',');
                if (!acceptedCurrencyCodes.Contains(postedData.CurrencyCode))
                {
                    throw new Exception("Invalid currency code...");
                }
                if (postedData.Amount <= 0)
                {
                    throw new Exception("Amount must be greater than zero[0].");
                }
                var currentRate = Math.Round(1 / repository.GetCurrencyRates(postedData.CurrencyCode), 2);

                var requestedRate = Math.Round(currentRate * postedData.Amount.Value, 2);
                var response = new Response
                {
                    Amount = postedData.Amount.Value,
                    ConversionRate = currentRate,
                    Err = AppSetings.SuccessMessage,
                    ReturnCode = 1,
                    SourceCurrency = postedData.CurrencyCode,
                    TimeStamp = DateTime.Now.Ticks.ToString(),
                    Total = requestedRate
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    Amount = postedData.Amount.Value,
                    Err = ex.Message,
                    ReturnCode = 2,
                    SourceCurrency = postedData.CurrencyCode,
                    TimeStamp = DateTime.Now.Ticks.ToString(),
                    Total = 0
                };
                return response;
            }
        }
    }
}
