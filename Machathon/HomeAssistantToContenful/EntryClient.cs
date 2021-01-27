using Contentful.Core;
using Contentful.Core.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeAssistantToContenful
{
    public class EntryClient
    {
        private ContentfulManagementClient _contentfulClient;

        public EntryClient(string apiKey, string spaceId)
        {
            var httpClient = new HttpClient();

            _contentfulClient = new ContentfulManagementClient(httpClient, apiKey, spaceId)
            {
            }; ;
        }

        public ContentfulManagementClient Client => _contentfulClient;

        public EntryClient(ContentfulManagementClient contentfulClient)
        {
            _contentfulClient = contentfulClient;
        }

        public async Task CreateItem(Entry<dynamic> itemToUpdate, string contentType)
        {
            await _contentfulClient.CreateOrUpdateEntry(itemToUpdate, contentTypeId: contentType);
        }

        public async Task UpdateEntry<T>(T itemToUpdate, string entryId, string locale = "en-US")
        {
            await _contentfulClient.UpdateEntryForLocale(itemToUpdate, entryId, locale);
        }

        public async Task PublishEntry(string entryId)
        {
            var latest = await _contentfulClient.GetEntry(entryId);

            if (latest != null)
            {
                await _contentfulClient.PublishEntry(entryId, latest.SystemProperties.Version.Value);
            }
        }
    }
}



