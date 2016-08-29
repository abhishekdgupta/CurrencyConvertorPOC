using CurrencyConvertor.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace CurrencyConvertor.Utilities.FixerIo
{
    public static class FixerIo
    {
        /// <summary>
        /// This method gets all the currency exchange rates which are needed based on the constants configured
        /// </summary>
        /// <returns>FixerIOResponse object which contains the currency rates of the required currencies</returns>
        public static FixerIOResponse GetCurrencyCovnertorRates()
        {
            RestClient client = new RestClient(AppSetings.FixerIOUrl);
            RestRequest request = new RestRequest(string.Format(AppSetings.FixerIOCurrencyConvertorResource,AppSetings.CurrenciesNeeded, AppSetings.BaseCurrency), Method.GET);

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                //TODO: Logging mechanism
                return null;
            }
            var data = JsonConvert.DeserializeObject<FixerIOResponse>(response.Content);
            return data;
        }
    }
}
