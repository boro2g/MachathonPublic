namespace HomeAssistantToContenful.Models.GraphQl
{
    public class ResultGraphQl
    {
        public SpecificOutcomesGraphQl SpecificOutcomes { get; set; }

        public SpecificOutcomesGraphQl DefaultOutcomes { get; set; }

        public MoodsGraphQl Moods { get; set; }

        public TeamMembersGraphQl TeamMembers { get; set; }
    }
}



