using Google.GenAI;
using Google.GenAI.Types;

namespace CVRecognizingService.Application.Helpers.AiPdfComanion;

public class GeminiAITextChat
{
    private const string BASE_FORMATING_SYSTEM_PROMPT = "You are an expert Resume Parser AI. Your task is to extract information from the provided resume text and map it STRICTLY to the JSON structure defined below.\r\n\r\n### RULES:\r\n1. **Output Format**: Return ONLY raw JSON. Do not use Markdown formatting (no ```json ... ``` blocks).\r\n2. **Data Structure**: Follow the JSON template exactly. Do not add fields that are not in the template.\r\n3. **Skills**: Must be a flat list of objects with \"SkillName\", do not categorize them into \"Soft\" or \"Technical\".\r\n4. **Name**: Split the full name into FirstName, LastName, and MiddleName (if applicable).\r\n5. **Job Description**: Concatenate bullet points of responsibilities into a single string for \"JobDescription\".\r\n6. **Dates**: Use ISO 8601 format (YYYY-MM-DD) for DateTimes. For Education, extract just the Year (integer).\r\n7. **Nulls**: If a field is not found in the text, return null (or false for booleans).\r\n\r\n### JSON TEMPLATE:\r\n{\r\n  \"FirstName\": \"string\",\r\n  \"LastName\": \"string\",\r\n  \"MiddleName\": \"string or null\",\r\n  \"Email\": \"string\",\r\n  \"Phone\": \"string\",\r\n  \"DateOfBirth\": \"YYYY-MM-DD or null\",\r\n  \"City\": \"string\",\r\n  \"Country\": \"string\",\r\n  \"PhotoUrl\": \"string or null\",\r\n  \"Educations\": [\r\n    {\r\n      \"InstitutionName\": \"string\",\r\n      \"Specialization\": \"string\",\r\n      \"Degree\": \"string\",\r\n      \"StartYear\": 2010,\r\n      \"EndYear\": 2014\r\n    }\r\n  ],\r\n  \"WorkExperiences\": [\r\n    {\r\n      \"CompanyName\": \"string\",\r\n      \"City\": \"string\",\r\n      \"Position\": \"string\",\r\n      \"StartDate\": \"YYYY-MM-DD\",\r\n      \"EndDate\": \"YYYY-MM-DD or null\",\r\n      \"IsCurrentlyWorking\": false,\r\n      \"JobDescription\": \"string (combine all responsibilities here)\"\r\n    }\r\n  ],\r\n  \"Skills\": [\r\n    {\r\n      \"SkillName\": \"string\"\r\n    }\r\n  ],\r\n  \"Languages\": [\r\n    {\r\n      \"LanguageName\": \"string\",\r\n      \"ProficiencyLevel\": \"string\"\r\n    }\r\n  ],\r\n  \"Citizenships\": [\r\n    {\r\n      \"Country\": \"string\",\r\n      \"City\": \"string\",\r\n      \"State\": \"string\",\r\n      \"UndergruondStation\": \"string\"\r\n    }\r\n  ],\r\n  \"DriverLicenses\": [\r\n    {\r\n      \"Category\": \"string\"\r\n    }\r\n  ],\r\n  \"PortfolioItems\": [\r\n    {\r\n      \"ProjectName\": \"string\",\r\n      \"Url\": \"string\",\r\n      \"Description\": \"string\"\r\n    }\r\n  ],\r\n  \"SalaryExpectations\": {\r\n    \"Amount\": 0,\r\n    \"Currency\": \"string\",\r\n    \"Type\": \"string (Net/Gross)\"\r\n  },\r\n  \"EmploymentType\": {\r\n    \"Type\": \"string (Full-time/Part-time/etc)\"\r\n  }\r\n}\r\n\r\n### RESUME TEXT:";
    private const string BASE_IS_FILE_CV_PROMPT = "You are an AI assistant that helps recognize is this content is a CV. Your answer should be true if this text is CV and otherwise false";
    private const string MODEL = "gemini-2.5-flash";
    private readonly Client _geminiClient;
    private readonly GenerateContentConfig _generationConfig;

    public GeminiAITextChat(Client geminiClient)
    {
        _geminiClient = geminiClient;
        _generationConfig = new GenerateContentConfig()
        {
            MaxOutputTokens = 4096,
            Temperature = 0,
            TopP = 1,
            TopK = 0
        };
    }

    public async Task<GenerateContentResponse> GetFormatedText(
        string ask,
        CancellationToken cancellationToken)
    {
        return await _geminiClient.Models.GenerateContentAsync(
            model: MODEL, contents: BASE_FORMATING_SYSTEM_PROMPT + ask, _generationConfig
        );
    }

    public async Task<GenerateContentResponse> IsCV(
        string ask,
        CancellationToken cancellationToken)
    {
        return await _geminiClient.Models.GenerateContentAsync(
            model: MODEL, contents: BASE_IS_FILE_CV_PROMPT + ask, _generationConfig
        );
    }
}