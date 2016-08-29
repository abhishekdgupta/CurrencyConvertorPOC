using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConvertor.Entities
{
    public class Response
    {
        [JsonProperty(PropertyName = "SourceCurrency", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceCurrency { get; set; }

        [JsonProperty(PropertyName = "ConversionRate", NullValueHandling = NullValueHandling.Ignore)]
        public double ConversionRate { get; set; }

        [JsonProperty(PropertyName = "Amount", NullValueHandling = NullValueHandling.Ignore)]
        public double Amount { get; set; }

        [JsonProperty(PropertyName = "Total", NullValueHandling = NullValueHandling.Ignore)]
        public double Total { get; set; }

        [JsonProperty(PropertyName = "ReturnCode", NullValueHandling = NullValueHandling.Ignore)]
        public int ReturnCode { get; set; }

        [JsonProperty(PropertyName = "Err", NullValueHandling = NullValueHandling.Ignore)]
        public string Err { get; set; }

        [JsonProperty(PropertyName = "TimeStamp", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeStamp { get; set; }
    }
}
