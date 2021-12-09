using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBotConversationalMasIA.Infrastructure
{
    public class LuisRecognizerService: ILuisRecognizerService
    {
        public LuisRecognizer _recognizer { get; private set; }        

        public LuisRecognizerService(IConfiguration configuration)
        {
            var luisAplication = new LuisApplication(
                configuration["LuisAppId"],
                configuration["LuisAPIKey"],
                configuration["LuisHostName"]
            );
            var recognizerOptions = new LuisRecognizerOptionsV3(luisAplication)
            {
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }
            };
            _recognizer = new LuisRecognizer(recognizerOptions);
        }        
    }
}
