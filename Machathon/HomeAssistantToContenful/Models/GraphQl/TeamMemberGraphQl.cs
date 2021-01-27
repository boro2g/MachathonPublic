namespace HomeAssistantToContenful.Models.GraphQl
{
    public class TeamMemberGraphQl
    {
        public string Name { get; set; }

        public MoodGraphQl Mood { get; set; }
        public SysGraphQl Sys { get; set; }
    }
}



