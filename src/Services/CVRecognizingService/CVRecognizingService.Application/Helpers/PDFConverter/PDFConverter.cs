using Microsoft.AspNetCore.Http;

namespace CVRecognizingService.Application.Helpers.PDFConverter;

public static class FiletoBytesExtension
{
    public async static Task<byte[]> GetBytesAsync(
        this IFormFile file, 
        CancellationToken token)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream, token);

            return memoryStream.ToArray();
        }
    }
}
