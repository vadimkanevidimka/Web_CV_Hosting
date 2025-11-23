using System.Text;
using UglyToad.PdfPig;
using static UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor.ContentOrderTextExtractor;

namespace CVRecognizingService.Application.Helpers.PDFRecognizing;

public class PDFRecognizer
{
    private StringBuilder? _recognizedText = new();
    public string? RecognizedText { get => _recognizedText!.ToString(); }

    public PDFRecognizer(byte[] filebytes)
    {
        using (var pdf = PdfDocument.Open(filebytes))
        {
            foreach (var page in pdf.GetPages())
            {
                _recognizedText.Append(
                    GetText(page, new Options()
                    {
                        ReplaceWhitespaceWithSpace = true,
                        SeparateParagraphsWithDoubleNewline = false,
                    })
                );
            }

        }
    }
}
