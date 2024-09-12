using CVRecognizingService.Application.PDFRecognizing;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess;
using DotnetGeminiSDK.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CVRecognizingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private const string BASE_SYSTEM_PROMT = "You are an AI assistant that helps people make structural response about person from text in json format with this template: {\r\n  \"Full Name\": \"\",\r\n  \"Contacts\": {\r\n    \"Phone\": \"\",\r\n    \"Email\": \"\",\r\n    \"LinkedIn\": \"\"\r\n  },\r\n  \"Location\": \"\",\r\n  \"Links\": {\r\n    \"GitHub\": \"\"\r\n  },\r\n  \"Job Title\": \"\",\r\n  \"Professional Experience\": [\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n      ]\r\n    },\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n        \"\",\r\n      ]\r\n    },\r\n    {\r\n      \"Company\": \"\",\r\n      \"Location\": \"\",\r\n      \"Start Date\": \"\",\r\n      \"End Date\": \"\",\r\n      \"Responsibilities\": [\r\n        \"\",\r\n        \"\",\r\n        \"\"\r\n      ]\r\n    }\r\n  ],\r\n  \"Education\": {\r\n    \"Degree\": \"\",\r\n    \"University\": \"\",\r\n    \"Year\": integer\r\n  },\r\n  \"Languages\": {\r\n    \"Programming Languages\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Back-end\": [\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Front-end\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Infrastructure\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ],\r\n    \"Metrics\": [\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\",\r\n      \"\"\r\n    ]\r\n  }\r\n} with this text ";
        private readonly IGeminiClient _geminiClient;
        private readonly ILogger _logger;
        private readonly CVRecDBContext _dbcontext;
        public DocumentController(IGeminiClient geminiClient, ILogger<DocumentController> logger, CVRecDBContext context) 
        {
            _geminiClient = geminiClient;
            _logger = logger;
            _dbcontext = context;
        }

        [HttpPost("upload")]
        public async Task<IResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest("No file uploaded.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Здесь можно обработать файл, например, сохранить его или распознать
                    byte[] fileBytes = memoryStream.ToArray();

                    var pdfRecognizer = new PDFRecognizer(fileBytes);


                    var token = new CancellationTokenSource().Token;

                    _logger.LogTrace($"text from PDF was successfully imported {pdfRecognizer.RecognizedText}");

                    var result = await _geminiClient.TextPrompt(BASE_SYSTEM_PROMT + pdfRecognizer.RecognizedText, new DotnetGeminiSDK.Model.Request.GenerationConfig()
                    {
                        MaxOutputTokens = 4096,
                        Temperature = 0,
                        TopP = 1,
                        TopK = 0
                    });

                    var doc = new BaseDocument
                    (
                        contentType: file.ContentType,
                        fileName: file.FileName,
                        filePath: file.Name,
                        fileSize: file.Length,
                        uploadedAt: DateTime.UtcNow,
                        user: new User("Vadim", "vadimdd5@gmail.com")
                    );

                    doc.ProcessedData = new ProcessedData(doc.Id, result?.Candidates[0].Content.Parts[0].Text, DateTime.UtcNow, doc);

                    await _dbcontext.Documents.AddAsync(doc, token);
                    await _dbcontext.SaveChangesAsync(token);

                    return Results.Ok(doc.Id);

                }
            }
            catch(Exception ex)
            {
                return Results.BadRequest("Wrong file");
            }
        }

        [HttpPost("GetAll")]
        public IResult GetAll()
        {
            var token = new CancellationTokenSource().Token;
            return Results.Ok(_dbcontext.Set<BaseDocument>().AsEnumerable<BaseDocument>());
        }
    }
}
