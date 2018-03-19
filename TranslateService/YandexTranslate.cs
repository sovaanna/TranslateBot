using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using TranslateService.Configuration;
using TranslateService.Enums;
using TranslateService.Extensions;

namespace TranslateService
{
    public class YandexTranslate : ITranslationService
    {
        private readonly Uri _endpoint;
        private readonly ILogger _logger;

        public YandexTranslate(YandexConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _endpoint = new Uri(configuration.Address + configuration.Key);
        }

        public async Task<string> TranslateRussian(string text)
        {
            return await Translate(text, Language.Russian, Language.English);
        }

        public async Task<string> TranslateEnglish(string text)
        {
            return await Translate(text, Language.English, Language.Russian);
        }

        public async Task<string> Translate(string text, Language source, Language destination)
        {
            var sourceLanguage = source.GetStringValue();
            var destinationLanguage = destination.GetStringValue();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _logger.Debug("Starting translation in {TranslationService} for {Text} ({SourceLanguage}-{DestinationLanguage})",
                        nameof(YandexTranslate), text, sourceLanguage, destinationLanguage);

                    var strUrl = $"{_endpoint}&lang={sourceLanguage}-{destinationLanguage}&text={text}";

                    var response = await client.GetStringAsync(strUrl);//"{\"code\":200,\"lang\":\"en - ru\",\"text\":[\"привет\"]}";

                    var deserializedResponce = JsonConvert.DeserializeObject<dynamic>(response);

                    var translations = deserializedResponce.text;
                    var result = string.Join(Environment.NewLine, translations);

                    _logger.Debug("Translation is done in {TranslationService}: {Text} -> {Result}",
                        nameof(YandexTranslate), text, result);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Exception in {TranslationService}: {Error}", nameof(YandexTranslate), ex.Message);
                    return ex.Message;
                }
            }
        }
    }
}
