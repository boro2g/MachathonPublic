namespace HomeAssistantToContenful.Models.GraphQl
{
    public class OutcomeGraphQl
    {
        public string Outcome { get; set; }
        public TeamMemberGraphQl TeamMember { get; set; }
        public MoodGraphQl Mood { get; set; }
        public SysGraphQl Sys { get; set; }
    }
}



