using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model.Request;
using DotnetGeminiSDK.Model.Response;

namespace CVRecognizingService.Application.Helpers.AiPdfComanion;

public class GeminiAITextChat
{
    private const string BASE_FORMATING_SYSTEM_PROMPT = "You are an AI assistant that helps make structural response about person from text in json format with this template: {\r\n  \"Full Name\": \"\",\r\n  \"Contacts\": {\r\n    \"Phone\": \"\",\r\n    \"Email\": \"\",\r\n    \"LinkedIn\": \"\"\r\n  },\r\n  \"Location\": \"\",\r\n  \"Links\": {\r\n    \"GitHub\": \"\"\r\n  },\r\n  \"Job Title\": \"\",\r\n  \"Professional Experience\": [\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n      ]\r\n    },\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n        \"\",\r\n      ]\r\n    },\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n        \"\"\r\n      ]\r\n    }\r\n  ],\r\n  \"Education\": {\r\n    \"Degree\": \"\",\r\n    \"University\": \"\",\r\n    \"Year\": integer\r\n  },\r\n  \"Languages\": {\r\n    \"Programming Languages\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Back-end\": [\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Front-end\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Infrastructure\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Metrics\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ]\r\n  }\r\n} with this text ";
    private const string BASE_IS_FILE_CV_PROMPT = "You are an AI assistant that helps recognize is this content is a CV. Your answer should be true if this text is CV and otherwise false";
    private readonly IGeminiClient _geminiClient;
    private readonly GenerationConfig _generationConfig;

    public GeminiAITextChat(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
        _generationConfig = new GenerationConfig()
        {
            MaxOutputTokens = 4096,
            Temperature = 0,
            TopP = 1,
            TopK = 0
        };
    }

    public async Task<GeminiMessageResponse> GetFormatedText(string ask, CancellationToken cancellationToken)
    {
        return await _geminiClient.TextPrompt(BASE_FORMATING_SYSTEM_PROMPT + ask, _generationConfig);
    }

    public async Task<GeminiMessageResponse> IsCV(string ask, CancellationToken cancellationToken)
    {
        return await _geminiClient.TextPrompt(BASE_IS_FILE_CV_PROMPT + ask, _generationConfig);
    }
}