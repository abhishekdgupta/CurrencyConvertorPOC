using Newtonsoft.Json;

namespace CurrencyConvertor.Entities
{
    public class Rates
    {
        [JsonProperty("AUD")]
        public double AUD { get; set; }

        [JsonProperty("CAD")]
        public double CAD { get; set; }

        [JsonProperty("EUR")]
        public double EUR { get; set; }

        [JsonProperty("GBP")]
        public double GBP { get; set; }
                
        [JsonProperty("SGD")]
        public double SGD { get; set; }

        [JsonProperty("USD")]
        public double USD { get; set; }

    }
}
