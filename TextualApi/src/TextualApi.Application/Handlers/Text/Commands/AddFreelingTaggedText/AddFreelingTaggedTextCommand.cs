using System.Collections.Generic;
using MediatR;

namespace TextualApi.Application.Handlers.Text.Commands.AddFreelingTaggedText
{
    public class AddFreelingTaggedTextCommand : IRequest<string>
    {
        public string Index { get; set; }
        public string TextId { get; set; }
        public string FreelingTaggedText { get; set; }

        public IEnumerable<MorfoFreeling> ToFreeling()
        {
            var lines = FreelingTaggedText.Split('\n');
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                yield return new MorfoFreeling
                {
                    Lemma = parts[0],
                    Word = parts[1],
                    PosTag = parts[2],
                    Confidence = double.Parse(parts[3])
                };
            }
        }
    }

    public class MorfoFreeling
    {
        public string Lemma { get; set; }
        public string Word { get; set; }
        public string PosTag { get; set; }
        public double Confidence { get; set; }
    }
}