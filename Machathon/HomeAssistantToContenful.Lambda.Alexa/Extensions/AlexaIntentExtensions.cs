using Alexa.NET.Response;
using AlexaCore.Intents;
using HomeAssistantToContenful.Lambda.Alexa.Services;
using System;

namespace HomeAssistantToContenful.Lambda.Alexa.Extensions
{
    public static class AlexaIntentExtensions
    {
        public static SkillResponse TellAndFormatContent(this AlexaIntent intent, string key, string defaultText,
            params string[] parameters)
        {
            var contentIntent = new ContentService().LoadIntentContent(key);

            if (String.IsNullOrWhiteSpace(contentIntent))
            {
                return intent.Tell(defaultText);
            }

            return intent.Tell(String.Format(contentIntent, parameters));
        }

        public static TeamMemberModel[] LoadTeam(this AlexaIntent intent)
        {
            return new TeamMemberService().LoadTeam();
        }
    }
}


