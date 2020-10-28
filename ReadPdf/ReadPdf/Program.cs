using System;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace ReadPdf
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter file name");
                return;
            }
            var srcPath = args[0];
            var text = ReadPdf(srcPath);

            if (args.Length > 1)
            {
                var dstPath = args[1];
                File.WriteAllText(dstPath, text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }

        private static string ReadPdf(string filename)
        {
            using var pdfReader = new PdfReader(filename);
            var stringBuilder = new StringBuilder();
            var strategy = new SimpleTextExtractionStrategy();
            for (var i = 1; i <= pdfReader.NumberOfPages; ++i)
            {
                var extractedText = PdfTextExtractor.GetTextFromPage(pdfReader, i, strategy);
                var convertEncoding = Encoding.Convert(
                    Encoding.Default,
                    Encoding.UTF8,
                    Encoding.Default.GetBytes(extractedText));
                var pageExtractedText = Encoding.UTF8.GetString(convertEncoding);
                stringBuilder.Append(pageExtractedText);
            }

            return stringBuilder.ToString();
        }
    }
}