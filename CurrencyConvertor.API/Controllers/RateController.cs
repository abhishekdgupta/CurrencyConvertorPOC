using CurrencyConvertor.API.Filters;
using CurrencyConvertor.Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CurrencyConvertor.API.Controllers
{
    [RoutePrefix("api/v0/rate")]
    public class RateController : ApiController
    {
        Rates ratesManager;

        public RateController()
        {
            ratesManager = new Rates();
        }

        /// <summary>
        /// Input : This API takes currencyCode and Amount as input
        /// Output: This API returns the currency exchange rate for specific currencies for any amount entered.        
        /// <returns>HttpResponseMessage object which contains the rate details</returns>
        /// </summary>
        [HttpPost]
        [Route("")]
        [CustomCache(Duration = 120, VaryByParam = "currencyCode")]
        public HttpResponseMessage GetCurrencyRates()
        {
            try
            {
                var response = ratesManager.GetRates(Request.Content.ReadAsStringAsync().Result);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                //TODO: Log the error
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { message = "Inetrnal Server error... Please try again later." + ex.Message});
            }
        }
    }
}
