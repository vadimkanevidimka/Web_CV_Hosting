﻿using UglyToad.PdfPig;
using static UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor.ContentOrderTextExtractor;

namespace CVRecognizingService.Application.Helpers.PDFRecognizing;
public class PDFRecognizer
{
    private string? _recognizedText;
    public string? RecognizedText { get => _recognizedText; }

    public PDFRecognizer(byte[] filebytes)
    {
        using (var pdf = PdfDocument.Open(filebytes))
        {
            foreach (var page in pdf.GetPages())
            {
                _recognizedText += GetText(page, new Options()
                {
                    ReplaceWhitespaceWithSpace = true,
                    SeparateParagraphsWithDoubleNewline = false,
                });

                var rawText = page.Text;
            }

        }
    }
}