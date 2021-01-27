using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HomeAssistantToContentful.Lambda.SignalRBridge
{
    public class Function
    {
        private HttpClient _httpClient;

        public Function()
        {
            _httpClient = new HttpClient();
        }

        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
            foreach (var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Notifying signalR with: {record.Sns.Message}");

            var postRequest = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("signalr_endpoint"))
            {
                Content = new StringContent(record.Sns.Message, Encoding.UTF8, "application/json")
            };

            var postResponse = await _httpClient.SendAsync(postRequest);

            postResponse.EnsureSuccessStatusCode();

            await Task.CompletedTask;
        }
    }
}


