using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAssistantToContenful
{
    public class ContentfulGraphQlClient
    {
        private GraphQLHttpClient _graphQLClient;

        public ContentfulGraphQlClient(string url, string bearerToken)
        {
            _graphQLClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            _graphQLClient.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        }

        public async Task<GraphQLResponse<TResponse>> SendQueryAsync<TResponse>(GraphQLRequest request, CancellationToken cancellationToken = default)
        {
            return await _graphQLClient.SendQueryAsync<TResponse>(request, cancellationToken);
        }
    }
}



