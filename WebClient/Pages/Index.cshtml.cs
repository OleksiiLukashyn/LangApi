using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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

        public async Task OnPostAll()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"/langs");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                JsonAnswer = await response.Content.ReadAsStringAsync();
            else
                JsonAnswer = $"ERROR. Status:{response.StatusCode}";
        }

        public async Task OnPostOne()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"/langs/{Lang.Id}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                JsonAnswer = await response.Content.ReadAsStringAsync();
            else
                JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}";
        }

        public async Task OnPostCreate()
        {
            KeyValuePair<string, string>[] formData = {
                new KeyValuePair<string, string>("id", Lang.Id),
                new KeyValuePair<string, string>("year", Lang.Year.ToString())
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/langs");
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                JsonAnswer = await response.Content.ReadAsStringAsync();
            else
                JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}, {Lang.Year}";
        }

        public async Task OnPostEdit()
        {
            KeyValuePair<string, string>[] formData = {
                new KeyValuePair<string, string>("id", Lang.Id),
                new KeyValuePair<string, string>("year", Lang.Year.ToString())
            };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"/langs/{Lang.Id}");
            request.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                JsonAnswer = await response.Content.ReadAsStringAsync();
            else
                JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}, {Lang.Year}";
        }

        public async Task OnPostDelete()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"/langs/{Lang.Id}");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                JsonAnswer = await response.Content.ReadAsStringAsync();
            else
                JsonAnswer = $"ERROR. Status:{response.StatusCode}   Lang: {Lang.Id}";
        }

    }
}
