using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Extensions;
using AlexaCore.Intents;
using HomeAssistantToContenful.Lambda.Alexa.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssistantToContenful.Lambda.Alexa.Intents
{
    public class TeamMoodIntent : AlexaIntent
    {
        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            var team = this.LoadTeam();

            return this.TellAndFormatContent("TeamMoodIntent", "Currently {0}", team.Select(a=> $"{a.Name} is {a.Mood.MoodKey}").JoinStringList());
        }
    }
}


