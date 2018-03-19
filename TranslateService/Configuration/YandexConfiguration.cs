using Microsoft.Extensions.Configuration;

namespace TranslateService.Configuration
{
    public class YandexConfiguration
    {
        public YandexConfiguration(IConfiguration configuration)
        {
            var yandexConfiguartion = configuration.GetSection("YandexTranslatorConfiguration");
            Address = yandexConfiguartion["address"];
            Key = yandexConfiguartion["yandex_api_key"];
        }

        public string Address { get; }

        public string Key { get; }
    }
}
