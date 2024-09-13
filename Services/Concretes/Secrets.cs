using API.Services.Interfaces;
using API.Types;

namespace API.Services.Concretes
{
    public class Secrets : ISecrets
    {
        public AssistantInformation AssistantInformation { get; set; }
    }
}