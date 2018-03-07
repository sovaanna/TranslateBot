using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TranslateService
{
    public class YandexTranslate : ITranslator
    {
        private readonly Uri _endpoint;

        public YandexTranslate(IConfiguration configuration)
        {
            string apiKey = configuration["yandex_api_key"];
            _endpoint = new Uri("address" + apiKey);
        }
        public Task<string> RussianToEnglish(string text)
        {
            throw new NotImplementedException();
        }

        public Task<string> EnglishToRussian(string text)
        {
            throw new NotImplementedException();
        }
    }
}
