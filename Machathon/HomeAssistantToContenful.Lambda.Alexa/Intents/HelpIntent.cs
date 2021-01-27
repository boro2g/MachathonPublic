using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;
using HomeAssistantToContenful.Lambda.Alexa.Extensions;
using System.Collections.Generic;

namespace HomeAssistantToContenful.Lambda.Alexa.Intents
{
    public class HelpIntent : AlexaHelpIntent
    {
        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return this.TellAndFormatContent("HelpIntent", $"Help!");
        }
    }
}


