using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using HomeAssistantToContenful.Models;
using HomeAssistantToContenful.Models.GraphQl;
using HomeAssistantToContenful.Queries.GraphQl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HomeAssistantToContenful.ConsoleApp
{
    class Program
    {
        private const string Locale = "en-US";

        static void Main(string[] args)
        {
            var entryClient = new EntryClient("CM access token", "spaceId");

            SearchEntry("Nick", "Sad");

            //UpdateOrCreateItemExample(entryClient);

            //UpdateItemExample(entryClient);

            //entryClient.PublishEntry(entryId).GetAwaiter().GetResult();
        }

        private static void UpdateOrCreateItemExample(EntryClient entryClient)
        {
            var entry = new Entry<dynamic>
            {
                SystemProperties = new SystemProperties()
            };

            entry.SystemProperties.Id = Guid.NewGuid().ToString("N"); 

            entry.Fields = new
            {
                Mood = new Dictionary<string, Reference>()
                {
                    { Locale, new Reference (SystemLinkTypes.Entry,  "8DfzImBjRCrh1N8b61JrE" /*ok*/) },       
                },
                Outcome = new Dictionary<string, string>()
                {
                    { Locale, "outcome 123" },
                },
                TeamMember = new Dictionary<string, Reference>()
                {
                    { Locale, new Reference (SystemLinkTypes.Entry,  "4e8QtDuwiw5of2g56sxInJ" /*nick*/) },
                },
            };

            entryClient.CreateItem(entry, "outcome").GetAwaiter().GetResult();
        }

        private static void SearchEntry(string name, string mood)
        {
            var client = new ContentfulGraphQlClient("https://graphql.contentful.com/content/v1/spaces/spaceId", "access token");

            var query = OutcomeInfoQuery.Query(name, mood);

            var result = client.SendQueryAsync<ResultGraphQl>(query).GetAwaiter().GetResult();

            string resultJson = JsonConvert.SerializeObject(result.Data);            
        }

        private static void UpdateItemExample(EntryClient entryClient)
        {
            var entryId = "6eQdwb4dHdKSvotcNFTb6b";

            var temp = new Dictionary<string, double>() { { "a", 1 } };

            entryClient.UpdateEntry(new TemperatureEntry { Temperature = "few", TemperatureBreakdown = temp }, entryId).GetAwaiter().GetResult();
        }
    }

    
}


