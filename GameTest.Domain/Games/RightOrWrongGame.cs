using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Domain.Games
{
    public class RightOrWrongGame : Game<RightOrWrongRound, bool>
    {

        public RightOrWrongGame()
        {
            this.StartNewRound();
        }

        public override void DeclareTimeOut(string connectionId, int roundNumber)
        {
            lock (roundLock)
            {
                if (currentRoundNumber == roundNumber)
                {
                    playerAnswers.TryAdd(connectionId, false);
                }
            }
        }

    }

    public class RightOrWrongRound : Round<bool>
    {
        public RightOrWrongRound()
        {
            var rnd = new Random();

            float operand1 = rnd.Next(1, 10);
            float operand2 = rnd.Next(1, 10);

            var operation = Operations[rnd.Next(0, Operations.Count - 1)];
            BuildExpression(operand1, operand2, operation, rnd.NextDouble() > 0.5);
        }

        public void BuildExpression(float operand1, float operand2, MathOperation operation, bool useRightAnswer)
        {
            float result = MathF.Round(operation.Calculate(operand1, operand2), 2);

            if (useRightAnswer)
            {
                this.Answer = true;
                this.Expression = $"{operand1} {operation.Operator} {operand2} = {result}";
            }
            else
            {
                this.Answer = false;
                var rnd = new Random();
                int falseResult;

                do
                {
                    falseResult = rnd.Next(1, 10);
                } while (falseResult == result);

                this.Expression = $"{operand1} {operation.Operator} {operand2} = {falseResult}";
            }
        }

        public override bool TestAnswer(bool answer)
        {
            return this.Answer == answer;
        }
    }
}
