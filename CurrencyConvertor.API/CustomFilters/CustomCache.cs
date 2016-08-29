using System;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
    
namespace CurrencyConvertor.API.Filters
{
    public class CustomCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The duration uptil which the data will last in cache(in seconds)
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Key of the data in cache
        /// </summary>
        private string _cachekey;

        /// <summary>
        /// The parameter which is optional but if mentioned saves unique values for each new value
        /// </summary>
        public string VaryByParam;

        private static readonly MemoryCache WebApiCache = MemoryCache.Default;

        /// <summary>
        /// This method checks if the cache contains some items related to the request and returns if found.
        /// </summary>
        /// <param name="ac">HttpActionContext for current request</param>
        public override void OnActionExecuting(HttpActionContext ac)
        {
            if (ac != null)
            {
                string paramValue = string.Empty;
                if (!string.IsNullOrWhiteSpace(VaryByParam))
                {
                    var requestData = ac.Request.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(requestData))
                    {
                        JObject obj = JObject.Parse(requestData);

                        paramValue = obj.Value<string>(VaryByParam) ?? string.Empty;
                    }
                }

                _cachekey = string.Join(":", new string[] { ac.Request.RequestUri.AbsolutePath, ac.Request.Headers.Accept.FirstOrDefault().ToString(), paramValue });
                if (WebApiCache.Contains(_cachekey))
                {
                    var val = (string)WebApiCache.Get(_cachekey);
                    if (val != null)
                    {
                        ac.Response = ac.Request.CreateResponse();
                        ac.Response.Content = new StringContent(val);
                        var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct");
                        if (contenttype == null)
                            contenttype = new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                        ac.Response.Content.Headers.ContentType = contenttype;
                        ac.Response.Headers.CacheControl = setClientCache();
                        return;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        /// <summary>
        /// This method saves the values to cache if the values do not exist already
        /// </summary>
        /// <param name="actionExecutedContext">actionExecutedContext for current request</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(WebApiCache.Contains(_cachekey)))
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(Duration));
                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(Duration));
            }
            actionExecutedContext.ActionContext.Response.Headers.CacheControl = setClientCache();
        }

        /// <summary>
        /// Sets the cache details
        /// </summary>
        /// <returns>CacheControlHeaderValue object</returns>
        private CacheControlHeaderValue setClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue();
            cachecontrol.MaxAge = TimeSpan.FromSeconds(Duration);
            cachecontrol.MustRevalidate = true;
            return cachecontrol;
        }
    }
}