﻿using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotConversationalMasIA.Infrastructure
{
    public interface ILuisRecognizerService
    {
        LuisRecognizer _recognizer { get; }
    }
}
