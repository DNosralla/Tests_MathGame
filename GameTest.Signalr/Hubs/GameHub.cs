using Microsoft.AspNetCore.SignalR;
using GameTest.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameTest.Domain.Games;
using Microsoft.AspNetCore.Sockets;
using System.Diagnostics;

namespace GameTest.Signalr.Hubs
{
    public abstract class GameHub<TRound, TAnswer> : Hub
        where TRound : Round<TAnswer>, new()
    {
        private readonly Game<TRound, TAnswer> game;
        
        public GameHub(Game<TRound, TAnswer> game)
        {
            this.game = game;
        }

        public override async Task OnConnectedAsync()
        {
            var newPlayer = game.CreateNewPlayer(Context.ConnectionId);

            if (game.PlayersOnline().Count() >= 10)
            {
                await Clients.Client(Context.ConnectionId).InvokeAsync("ServerFull");
                Context.Connection.Abort();
            }

            await Clients.Client(Context.ConnectionId).InvokeAsync("PlayerCreated", newPlayer);
            await Clients.Client(Context.ConnectionId).InvokeAsync("AddRounds", game.AllRounds());
            await PlayerJoined(newPlayer);
            await UpdatePlayersList();
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var playerDisconnected = game.RemoveUser(Context.ConnectionId);

            await UpdatePlayersList();
            await PlayerLeft(playerDisconnected);

            await base.OnDisconnectedAsync(exception);
        }
        
        public async Task GiveAnswer(int roundNumber, TAnswer answer)
        {
            var round = game.CurrentRound();

            if (game.GiveAnswer(Context.ConnectionId, roundNumber, answer))
            {
                //update current round
                await UpdateRound(round);

                //update scores
                await UpdatePlayersList();

                //round won message
                await RoundEndedMessage(round);

                //start new round
                await this.StartNewRound();
            } else
            {
                //start new round if all players answered
                if (game.AllPlayersAnswered())
                {
                    await CloseCurrentRound();
                    await StartNewRound();
                }
            }
        }
        
        private Task StartNewRound()
        {
            var newRound = game.StartNewRound();
            return Clients.All.InvokeAsync("AddRounds", new TRound[] { newRound });
        }

        private Task Message(string message)
        {
            return Clients.All.InvokeAsync("Message", message);
        }

        private Task UpdateRound(TRound round)
        {
            return Clients.All.InvokeAsync("UpdateRound", round);
        }
        
        private async Task UpdatePlayersList()
        {
            await Clients.All.InvokeAsync("SetPlayersList", game.PlayersOnline().OrderByDescending(p=>p.Score));
        }
        
        private Task PlayerJoined(Player player)
        {
            return Message($"{player.Name} joined the game");
        }

        private Task PlayerLeft(Player player)
        {
            return Message($"{player.Name} left the game");
        }

        private Task RoundEndedMessage(TRound round)
        {
            return Message($"{round.Winner.Name} won round {round.RoundNumber} <span class='fa fa - trophy'></span>");
        }
        
        private async Task CloseCurrentRound()
        {
            var closedRound = game.CloseCurrentRound();
            await UpdateRound(closedRound);
            await RoundEndedMessage(closedRound);
        }
    }
}
