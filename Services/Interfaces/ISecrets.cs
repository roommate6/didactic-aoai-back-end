using API.Types;

namespace API.Services.Interfaces
{
    public interface ISecrets
    {
        public AssistantInformation AssistantInformation { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}