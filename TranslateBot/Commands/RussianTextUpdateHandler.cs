using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using TranslateService;

namespace TranslateBot.Commands
{
    public class RussianTextUpdateHandler : UpdateHandlerBase
    {
        private readonly ITranslationService _service;

        public RussianTextUpdateHandler(ITranslationService service)
        {
            _service = service;
        }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            var msgText = update.Message.Text;
            return !(msgText == null ||
                     msgText.StartsWith('/')) &&
                   Regex.IsMatch(msgText, @"\p{IsCyrillic}");
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            string translation = await _service.RussianToEnglish(update.Message.Text);

            try
            {
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, translation);
            }
            catch (Exception ex)
            {
                var checkResult = ex.Message;
            }

            return UpdateHandlingResult.Handled;
        }
    }
}
