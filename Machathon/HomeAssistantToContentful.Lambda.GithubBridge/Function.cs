using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using HomeAssistantToContenful.Lambda.Core;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HomeAssistantToContentful.Lambda.GithubBridge
{
    public class Function
    {
        private HttpClient _httpClient;
        private SecretData _secrets;

        public Function()
        {
            _httpClient = new HttpClient();

            var secretReader = new SecretReader();

            _secrets = secretReader.GetSecrets<SecretData>();
        }

        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
            await ProcessRecordAsync(context);            
        }

        private async Task ProcessRecordAsync(ILambdaContext context)
        {
            context.Logger.LogLine($"Notifying github");

            var postRequest = new HttpRequestMessage(HttpMethod.Post, _secrets.GithubPublishUrl)
            {
                Content = new StringContent("{\"ref\": \"master\"}", Encoding.UTF8, "application/json")
            };

            postRequest.Headers.Add("Accept", "application/vnd.github.everest-preview+json");

            postRequest.Headers.Add("User-Agent", "Lambda");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _secrets.GithubPat);

            var postResponse = await _httpClient.SendAsync(postRequest);

            postResponse.EnsureSuccessStatusCode();

            await Task.CompletedTask;
        }
    }

    class SecretData
    {
        [JsonProperty("github_pat")]
        public string GithubPat { get; set; }

        [JsonProperty("github_publish_url")]
        public string GithubPublishUrl { get; set; }
    }
}


