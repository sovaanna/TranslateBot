using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace TranslateService
{
    public class YandexTranslate : ITranslationService
    {
        private readonly Uri _endpoint;

        public YandexTranslate(IConfiguration configuration)
        {
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
                    var response = await client.GetStringAsync(strUrl.ToString());
                    var joResponse = JObject.Parse(response);
                    var translationArray = (JArray)joResponse["text"];
                    return string.Join(" | ", translationArray);
                }
                catch (Exception ex)
                {
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
                    var response = await client.GetStringAsync(strUrl.ToString());
                    var joResponse = JObject.Parse(response);
                    var translationArray = (JArray)joResponse["text"];
                    return string.Join(" | ", translationArray);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        //public static async Task<string> YandexTranslate(string inputText, string language)
        //{
        //    var strUrl = new StringBuilder();
        //    strUrl.Append("https://translate.yandex.net/api/v1.5/tr.json/translate?")
        //        .Append("key=trnsl.1.1.20180228T153003Z.b3b5192753962cd2.862bae356f3567fdd64c0ebcca3aea9df6fdbfb3");
        //    strUrl.AppendFormat("&lang={0}&text={1}", language, inputText);
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            var response = await client.GetStringAsync(strUrl.ToString());
        //            JObject joResponse = JObject.Parse(response);
        //            JArray translationArray = (JArray)joResponse["text"];
        //            return string.Join(" | ", translationArray);
        //        }
        //        catch (Exception ex)
        //        {
        //            string checkResult = "Error " + ex.ToString();
        //            client.Dispose();
        //            return checkResult;
        //        }
        //    }
        //}
    }
}
