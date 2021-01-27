using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using GraphQL;
using HomeAssistantToContenful.Lambda.Core;
using HomeAssistantToContenful.Models.GraphQl;
using HomeAssistantToContenful.Queries.GraphQl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HomeAssistantToContenful.Lambda
{
    public class Function
    {
        private static SecretData _secrets;

        private static async Task Main(string[] args)
        {
            Func<InputEvent, ILambdaContext, string> func = FunctionHandler;
            using (var handlerWrapper = HandlerWrapper.GetHandlerWrapper(func, new DefaultLambdaJsonSerializer()))
            using (var bootstrap = new LambdaBootstrap(handlerWrapper))
            {
                await bootstrap.RunAsync();
            }
        }

        public static string FunctionHandler(InputEvent input, ILambdaContext context)
        {
            var secretReader = new SecretReader();

            _secrets = secretReader.GetSecrets<SecretData>();

            context.Logger.LogLine(JsonConvert.SerializeObject(input));

            var entryClient = new EntryClient(_secrets.CmsApiKey, _secrets.SpaceId);

            var queryClient = new ContentfulGraphQlClient(_secrets.GraphQlEndpoint, _secrets.CdApiKey);

            ProcessData(input, context, entryClient, queryClient).GetAwaiter().GetResult();

            return $"{input.Data.Action} processed";
        }

        private static async Task ProcessData(InputEvent input, ILambdaContext context, EntryClient entryClient, ContentfulGraphQlClient queryClient)
        {
            var result = await queryClient.SendQueryAsync<ResultGraphQl>(OutcomeInfoQuery.Query(input.Data.Username, input.Data.Usermood));

            var existingOutcome = result.Data.SpecificOutcomes.Items.FirstOrDefault();

            var entryId = "";

            var outcome = result.Data.DefaultOutcomes.Items.FirstOrDefault(a => a.Mood.MoodKey == input.Data.Usermood).Outcome;

            var isDefault = true;

            if (!String.IsNullOrWhiteSpace(input.Data.UsermoodOutcome))
            {
                outcome = input.Data.UsermoodOutcome;

                isDefault = false;
            }

            if (existingOutcome != null)
            {
                entryId = await UpdateOutcome(input, context, entryClient, existingOutcome);
            }
            else
            {
                entryId = await CreateItem(input, context, entryClient, result);
            }

            await entryClient.PublishEntry(entryId);

            var existingTeamMember = result.Data.TeamMembers.Items.FirstOrDefault();

            if (existingTeamMember.Mood.MoodKey != input.Data.Usermood)
            {
                entryId = await UpdateTeamMemberMood(entryClient, context, result.Data.Moods, input.Data.Usermood, existingTeamMember);

                await entryClient.PublishEntry(entryId);
            }

            context.Logger.LogLine($"Writing message to {_secrets.SnsTopicArn}");

            await new AmazonSimpleNotificationServiceClient().PublishAsync(new PublishRequest
            {
                TopicArn = _secrets.SnsTopicArn,
                Message = JsonConvert.SerializeObject(new
                {
                    Username = input.Data.Username,
                    Usermood = input.Data.Usermood,
                    Temperatures = input.Data.Temperatures,
                    Outcome = outcome,
                    IsDefaultOutcome = isDefault
                }),
                Subject = "Forwarding on HA event"
            });
        }

        private static async Task<string> CreateItem(InputEvent input, ILambdaContext context, EntryClient entryClient, GraphQLResponse<ResultGraphQl> result)
        {
            var locale = "en-US";

            var entry = new Entry<dynamic>
            {
                SystemProperties = new SystemProperties()
            };

            entry.SystemProperties.Id = Guid.NewGuid().ToString("N");

            entry.Fields = new
            {
                Mood = new Dictionary<string, Reference>()
                    {
                        { locale, new Reference (SystemLinkTypes.Entry,  result.Data.Moods.Items.FirstOrDefault(a=>a.MoodKey == input.Data.Usermood).Sys.Id) },
                    },
                Outcome = new Dictionary<string, string>()
                    {
                        { locale,  GetUserMood(input)},
                    },
                TeamMember = new Dictionary<string, Reference>()
                    {
                        { locale, new Reference (SystemLinkTypes.Entry,  result.Data.TeamMembers.Items.FirstOrDefault().Sys.Id) },
                    },
            };

            context.Logger.LogLine($"Creating entry: '{JsonConvert.SerializeObject(entry.Fields)}'");

            await entryClient.CreateItem(entry, "outcome");

            return entry.SystemProperties.Id;
        }

        private static string GetUserMood(InputEvent input)
        {
            return input.Data.UsermoodOutcome != "" ? input.Data.UsermoodOutcome : "default";
        }

        private static async Task<string> UpdateOutcome(InputEvent input, ILambdaContext context, EntryClient entryClient, OutcomeGraphQl existingOutcome)
        {
            var outcome = new OutcomeModel { Outcome = GetUserMood(input) };

            context.Logger.LogLine($"Updating entry: '{existingOutcome.Sys.Id}' with value: '{outcome.Outcome}'");

            await entryClient.UpdateEntry<object>(outcome, existingOutcome.Sys.Id);

            return existingOutcome.Sys.Id;
        }

        private async static Task<string> UpdateTeamMemberMood(EntryClient entryClient, ILambdaContext context, MoodsGraphQl moods, string usermood, TeamMemberGraphQl existingTeamMember)
        {
            var model = new TeamMemberModel { Mood = new Reference(SystemLinkTypes.Entry, moods.Items.FirstOrDefault(a => a.MoodKey == usermood).Sys.Id) };

            context.Logger.LogLine($"Updating entry: '{existingTeamMember.Sys.Id}' with mood: '{usermood}'");

            await entryClient.UpdateEntry<TeamMemberModel>(model, existingTeamMember.Sys.Id);

            return existingTeamMember.Sys.Id;
        }
    }

    class OutcomeModel
    {
        public string Outcome { get; set; }
    }

    class TeamMemberModel
    {
        public Reference Mood { get; set; }
    }

    class SecretData
    {
        [JsonProperty("cm_api_key")]
        public string CmsApiKey { get; set; }

        [JsonProperty("cd_api_key")]
        public string CdApiKey { get; set; }

        [JsonProperty("space_id")]
        public string SpaceId { get; set; }

        [JsonProperty("cd_graphql_url")]
        public string GraphQlEndpoint { get; set; }

        [JsonProperty("sns_topic_arn")]
        public string SnsTopicArn { get; set; }  
    }
}


