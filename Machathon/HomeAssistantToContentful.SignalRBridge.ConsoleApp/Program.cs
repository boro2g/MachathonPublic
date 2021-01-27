using System;
using System.Net.Http;
using System.Text;

namespace HomeAssistantToContentful.SignalRBridge.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var payload = @"

{
    ""Username"": ""Nick"",
    ""Usermood"": ""Ok"",
    ""Temperatures"": {
                ""office"": 19.9,
        ""lounge"": 14.3,
        ""basement"": 17.5
    },
    ""Outcome"": ""At least the sun is out!"",
    ""IsDefaultOutcome"": false
}
";

            var client = new HttpClient();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "https://signalrendpoint/messages")
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json"),                
            };            

            var postResponse = client.SendAsync(postRequest).GetAwaiter().GetResult();

            postResponse.EnsureSuccessStatusCode();
        }
    }
}


