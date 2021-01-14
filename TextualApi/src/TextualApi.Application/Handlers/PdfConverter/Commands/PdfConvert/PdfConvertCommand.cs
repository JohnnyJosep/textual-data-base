using System.IO;
using MediatR;

namespace TextualApi.Application.Handlers.PdfConverter.Commands.PdfConvert
{
    public class PdfConvertCommand : IRequest<string>
    {
        public byte[] PdfBytes { get; set; }
    }
}