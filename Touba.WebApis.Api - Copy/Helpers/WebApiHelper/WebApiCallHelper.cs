using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Helpers.WebApiHelper
{
    public static class WebApiCallHelper
    {
        public static WebApiResponse CallApiWithPostJson(string url,string jsonBody,string token)
        {
            WebApiResponse webApiResponse = new WebApiResponse();
            var client = new RestClient(url);
            RestRequest restRequest = new RestRequest();         
            restRequest.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(token))
            {
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));

            }
            restRequest.Method = Method.POST;
            
            if (!string.IsNullOrEmpty( jsonBody))
            {
                restRequest.AddJsonBody(jsonBody);
            }
            var response = client.Execute(restRequest);
            webApiResponse.ResponseCode = response.StatusCode.ToString();
            webApiResponse.ResponseContent = response.Content;
            return webApiResponse;
        }

        public static WebApiResponse CallApiWithPostParameters(string BaseUrl, string getMethod, Dictionary<string, string> paramsDictionary, string token)
        {
            WebApiResponse webApiResponse = new WebApiResponse();
            var client = new RestClient(BaseUrl);

            var restRequest = new RestRequest(getMethod);
            foreach (var item in paramsDictionary)
            {
                restRequest.AddQueryParameter(item.Key, item.Value);
            }
            restRequest.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(token))
            {
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));
            }
            restRequest.Method = Method.POST;
            var response = client.Execute(restRequest);
            webApiResponse.ResponseCode = response.StatusCode.ToString();
            webApiResponse.ResponseContent = response.Content;
            return webApiResponse;
        }


        public static WebApiResponse CallApiWithGetParameters(string BaseUrl, string getMethod, Dictionary<string, string> paramsDictionary, string token)
        {
            WebApiResponse webApiResponse = new WebApiResponse();
            var client = new RestClient(BaseUrl);
           
            var restRequest = new RestRequest(getMethod);
            foreach (var item in paramsDictionary)
            {
                restRequest.AddQueryParameter(item.Key, item.Value);
            }       
            restRequest.AddHeader("Accept", "application/json");
            if (!string.IsNullOrEmpty(token))
            {
                client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", token));
            }
            restRequest.Method = Method.GET;        
            var response = client.Execute(restRequest);
            webApiResponse.ResponseCode = response.StatusCode.ToString();
            webApiResponse.ResponseContent = response.Content;
            return webApiResponse;
        }
    }

    public class WebApiResponse
    { 
       public string ResponseCode { get; set; }
       public string ResponseContent { get; set; }

    }
   
}
