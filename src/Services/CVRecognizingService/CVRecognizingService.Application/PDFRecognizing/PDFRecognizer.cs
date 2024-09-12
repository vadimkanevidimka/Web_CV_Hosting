using static UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor.ContentOrderTextExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig;

namespace CVRecognizingService.Application.PDFRecognizing
{
    public class PDFRecognizer
    {
        private string _recognizedText;
        public string RecognizedText { get => _recognizedText; }

        public PDFRecognizer(byte[] filebytes) 
        {
            using (var pdf = PdfDocument.Open(filebytes))
            {
                foreach (var page in pdf.GetPages())
                {
                    // Either extract based on order in the underlying document with newlines and spaces.
                    _recognizedText += ContentOrderTextExtractor.GetText(page, new Options() 
                    {
                        ReplaceWhitespaceWithSpace = true,
                        SeparateParagraphsWithDoubleNewline = false,
                    });

                    // Or based on grouping letters into words.
                    var otherText = string.Join(" ", page.GetWords());

                    // Or the raw text of the page's content stream.
                    var rawText = page.Text;
                }

            }
        }
    }
}
