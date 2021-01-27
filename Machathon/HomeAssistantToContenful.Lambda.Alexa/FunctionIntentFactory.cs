using AlexaCore.Intents;
using HomeAssistantToContenful.Lambda.Alexa.Intents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeAssistantToContenful.Lambda.Alexa
{
    public class FunctionIntentFactory : IntentFactory
    {
        protected override List<Type> ApplicationIntentTypes()
        {
            return IntentFinder.FindIntentTypes(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly }).ToList();
        }

        public override Type LaunchIntentType()
        {
            return typeof(LaunchIntent);
        }

        public override Type HelpIntentType()
        {
            return typeof(HelpIntent);
        }
    }
}

