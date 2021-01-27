using Contentful.Core.Models;

namespace HomeAssistantToContenful.Models
{
    public class OutcomeEntry
    {
        public SystemProperties Sys { get; set; }
        public string Outcome { get; set; }
        public object TeamMember { get; set; }
        public object Mood { get; set; }
    }
}




