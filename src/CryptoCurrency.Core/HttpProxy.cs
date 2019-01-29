using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CryptoCurrency.Core
{
    public class HttpProxy
    {
        public async static Task<string> Send(string url, HttpMethod method, NameValueCollection headers, string postData, string requestContentType = "application/json")
        {
            using (var client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method
                };

                if (method == HttpMethod.Post && postData != null)
                {
                    request.Content = new StringContent(postData, Encoding.UTF8, requestContentType);
                }

                request.Headers.Add("User-Agent", "cryptocurrency Client");

                if (headers != null)
                {
                    request.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

                    foreach (var header in headers.AllKeys)
                    {
                        request.Headers.Add(header, headers[header]);
                    }
                }

                var response = await client.SendAsync(request);

                var contents = await response.Content.ReadAsStringAsync();

                response.Dispose();

                return contents;
            }
        }

        public async static Task<T> SendJson<T>(string url, HttpMethod method, NameValueCollection headers, string postData, string requestContentType = "application/json")
        {
            var contents = await Send(url, method, headers, postData, requestContentType);    

            return JsonConvert.DeserializeObject<T>(contents);
        }

        public static async Task<T> GetJson<T>(string url, NameValueCollection headers)
        {
            return await SendJson<T>(url, HttpMethod.Get, headers, null);
        }
    }
}
