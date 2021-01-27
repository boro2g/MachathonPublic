using GraphQL;

namespace HomeAssistantToContenful.Queries.GraphQl
{
    public static class OutcomeInfoQuery
    {
        public static GraphQLRequest Query(string name, string moodKey)
        {
            return new GraphQLRequest
            {
                Variables = new { name = name, moodKey = moodKey },
                Query = @"
fragment outcomeInfo on Outcome {
  outcome
  sys {
    id
    publishedAt
    publishedVersion    
  }
  mood {
    moodKey
    description
    sys {
      id
      publishedAt
      publishedVersion
    }
  }
  teamMember {
    name
    sys {
      id
      publishedAt
      publishedVersion
    }
  }
}

query ($name: String!, $moodKey: String!) {
  teamMembers: teamMemberCollection(where: {name: $name}) {
    items {
      name
      sys {
        id
      }
      mood{
        moodKey
      }
    }
  }
  moods: moodCollection(where: {moodKey: $moodKey}) {
    items {
      moodKey
      sys {
        id
      }
    }
  }
  specificOutcomes: outcomeCollection(where: {AND: [{teamMember: {name: $name}}, {mood: {moodKey: $moodKey}}]}) {
    items {
      ...outcomeInfo
    }
  }
  defaultOutcomes: outcomeCollection(where: {AND: [{teamMember: {name: ""Default""}}]}) {
    items {
      ...outcomeInfo
    }
  }

}
"

            };
        }
    }
}



