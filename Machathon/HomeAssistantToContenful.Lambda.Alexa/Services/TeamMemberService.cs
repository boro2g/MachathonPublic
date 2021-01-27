using Newtonsoft.Json;
using System.Net;

namespace HomeAssistantToContenful.Lambda.Alexa.Services
{
    public class TeamMemberService
    {
        private TeamMemberModel[] _content;

        public TeamMemberService()
        {
            WebClient client = new WebClient();

            string contentJson = client.DownloadString("https://machathon-api.boro2g.co.uk/team");

            _content = JsonConvert.DeserializeObject<TeamMemberModel[]>(contentJson);
        }

        public TeamMemberModel[] LoadTeam()
        {
            return _content;
        }
    }

    public class TeamMemberModel
    {
        public string Name { get; set; }

        public MoodModel Mood { get; set; }
    }

    public class MoodModel
    {
        public string MoodKey { get; set; }
    }
}



