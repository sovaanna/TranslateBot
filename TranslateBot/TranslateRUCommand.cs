﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace TranslateBot
{
    public class TranslateRUCommand : UpdateHandlerBase
    {
        public TranslateRUCommand() { }

        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return (!update.Message.Text?.StartsWith("/") ?? false)
                && Regex.IsMatch(update.Message.Text, @"\p{IsCyrillic}");
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            string translation = await YandexTranslate(update.Message.Text, "ru-en");

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
