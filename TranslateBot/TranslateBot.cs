using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

namespace TranslateBot
{
    public class TranslateBot: BotBase<TranslateBot>
    {
        public TranslateBot(IOptions<BotOptions<TranslateBot>> botOptions) 
            : base(botOptions) { }

        public override Task HandleUnknownUpdate(Update update) => Task.CompletedTask;

        public override Task HandleFaultedUpdate(Update update, Exception e) => Task.CompletedTask;
    }
}
