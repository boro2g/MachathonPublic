using Contentful.Core.Models;

namespace HomeAssistantToContenful.Models
{
    public class MoodEntry
    {
        public SystemProperties Sys { get; set; }
        public string MoodKey { get; set; }
        public string Description { get; set; }
    }
}




