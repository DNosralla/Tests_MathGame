using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameTest.Domain.Games;

namespace GameTest.Tests
{
    [TestClass]
    public class GuessTheAnswerRoundTests
    {

        [TestMethod]
        public void Addition()
        {
            var addition = new GuessTheAnswerRound();

            var operation = GuessTheAnswerRound.Operations.First(o => o.Operator == "+");
            var expression = $"1 + 1 = ?";

            addition.BuildExpression(1, 1, operation);

            Assert.AreEqual(expression, addition.Expression);
            Assert.IsTrue(addition.TestAnswer(2));
            Assert.IsFalse(addition.TestAnswer(1));
        }

        [TestMethod]
        public void Subtraction()
        {
            var subtraction = new GuessTheAnswerRound();

            var operation = GuessTheAnswerRound.Operations.First(o => o.Operator == "-");
            var expression = $"4 - 2 = ?";

            subtraction.BuildExpression(4, 2, operation);

            Assert.AreEqual(expression, subtraction.Expression);
            Assert.IsTrue(subtraction.TestAnswer(2));
            Assert.IsFalse(subtraction.TestAnswer(1));
        }

        [TestMethod]
        public void Multiply()
        {
            var multiply = new GuessTheAnswerRound();

            var operation = GuessTheAnswerRound.Operations.First(o => o.Operator == "*");
            var expression = $"2 * 2 = ?";

            multiply.BuildExpression(2, 2, operation);

            Assert.AreEqual(expression, multiply.Expression);
            Assert.IsTrue(multiply.TestAnswer(4));
            Assert.IsFalse(multiply.TestAnswer(1));
        }

        [TestMethod]
        public void Division()
        {
            var division = new GuessTheAnswerRound();

            var operation = GuessTheAnswerRound.Operations.First(o => o.Operator == "/");
            var expression = $"4 / 2 = ?";

            division.BuildExpression(4, 2, operation);

            Assert.AreEqual(expression, division.Expression);
            Assert.IsTrue(division.TestAnswer(2));
            Assert.IsFalse(division.TestAnswer(1));
        }
    }
}
