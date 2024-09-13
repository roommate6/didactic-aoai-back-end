using API.DTOs.Request.AI;
using API.DTOs.Response.AI;
using API.Services.Interfaces;
using Azure.AI.OpenAI.Assistants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly IAssistantsClient _assistantsClient;
        private readonly ISecrets _secrets;

        public AIController(IAssistantsClient assistantsClient, ISecrets secrets)
        {
            _assistantsClient = assistantsClient;
            _secrets = secrets;
        }

        [HttpPost("assistant/post-message")]
        public async Task<ActionResult<PostMessageResponseDTO>>
            Get([FromBody] PostMessageDTO requestBody)
        {
            AssistantThread thread = await
                _assistantsClient.GetClient().CreateThreadAsync();

            ThreadMessage message = await
                _assistantsClient.GetClient().CreateMessageAsync(
                    thread.Id,
                    MessageRole.User,
                    requestBody.TextMessage
            );

            ThreadRun run = await
                _assistantsClient.GetClient().CreateRunAsync(
                    thread.Id,
                    new CreateRunOptions(_secrets.AssistantInformation.Id)
            );

            do
            {
                run = await _assistantsClient.GetClient().GetRunAsync(thread.Id, run.Id);
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }
            while (run.Status == RunStatus.Queued || run.Status == RunStatus.InProgress);

            PostMessageResponseDTO response;
            if (run.Status != RunStatus.Completed)
            {
                response = new() { ResponseText = "Error!" };
                return Ok(response);
            }

            PageableList<ThreadMessage> messagesPage =
                await _assistantsClient.GetClient()
                .GetMessagesAsync(thread.Id);

            IReadOnlyList<ThreadMessage> messages = messagesPage.Data;

            ThreadMessage? lastAssistantMessage = messages.FirstOrDefault(
                m => m.Role == MessageRole.Assistant
            );

            if (lastAssistantMessage == null)
            {
                response = new() { ResponseText = "Error!" };
                return Ok(response);
            }

            string responseText = "Error!";
            foreach (var contentItem in lastAssistantMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    responseText = textItem.Text;
                    break;
                }
            }

            response =
                new() { ResponseText = responseText };

            _ = _assistantsClient.GetClient().DeleteThreadAsync(thread.Id);

            return Ok(response);
        }
    }
}