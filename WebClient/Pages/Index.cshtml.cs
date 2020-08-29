using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebClient.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Lang Lang { set; get; }
        public string JsonAnswer { set; get; }

        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient;

        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private async Task<string> GetTokenAsync(string username, string password)
        {
            KeyValuePair<string, string>[] formData = {
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
        };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/token");
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string contentStr = await response.Content.ReadAsStringAsync();
                var contentObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(contentStr);
                string token = contentObj["access_token"].ToString();
                return token;
            }
            return null;
        }

        public async Task OnPostAll()
        {
            string token = await GetTokenAsync("aaa", "123456");

            if (token != null)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"/langs");
                request.Headers.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    JsonAnswer = await response.Content.ReadAsStringAsync();
                else
                    JsonAnswer = $"ERROR. Status:{response.StatusCode}";
            }
            else
                JsonAnswer = "No token";
        }

        public async Task OnPostOne()
        {
            string token = await GetTokenAsync("aaa", "123456");

            if (token != null)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"/langs/{Lang.Id}");
                request.Headers.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    JsonAnswer = await response.Content.ReadAsStringAsync();
                else
                    JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}";
            }
            else
                JsonAnswer = "No token";
        }

        public async Task OnPostCreate()
        {
            KeyValuePair<string, string>[] formData = {
                new KeyValuePair<string, string>("id", Lang.Id),
                new KeyValuePair<string, string>("year", Lang.Year.ToString())
            };

            string token = await GetTokenAsync("aaa", "123456");

            if (token != null)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/langs");
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Content = new FormUrlEncodedContent(formData);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    JsonAnswer = await response.Content.ReadAsStringAsync();
                else
                    JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}, {Lang.Year}";
            }
            else
                JsonAnswer = "No token";
        }

        public async Task OnPostEdit()
        {
            KeyValuePair<string, string>[] formData = {
                new KeyValuePair<string, string>("id", Lang.Id),
                new KeyValuePair<string, string>("year", Lang.Year.ToString())
            };

            string token = await GetTokenAsync("aaa", "123456");

            if (token != null)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"/langs/{Lang.Id}");
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Content = new FormUrlEncodedContent(formData);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    JsonAnswer = await response.Content.ReadAsStringAsync();
                else
                    JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}, {Lang.Year}";
            }
            else
                JsonAnswer = "No token";
        }

        public async Task OnPostDelete()
        {

            string token = await GetTokenAsync("aaa", "123456");

            if (token != null)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"/langs/{Lang.Id}");
                request.Headers.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    JsonAnswer = await response.Content.ReadAsStringAsync();
                else
                    JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}";
            }
            else
                JsonAnswer = "No token";
        }

    }
}
