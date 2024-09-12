using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model.Response;

namespace CVRecognizingService.Application.AICompanion
{
    public class GeminiAITextChat
    {
        private readonly IGeminiClient _geminiClient;

        public GeminiAITextChat(IGeminiClient geminiClient)
        {
            _geminiClient = geminiClient;
        }

        public async Task<GeminiMessageResponse> GetAnswer(string ask)
        {
           return await _geminiClient.TextPrompt(ask, new DotnetGeminiSDK.Model.Request.GenerationConfig() 
           {
               MaxOutputTokens = 4096,
               Temperature = 0,
               TopP = 1,
               TopK = 0
           });
        }
    }
}
