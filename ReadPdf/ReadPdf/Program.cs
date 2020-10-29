using System;
using System.Collections.Generic;
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
            
            if (args.Length > 1)
            {
                var dstPath = args[1];
                ReadPdf(srcPath, dstPath);
            }
            else
            {
                Console.WriteLine(ReadPdf(srcPath));
            }
        }

        private static void ReadPdf(string filenameIn, string filenameOut)
        {
            using var pdfReader = new PdfReader(filenameIn);
            var pages = new List<string>();
            
            File.Delete(filenameOut);
            
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
                
                File.AppendAllText(filenameOut, pageExtractedText + Environment.NewLine, Encoding.UTF8);
            }
        }
        
        private static string ReadPdf(string filename)
        {
            using var pdfReader = new PdfReader(filename);
            var stringBuilder = new StringBuilder();
            var pages = new List<string>();
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
                stringBuilder.Append(pageExtractedText);
            }

            return stringBuilder.ToString();
        }
    }
}