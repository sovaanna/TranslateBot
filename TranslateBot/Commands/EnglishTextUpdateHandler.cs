using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using TranslateService;

namespace TranslateBot.Commands
{
    public class EnglishTextUpdateHandler : UpdateHandlerBase
    {
        private readonly ITranslationService _service;
        private readonly ILogger _logger;

        public EnglishTextUpdateHandler(ITranslationService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }
        
        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            var msgText = update.Message.Text;
            return !(msgText == null ||
                   msgText.StartsWith('/') ||
                   Regex.IsMatch(msgText, @"\p{IsCyrillic}"));
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            try
            {
                string translation = await _service.EnglishToRussian(update.Message.Text);
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, translation);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception in {BotName}: {Error}", nameof(TranslateBot), ex.Message);
            }

            return UpdateHandlingResult.Handled;
        }
    }
}
