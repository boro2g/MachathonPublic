using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HomeAssistantToContentful.GithubBridge.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/repos/boro2g/machathon/actions/workflows/ui.yml/dispatches")
            {
                Content = new StringContent("{\"ref\": \"master\"}", Encoding.UTF8, "application/json")
            };

            postRequest.Headers.Add("Accept", "application/vnd.github.everest-preview+json");

            postRequest.Headers.Add("User-Agent", "Lambda");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Github PAT");

            var postResponse = httpClient.SendAsync(postRequest).Result;

            postResponse.EnsureSuccessStatusCode();
        }
    }
}


