using Azure.AI.OpenAI.Assistants;

namespace API.Services.Interfaces
{
    public interface IAssistantsClient
    {
        public AssistantsClient GetClient();
    }
}