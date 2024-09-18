using DotnetGeminiSDK;
using Microsoft.Extensions.DependencyInjection;
namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class AiChatExtension
    {
        public static IServiceCollection AddGeminiAI(this IServiceCollection services, string API_KEY)
        {
            services.AddGeminiClient(config =>
            {
                config.ApiKey = API_KEY;
            });
            return services;
        }
    }
}
