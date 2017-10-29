using GameTest.Domain;
using GameTest.Domain.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Signalr.Hubs
{
    public class GuessTheAnswerGameHub : GameHub<GuessTheAnswerRound, float>
    {
        public GuessTheAnswerGameHub(Game<GuessTheAnswerRound, float> game) : base(game)
        {
        }
    }
}
