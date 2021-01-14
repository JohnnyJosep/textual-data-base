using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.Models.ApplicationEvents;
using TextualDatabaseApi.Application.UnitOfWork;

namespace TextualDatabaseApi.Application.Events.TextualDataCreate
{
    public class TextualDataCreateEventEventHandler : INotificationHandler<ApplicationEventNotification<TextualDataCreateEvent>>
    {
        private readonly ILogger<TextualDataCreateEventEventHandler> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;

        public TextualDataCreateEventEventHandler(ILogger<TextualDataCreateEventEventHandler> logger, IMemoryCache cache, IConfiguration configuration, IUnitOfWork uow)
        {
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            _uow = uow;
        }
        
        public async Task Handle(ApplicationEventNotification<TextualDataCreateEvent> notification, CancellationToken cancellationToken)
        {
            RemoveCache();
            
            var applicationEvent = notification.ApplicationEvent;
            await FreeLingAnalyzer(applicationEvent.TextualData, cancellationToken);
            
            _logger.LogInformation("Textual Database done application event: {ApplicationEvent}", applicationEvent.GetType().Name);
        }

        private void RemoveCache()
        {
            _cache.Remove("get_textual_data");
            _logger.LogInformation("Cache key get_textual_data removed");
        }
        
        private async Task FreeLingAnalyzer(TextualData textualData, CancellationToken cancellationToken = default)
        {
            var analyzer = _configuration.GetValue<string>("FreeLingAnalyzerClient");
            _logger.LogInformation(analyzer);

            var inputFile = Path.GetRandomFileName();
            await File.WriteAllTextAsync(inputFile, textualData.Text, Encoding.UTF8, cancellationToken);

            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = analyzer,
                    Arguments = $"50005 {inputFile}",
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            process.Start();
            var partOfSpeech = await process.StandardOutput.ReadToEndAsync();
            
            File.Delete(inputFile);

            var open = $"<OUTPUT SRCFILE=\"{inputFile}\">".Length;
            var close = "</OUTPUT>".Length;
            partOfSpeech = partOfSpeech
                .Trim()
                .Substring(open, partOfSpeech.Length - (open + close + 2))
                .Trim();
            
            
            
            var textEntry = await _uow.TextEntryRepository.FindAsync(cancellationToken, textualData.Id);
            textEntry.PartOfSpeech = partOfSpeech;
            _uow.TextEntryRepository.Update(textEntry);
            
            _logger.LogInformation("Shell: {result}", partOfSpeech);
        }
    }
}