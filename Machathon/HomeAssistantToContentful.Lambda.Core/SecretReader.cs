using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace HomeAssistantToContenful.Lambda.Core
{
    public class SecretReader
    {
        private AmazonSecretsManagerClient _client;
        private GetSecretValueRequest _request;

        public SecretReader(string secretName = "Machathon", string region = "eu-west-1")
        {
            _client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            _request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT"
            };
        }
        public T GetSecrets<T>()
        {
            GetSecretValueResponse response = null;

            string result = "";

            response = _client.GetSecretValueAsync(_request).Result;

            if (response.SecretString != null)
            {
                result = response.SecretString;
            }
            else
            {
                MemoryStream memoryStream = response.SecretBinary;

                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));

                result = decodedBinarySecret;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}


