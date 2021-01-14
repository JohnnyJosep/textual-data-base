using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using MediatR;

namespace TextualApi.Application.Handlers.PdfConverter.Commands.PdfConvert
{
    public class PdfConvertCommandHandler : IRequestHandler<PdfConvertCommand, string>
    {
        public Task<string> Handle(PdfConvertCommand request, CancellationToken cancellationToken)
        {
            using var pdfReader = new PdfReader(request.PdfBytes);
            var pages = new List<string>();

            var stringBuilder = new StringBuilder();
            
            for (var i = 1; i <= pdfReader.NumberOfPages; ++i)
            {
            
                var extractedText = PdfTextExtractor.GetTextFromPage(pdfReader, i, new SimpleTextExtractionStrategy());

                var convertEncoding = Encoding.Convert(
                    Encoding.Default,
                    Encoding.UTF8,
                    Encoding.Default.GetBytes(extractedText));
                var pageExtractedText = Encoding.UTF8.GetString(convertEncoding);
                
                if (pages.Contains(pageExtractedText)) continue;
                pages.Add(pageExtractedText);

                stringBuilder.AppendLine(pageExtractedText);
            }

            return Task.FromResult(stringBuilder.ToString());
        }
    }
}