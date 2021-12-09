// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using ChatBotConversationalMasIA.Infrastructure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Services.FihogarService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace ChatBotConversationalMasIA
{
    public class ChatBotLUIS : ActivityHandler
    {
        protected readonly ILuisRecognizerService _luisRecognizerService;
        protected readonly IFihogarService _iFihogarService;

        public ChatBotLUIS(ILuisRecognizerService luisRecognizerService, IFihogarService iFihogarService)
        {
            _luisRecognizerService = luisRecognizerService;
            _iFihogarService = iFihogarService;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }

        public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            return base.OnTurnAsync(turnContext, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var recognizeResult = await _luisRecognizerService._recognizer.RecognizeAsync(turnContext, cancellationToken);
            await ManageIntentions(turnContext, recognizeResult, cancellationToken);
        }

        private async Task ManageIntentions(ITurnContext<IMessageActivity> turnContext, RecognizerResult recognizeResult, CancellationToken cancellationToken)
        {
            var topIntent = recognizeResult.GetTopScoringIntent();
            switch (topIntent.intent)
            {
                case "Saludar":
                    await IntentSaludar(turnContext, recognizeResult, cancellationToken);
                    break;

                case "Agradecer":
                    await IntentAgradecer(turnContext, recognizeResult, cancellationToken);
                    break;

                case "PrestarDinero":
                    await IntentPrestarDinero(turnContext, recognizeResult, cancellationToken);
                    break;

                case "None":
                    await IntentNone(turnContext, recognizeResult, cancellationToken);
                    break;

                default:
                    break;
            }
        }

        private async Task IntentSaludar(ITurnContext<IMessageActivity> turnContext, object recognizeResult, CancellationToken cancellationToken)
        {
            var userName = turnContext.Activity.From.Name;
            await turnContext.SendActivityAsync($"Hola {userName}, como te puedo ayudar?", cancellationToken: cancellationToken);
        }

        private async Task IntentAgradecer(ITurnContext<IMessageActivity> turnContext, object recognizeResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"No te preocupes me gusta ayudar", cancellationToken: cancellationToken);
        }

        private async Task IntentPrestarDinero(ITurnContext<IMessageActivity> turnContext, object recognizeResult, CancellationToken cancellationToken)
        {
            var cuentas = await _iFihogarService.GetAccount(); 
            
            await turnContext.SendActivityAsync($"dame le numero de cuenta", cancellationToken: cancellationToken);

            foreach (var item in cuentas.Data.Account)
            {
                foreach (var j in item.Balance)
                {
                    await turnContext.SendActivityAsync($"{item.Currency}, {j.Amount.Amount}", cancellationToken: cancellationToken);

                }


            }

            /*await turnContext.SendActivityAsync($"Nuestros prestamos son los siguientes", cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await turnContext.SendActivityAsync($"$ 100", cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await turnContext.SendActivityAsync($"$ 300", cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await turnContext.SendActivityAsync($"$ 500", cancellationToken: cancellationToken);*/
        }

        private async Task IntentNone(ITurnContext<IMessageActivity> turnContext, object recognizeResult, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync($"No entiendo lop que me dices", cancellationToken: cancellationToken);
        }

        /*private async Task ConsultarBalance(ITurnContext<IMessageActivity> turnContext, object recognizeResult, CancellationToken cancellationToken)
        {
            var cuentas = await _iFihogarService.GetAccount();

            foreach (var item in cuentas.Data.Account)
            {
                foreach (var j in item.Balance)
                {
                    await turnContext.SendActivityAsync($"{j.Amount.Amount}", cancellationToken: cancellationToken);

                }


            }
        }*/

    }
}
