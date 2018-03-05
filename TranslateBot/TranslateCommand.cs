using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace TranslateBot
{
    public class TranslateCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }
        public string ArgsInput { get; set; }
    }

    public class TranslateENCommand : UpdateHandlerBase//CommandBase<TranslateCommandArgs>
    {
        public TranslateENCommand() { }

        public static async Task<string> YandexTranslate(string inputText, string language)
        {
            var strUrl = new StringBuilder();
            strUrl.Append("https://translate.yandex.net/api/v1.5/tr.json/translate?")
                .Append("key=trnsl.1.1.20180228T153003Z.b3b5192753962cd2.862bae356f3567fdd64c0ebcca3aea9df6fdbfb3");
            strUrl.AppendFormat("&lang={0}&text={1}", language, inputText);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(strUrl.ToString());
                    JObject joResponse = JObject.Parse(response);
                    JArray translationArray = (JArray)joResponse["text"];
                    return string.Join(" | ", translationArray);
                }
                catch (Exception ex)
                {
                    string checkResult = "Error " + ex.ToString();
                    client.Dispose();
                    return checkResult;
                }
            }
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return (!update.Message.Text?.StartsWith("/") ?? false) 
                && !Regex.IsMatch(update.Message.Text, @"\p{IsCyrillic}");
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            string translation = await YandexTranslate(update.Message.Text, "en-ru");

            try
            {
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, translation);
            }
            catch (Exception ex)
            {
                string checkResult = "Error " + ex.ToString();
            }

            return UpdateHandlingResult.Handled;
        }
    }
}
