using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;
using HomeAssistantToContenful.Lambda.Alexa.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssistantToContenful.Lambda.Alexa.Intents
{
    public class LaunchIntent : AlexaIntent
    {
        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            var team = this.LoadTeam();

            return this.TellAndFormatContent("LaunchIntent", $"Welcome to Alexa. How can I help?", team.Count().ToString());
        }
    }
}


