using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CloudCoin_SafeScan
{
    public class RestClient
    {
        public Uri BaseUrl;

        HttpClient Client;

        public RestClient()
        {
            Client = new HttpClient();
        }

        public async Task<string> Execute(RestRequest request)
        {
            var fullRequest = new HttpRequestMessage(request.Method, BaseUrl.AbsoluteUri + "/" + request.Resource);
            Client.Timeout = new TimeSpan(0, 0, 0, 0, request.Timeout);
            HttpResponseMessage response = await Client.SendAsync(fullRequest);

            return await response.Content.ReadAsStringAsync();
        }
    }

    public class RestRequest
    {
        public HttpMethod Method { get; set; }
        public int Timeout { get; set; }
        List<Parameter> Parameters { get; }
        public string Resource { get; set; }


        public HttpRequestMessage Request;

        public RestRequest(string request)
        {
            Method = HttpMethod.Get;
            Parameters = new List<Parameter>();
            Resource = request;
        }
    }

    public class Parameter
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Type of the parameter
        /// </summary>
        public ParameterType Type { get; set; }

        /// <summary>
        /// MIME content type of the parameter
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Return a human-readable representation of this parameter
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", this.Name, this.Value);
        }
    }

    public enum ParameterType
    {
        Cookie,
        GetOrPost,
        UrlSegment,
        HttpHeader,
        RequestBody,
        QueryString
    }
}
