using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class SerializerHelper
    {
        private static JsonSerializerSettings _settings;

        static SerializerHelper()
        {
            _settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public static string Serialize(object @object)
        {
            if (@object == null) throw new ArgumentNullException(nameof(@object), "The object to serialize cannot be null.");

            return JsonConvert.SerializeObject(@object, _settings);
        }

        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json), "The Json string cannot be null nor empty.");

            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public static async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response), "The response object to deserialize cannot be null.");

            string responseString = await response.Content.ReadAsStringAsync();
            return Deserialize<T>(responseString);
        }
    }
}
