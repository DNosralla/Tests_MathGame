using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameTest.Domain.Games
{
    public abstract class Game<TRound, TAnswer>
        where TRound : Round<TAnswer>, new()
    {
        public const int MAX_ROUNDS = 20;
        protected object roundLock = new object();
        protected int currentRoundNumber = 0;

        protected readonly ConcurrentDictionary<string, Player> players = new ConcurrentDictionary<string, Player>();
        protected readonly ConcurrentDictionary<string, string> playerNames = new ConcurrentDictionary<string, string>();
        protected readonly Dictionary<string, TAnswer> playerAnswers = new Dictionary<string, TAnswer>();

        protected readonly SortedList<int, TRound> Rounds = new SortedList<int, TRound>(MAX_ROUNDS);

        public Game()
        {

        }

        public bool GiveAnswer(string connectionId, int roundNumber, TAnswer answer)
        {
            lock (roundLock)
            {
                var round = Rounds[roundNumber];

                //check if player already answered
                if (playerAnswers.ContainsKey(connectionId))
                {
                    return false;
                }

                //mark player answer
                playerAnswers.Add(connectionId, answer);

                //check if player still connected
                Player player;
                if (!players.TryGetValue(connectionId, out player))
                {
                    return false;
                }

                var currentRound = Rounds[currentRoundNumber];

                //is the current round open and the answer right?
                if (currentRound.Winner == null && currentRound.RoundNumber == round.RoundNumber && round.TestAnswer(answer))
                {
                    currentRound.Winner = player;
                    player.Score += 1;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public abstract void DeclareTimeOut(string connectionId, int roundNumber);

        public bool AllPlayersAnswered()
        {
            return players.All(p => playerAnswers.ContainsKey(p.Key));
        }

        public TRound StartNewRound()
        {
            lock (roundLock)
            {
                playerAnswers.Clear();

                if (this.Rounds.Count == MAX_ROUNDS)
                {
                    this.Rounds.RemoveAt(0);
                }

                var round = new TRound();
                round.RoundNumber = ++currentRoundNumber;
                this.Rounds.Add(round.RoundNumber, round);
                return round;
            }
        }

        public TRound CloseCurrentRound()
        {
            lock (roundLock)
            {
                var round = CurrentRound();

                if (round.Winner == null)
                {
                    round.Winner = new Player("-1", "Nobody");
                }

                return round;
            }
        }

        public TRound CurrentRound()
        {
            return Rounds.Last().Value;
        }

        public IEnumerable<TRound> AllRounds()
        {
            return Rounds.Values;
        }

        public virtual Player CreateNewPlayer(string connectionId)
        {
            Player newPlayer = new Player(connectionId, string.Empty);

            var count = players.Count;
            do
            {
                count++;
                newPlayer.Name = $"Player {count}";
            } while (!playerNames.TryAdd(newPlayer.Name, newPlayer.ConnectionId));

            players.TryAdd(newPlayer.ConnectionId, newPlayer);

            return newPlayer;
        }

        public virtual Player RemoveUser(string connectionId)
        {
            Player player;

            if (players.TryRemove(connectionId, out player))
            {
                playerNames.TryRemove(player.Name, out var id);
            }

            return player;
        }

        public IEnumerable<Player> PlayersOnline()
            => players.Values.AsEnumerable();
    }
}
