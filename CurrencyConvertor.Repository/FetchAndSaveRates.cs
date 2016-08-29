using CurrencyConvertor.Utilities;
using CurrencyConvertor.Utilities.FixerIo;
using FluentScheduler;
using System;


namespace CurrencyConvertor.Repository
{
    public class FetchAndSaveRates : Registry
    {
        RateRepository rateRepository;

        /// <summary>
        /// This is the constructor which initializes the rate daemon
        /// </summary>
        public FetchAndSaveRates()
        {
            rateRepository = new RateRepository();
            // Schedule a simple job to run at a specific time
            Schedule(() =>
            {
                StartWork();
            }).ToRunEvery(15).Seconds();

        }

        /// <summary>
        /// This method gets the rates from FixerIO tool and makes a call to the repository to save the values to the database
        /// </summary>
        public void StartWork()
        {
            var newRates = FixerIo.GetCurrencyCovnertorRates();

            var currencyCodesSupported = AppSetings.CurrenciesNeeded.Split(',');

            foreach (var currencyCode in currencyCodesSupported)
            {
                var value = newRates.Rates.GetType().GetProperty(currencyCode).GetValue(newRates.Rates, null);
                rateRepository.SaveCurrencyRates(currencyCode, Convert.ToDouble(value));
            }
        }
    }
}
