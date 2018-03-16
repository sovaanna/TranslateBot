using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;

namespace TranslateService
{
    public class YandexTranslate : ITranslationService
    {
        private readonly Uri _endpoint;
        private readonly ILogger _logger; 

        public YandexTranslate(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            var myList = configuration.GetSection("YandexTranslatorOptions");
            string apiKey = myList["yandex_api_key"];
            _endpoint = new Uri(myList["address"] + apiKey);
        }
        public async Task<string> RussianToEnglish(string text)
        {
            var strUrl = new StringBuilder();
            strUrl.AppendFormat("{0}&lang={1}&text={2}", _endpoint, "ru-en", text);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _logger.Information("Starting translation in {TranslationService} for {Text} ({Language})",
                        nameof(YandexTranslate), text, "ru-en");
                    var response = await client.GetStringAsync(strUrl.ToString());
                    var joResponse = JObject.Parse(response);
                    var translationArray = (JArray)joResponse["text"];
                    var result = string.Join(" | ", translationArray);

                    _logger.Information("Translation is done in {TranslationService}: {Text} -> {Result}",
                        nameof(YandexTranslate), text, result);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception in {TranslationService}: {Error}", nameof(YandexTranslate), ex.Message);
                    return ex.Message;
                }
            }
        }

        public async Task<string> EnglishToRussian(string text)
        {
            var strUrl = new StringBuilder();
            strUrl.AppendFormat("{0}&lang={1}&text={2}", _endpoint, "en-ru", text);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _logger.Information("Starting translation in {TranslationService} for {Text} ({Language})",
                        nameof(YandexTranslate), text, "en-ru");
                    var response = await client.GetStringAsync(strUrl.ToString());
                    var joResponse = JObject.Parse(response);
                    var translationArray = (JArray)joResponse["text"];
                    var result = string.Join(" | ", translationArray);

                    _logger.Information("Translation is done in {TranslationService}: {Text} -> {Result}",
                        nameof(YandexTranslate), text, result);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Error("Exception in {TranslationService}: {Error}", nameof(YandexTranslate), ex.Message);
                    return ex.Message;
                }
            }
        }
    }
}
