using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace RestClienttService
{
    public class RestClient : HttpClient
    {
        private string _baseUri = "http://localhost:5243/";

        private TokenResponse _token;
        private readonly HttpClient _httpClient;


        public RestClient()
        {
            _httpClient = new HttpClient();
            InitializeTlsProtocol();
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> query)
        {
            await SetTokenAsync();

            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

            using (HttpResponseMessage response = await GetAsync(Utils.AddQueryString(_baseUri + url, query)))
            {
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Populate
                    };
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result, settings);
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                await SetTokenAsync();

                DefaultRequestHeaders.Clear();
                DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                //DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

                using (HttpResponseMessage response = await GetAsync(_baseUri + url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            DefaultValueHandling = DefaultValueHandling.Populate
                        };
                        var result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(result, settings);
                    }

                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<T> PostAsync<T>(string url, string data)
        {
            await SetTokenAsync();

            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await PostAsync(_baseUri + url, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Populate
                    };
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result, settings);
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<T> PutAsync<T>(string url, FileStream fs)
        {
            await SetTokenAsync();

            DefaultRequestHeaders.Clear();
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            //DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);

            using (HttpResponseMessage response = await PutAsync(_baseUri + url, new StreamContent(fs)))
            {
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Populate
                    };
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result, settings);
                }

                throw new Exception(response.ReasonPhrase);
            }
        }
        private void InitializeTlsProtocol()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        private async Task SetTokenAsync()
        {
            return;
            if (_token == null)
            {
                DefaultRequestHeaders.Clear();
                DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "SOME CREDENTIAL SCHEME");

                var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                using (HttpResponseMessage response = await PostAsync(_baseUri + "/some-path-to-get/token", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            DefaultValueHandling = DefaultValueHandling.Populate
                        };
                        var result = await response.Content.ReadAsStringAsync(); //ReadAsAsync<TokenResponse>();
                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            _token = JsonConvert.DeserializeObject<TokenResponse>(result, settings);
                        }
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }
    }
}
