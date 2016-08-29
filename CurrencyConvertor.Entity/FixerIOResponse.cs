using Newtonsoft.Json;

namespace CurrencyConvertor.Entities
{
    public class FixerIOResponse
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("rates")]
        public Rates Rates { get; set; }
    }
}
