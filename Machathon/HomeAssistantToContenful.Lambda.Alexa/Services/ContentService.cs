using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace HomeAssistantToContenful.Lambda.Alexa.Services
{
    public class ContentService
    {
        private IntentContentModel[] _content;

        public ContentService()
        {
            WebClient client = new WebClient();

            string contentJson = client.DownloadString("https://machathon-api.boro2g.co.uk/content");

            _content = JsonConvert.DeserializeObject<IntentContentModel[]>(contentJson);
        }

        public string LoadIntentContent(string intentName)
        {
            return _content.FirstOrDefault(a => a.IntentName == intentName).IntentContent;
        }
    }

    public class IntentContentModel
    {
        public string IntentName { get; set; }

        public string IntentContent { get; set; }
    }
}


