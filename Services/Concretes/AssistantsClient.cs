using API.Services.Interfaces;
using Azure;

namespace API.Services.Concretes
{
    public class AssistantsClient : IAssistantsClient
    {
        private readonly Azure.AI.OpenAI.Assistants.AssistantsClient _client;
        private readonly ISecrets _secrets;

        public AssistantsClient(ISecrets secrets)
        {
            _secrets = secrets;

            _client =
                    new Azure.AI.OpenAI.Assistants.AssistantsClient(
                        new Uri(_secrets.AssistantInformation.EndPoint),
                        new AzureKeyCredential(_secrets.AssistantInformation.Key)
                    );
        }

        public Azure.AI.OpenAI.Assistants.AssistantsClient GetClient()
        {
            return _client;
        }
    }
}