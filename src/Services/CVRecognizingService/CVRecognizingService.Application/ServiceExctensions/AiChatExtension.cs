using DotnetGeminiSDK;
using Google.GenAI;
using Microsoft.Extensions.DependencyInjection;
namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class AiChatExtension
    {
        public static IServiceCollection AddGeminiAI(this IServiceCollection services, string API_KEY)
        {
            services.AddSingleton(_ => new Client(apiKey: API_KEY));
            return services;
        }
    }
}
