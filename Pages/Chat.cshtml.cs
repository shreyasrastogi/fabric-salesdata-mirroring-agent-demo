using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FabricWebApp.Pages
{
    public class ChatModel : PageModel
    {
        public List<(string Role, string Content)> ChatHistory { get; set; } = new();
        [BindProperty] public string? UserMessage { get; set; }

        private readonly string projectEndpoint = "https://test2708.services.ai.azure.com/api/projects/firstProject";
        private readonly string agentId = "asst_EkkDXEGzL9dMWRaRjSj7jG4r";

        private string ThreadId
        {
            get => HttpContext.Session.GetString("ThreadId");
            set => HttpContext.Session.SetString("ThreadId", value);
        }

        public void OnGet()
        {
            ChatHistory = HttpContext.Session.GetObject<List<(string, string)>>("ChatHistory")
                          ?? new List<(string, string)>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage)) return Page();

            ChatHistory = HttpContext.Session.GetObject<List<(string, string)>>("ChatHistory")
                          ?? new List<(string, string)>();
            ChatHistory.Add(("User", UserMessage));

            // ✅ Use ONLY PersistentAgentsClient like your console app:
            var agentsClient = new PersistentAgentsClient(
                projectEndpoint,
                new DefaultAzureCredential());

            // ✅ Create thread once
            if (string.IsNullOrEmpty(ThreadId))
            {
                var thread = agentsClient.Threads.CreateThread();
                ThreadId = thread.Value.Id;
            }

            // ✅ Create user message
            agentsClient.Messages.CreateMessage(
                ThreadId,
                Azure.AI.Agents.Persistent.MessageRole.User,
                UserMessage);

            // ✅ Run agent + poll
            Azure.AI.Agents.Persistent.ThreadRun run =
                agentsClient.Runs.CreateRun(ThreadId, agentId);

            while (run.Status == Azure.AI.Agents.Persistent.RunStatus.Queued ||
                   run.Status == Azure.AI.Agents.Persistent.RunStatus.InProgress)
            {
                await Task.Delay(500);
                run = agentsClient.Runs.GetRun(ThreadId, run.Id);
            }

            // ✅ After completion, retrieve assistant reply
          var messages = agentsClient.Messages.GetMessages(ThreadId); // ✅ FIX: Use string instead of enum


            var lastAssistant = messages.FirstOrDefault(
                m => m.Role.ToString() == "assistant");

            if (lastAssistant != null)
            {
                string reply = string.Join("\n",
                    lastAssistant.ContentItems
                        .OfType<Azure.AI.Agents.Persistent.MessageTextContent>()
                        .Select(t => t.Text));

                ChatHistory.Add(("Agent", reply));
            }

            HttpContext.Session.SetObject("ChatHistory", ChatHistory);
            return Page();
        }
    }

    // Session helpers
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}