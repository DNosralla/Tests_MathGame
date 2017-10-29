using Microsoft.AspNetCore.SignalR;
using GameTest.Domain;
using GameTest.Domain.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Signalr.Hubs
{
    public class RightOrWrongGameHub : GameHub<RightOrWrongRound, bool>
    {
        public RightOrWrongGameHub(Game<RightOrWrongRound, bool> game) : base(game)
        {
        }
    }
}
