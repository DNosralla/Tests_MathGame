using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameTest.Domain.Games;

namespace GameTest.Tests
{
    [TestClass]
    public class RightWrongRoundTests
    {
        [TestMethod]
        public void Addition_True()
        {
            var addition = new RightOrWrongRound();
            
            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "+");
            var expression = $"1 + 1 = 2";

            addition.BuildExpression(1, 1, operation, true);

            Assert.AreEqual(expression, addition.Expression);
            Assert.IsTrue(addition.TestAnswer(true));
            Assert.IsFalse(addition.TestAnswer(false));
        }

        [TestMethod]
        public void Addition_False()
        {
            var addition = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "+");
            var expression = $"1 + 1 = 2";

            addition.BuildExpression(1, 1, operation, false);

            Assert.AreNotEqual(expression, addition.Expression);
            Assert.IsTrue(addition.TestAnswer(false));
            Assert.IsFalse(addition.TestAnswer(true));
        }

        [TestMethod]
        public void Subtraction_True()
        {
            var subtraction = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "-");
            var expression = $"1 - 1 = 0";

            subtraction.BuildExpression(1, 1, operation, true);

            Assert.AreEqual(expression, subtraction.Expression);
            Assert.IsTrue(subtraction.TestAnswer(true));
            Assert.IsFalse(subtraction.TestAnswer(false));
        }

        [TestMethod]
        public void Subtraction_False()
        {
            var subtraction = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "-");
            var expression = $"1 - 1 = 0";

            subtraction.BuildExpression(1, 1, operation, false);

            Assert.AreNotEqual(expression, subtraction.Expression);
            Assert.IsTrue(subtraction.TestAnswer(false));
            Assert.IsFalse(subtraction.TestAnswer(true));
        }

        [TestMethod]
        public void Division_True()
        {
            var division = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "/");
            var expression = $"10 / 2 = 5";

            division.BuildExpression(10, 2, operation, true);

            Assert.AreEqual(expression, division.Expression);
            Assert.IsTrue(division.TestAnswer(true));
            Assert.IsFalse(division.TestAnswer(false));
        }

        [TestMethod]
        public void Division_False()
        {
            var subtraction = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "/");
            var expression = $"10 / 2 = 5";

            subtraction.BuildExpression(10, 2, operation, false);

            Assert.AreNotEqual(expression, subtraction.Expression);
            Assert.IsTrue(subtraction.TestAnswer(false));
            Assert.IsFalse(subtraction.TestAnswer(true));
        }

        [TestMethod]
        public void Multiply_True()
        {
            var multiply = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "*");
            var expression = $"10 * 2 = 20";

            multiply.BuildExpression(10, 2, operation, true);

            Assert.AreEqual(expression, multiply.Expression);
            Assert.IsTrue(multiply.TestAnswer(true));
            Assert.IsFalse(multiply.TestAnswer(false));
        }

        [TestMethod]
        public void Multiply_False()
        {
            var multiply = new RightOrWrongRound();

            var operation = RightOrWrongRound.Operations.First(o => o.Operator == "*");
            var expression = $"10 * 2 = 20";

            multiply.BuildExpression(10, 2, operation, false);

            Assert.AreNotEqual(expression, multiply.Expression);
            Assert.IsTrue(multiply.TestAnswer(false));
            Assert.IsFalse(multiply.TestAnswer(true));
        }


    }
}
