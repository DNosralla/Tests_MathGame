using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameTest.Domain.Games;

namespace GameTest.Tests
{
    [TestClass]
    public class GuessTheAnswerGameTests
    {
        [TestMethod]
        public void CreatePlayer()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");
         
            Assert.IsNotNull(player);
            Assert.IsNotNull(player.ConnectionId);
            Assert.IsNotNull(player.Name);
            Assert.AreEqual(0, player.Score);
        }

        [TestMethod]
        public void Increase_Score_With_Win()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");

            Assert.AreEqual(0, player.Score);

            var round = game.CurrentRound();
            var result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer);

            Assert.AreEqual(1, player.Score);
        }

        [TestMethod]
        public void Dont_Increase_Score_With_Fail()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");

            Assert.AreEqual(0, player.Score);

            var round = game.CurrentRound();
            var result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer+1);

            Assert.AreEqual(0, player.Score);
        }

        [TestMethod]
        public void StartNewRound()
        {
            var game = new GuessTheAnswerGame();

            var rounds = game.AllRounds();
            Assert.AreEqual(1, rounds.Count());

            var newRound = game.StartNewRound();

            rounds = game.AllRounds();
            Assert.AreEqual(2, rounds.Count());

            Assert.AreEqual(newRound, game.CurrentRound());
        }

        [TestMethod]
        public void GetCurrentRound()
        {
            var game = new GuessTheAnswerGame();
            var round = game.CurrentRound();

            Assert.IsNotNull(round);
        }

        [TestMethod]
        public void CloseCurrentRound()
        {
            var game = new GuessTheAnswerGame();
            game.CloseCurrentRound();
            var round = game.CurrentRound();
            Assert.IsNotNull(round.Winner);
        }

        [TestMethod]
        public void GetAllRounds()
        {
            var game = new GuessTheAnswerGame();

            var rounds = game.AllRounds();
            Assert.AreEqual(1, rounds.Count());

            game.StartNewRound();

            rounds = game.AllRounds();

            Assert.AreEqual(2, rounds.Count());
        }

        [TestMethod]
        public void KeeepMaxRounds()
        {
            var game = new GuessTheAnswerGame();
            
            for (int i = 0; i < GuessTheAnswerGame.MAX_ROUNDS*2; i++)
            {
                game.StartNewRound();
            }

            Assert.AreEqual(GuessTheAnswerGame.MAX_ROUNDS, game.AllRounds().Count());
        }

        [TestMethod]
        public void Answer_OK()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");
            var round = game.CurrentRound();
            var result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Answer_Fail()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");
            var round = game.CurrentRound();
            var result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer+1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Cant_Answer_More_Then_Once()
        {
            var game = new GuessTheAnswerGame();
            var player = game.CreateNewPlayer("0");
            var round = game.CurrentRound();

            //firts try with wrong answer
            var result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer+1);
            Assert.IsFalse(result);

            //then try with right answer
            result = game.GiveAnswer(player.ConnectionId, round.RoundNumber, round.Answer);
            Assert.IsFalse(result);
        }
    }
}
