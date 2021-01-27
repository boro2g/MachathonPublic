var contentful = require("contentful");
const apollo = require("apollo-boost");
const ApolloClient = apollo.default;
const fetch = require("node-fetch");
var gql = require("graphql-tag");

exports.lambdaHandler = async (event) => {
  const clientG = new ApolloClient({
    uri: "https://graphql.contentful.com/content/v1/spaces/spaceId",
    fetch: fetch,
    request: (operation) => {
      const token = "access token";
      operation.setContext({
        headers: {
          authorization: token ? `Bearer ${token}` : "",
        },
      });
    },
  });

  let query;

  if (event && event.rawPath == "/default/content") {
    query = gql`
      query Result {
        results: alexaIntentCollection {
          items {
            intentName
            intentContent
          }
        }
      }
    `;
  } else {
    query = gql`
      query Result {
        results: teamMemberCollection (where: {AND: [{name_not: "Default"}]}) {
          items {
            name
            mood {
              moodKey
            }
          }
        }
      }
    `;
  }

  const result = await clientG
    .query({
      query: query,
    })
    .catch((a) => console.log(a)); 

  const response = {
    statusCode: 200,
    body: JSON.stringify(result.data.results.items),
  };

  return response;
};


