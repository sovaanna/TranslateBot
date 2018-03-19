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
    public class RussianTextUpdateHandler : UpdateHandlerBase
    {
        private readonly ITranslationService _service;
        private readonly ILogger _logger;

        public RussianTextUpdateHandler(ITranslationService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
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
            try
            {
                string translation = await _service.TranslateRussian(update.Message.Text);

                _logger.Information("Sending translation Results in {BotName}", nameof(TranslateBot));
                await bot.Client.SendTextMessageAsync(update.Message.Chat.Id, translation);
                _logger.Information("Translation was sent to {BotName} successfully", nameof(TranslateBot));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception in {BotName}: {Error}", nameof(TranslateBot), ex.Message);
            }

            return UpdateHandlingResult.Handled;
        }
    }
}
