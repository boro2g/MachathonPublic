using Contentful.Core.Models;

namespace HomeAssistantToContenful.Models
{
    public class TeamMemberEntry
    {
        public SystemProperties Sys { get; set; }
        public string Name { get; set; }
        public Asset Photo { get; set; }
        public object Mood { get; set; }
    }
}




