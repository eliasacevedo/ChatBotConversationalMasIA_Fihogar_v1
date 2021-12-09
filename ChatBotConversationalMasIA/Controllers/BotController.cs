// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.15.0

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Models;
using Services.FihogarService;
using System.Threading.Tasks;

namespace ChatBotConversationalMasIA.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;
        private readonly IFihogarService FihogarService;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot, IFihogarService fihogarService)
        {
            Adapter = adapter;
            Bot = bot;
            FihogarService = fihogarService;
        }

        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }

        [HttpGet]
        [Route("accounts")]
        public async Task<AccountDetails> GetAccountAsync() {
            return await FihogarService.GetAccount();
        }
    }
}
