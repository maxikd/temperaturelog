using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class HttpHelper
    {
        private static HttpHelper _helper;
        private HttpClient _client;
        private MediaTypeFormatter _formatter;

        private HttpHelper()
        {
            _client = new HttpClient();
            _formatter = new JsonMediaTypeFormatter();
        }

        public static HttpHelper Client
        {
            get
            {
                if (_helper == null)
                    _helper = new HttpHelper();
                return _helper;
            }
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url), "The url to get cannot be null nor empty.");

            return _client.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync<T>(string url, T @object)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url), "The url to post cannot be null nor empty.");
            if (@object == null) throw new ArgumentNullException(nameof(@object), "The object to post cannot be null.");

            return _client.PostAsync(url, @object, _formatter);
        }

        public Task<HttpResponseMessage> PutAsync<T>(string url, T @object)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url), "The url to put cannot be null nor empty.");
            if (@object == null) throw new ArgumentNullException(nameof(@object), "The object to post cannot be null.");

            return _client.PutAsync(url, @object, _formatter);
        }

        public Task<HttpResponseMessage> DeleteAsync(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url), "The url to delete cannot be null nor empty.");

            return _client.DeleteAsync(url);
        }
    }
}
