using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Domain.Games
{
    public class GuessTheAnswerGame : Game<GuessTheAnswerRound, float>
    {

        public GuessTheAnswerGame()
        {
            this.StartNewRound();
        }

        public override void DeclareTimeOut(string connectionId, int roundNumber)
        {
            lock (roundLock)
            {
                if (currentRoundNumber == roundNumber)
                {
                    playerAnswers.TryAdd(connectionId, 0);
                }
            }
        }
    }
    
    public class GuessTheAnswerRound : Round<float>
    {
        public GuessTheAnswerRound()
        {
            var rnd = new Random();

            float operand1 = rnd.Next(1, 10);
            float operand2 = rnd.Next(1, 10);

            var operation = Operations[rnd.Next(0, Operations.Count - 1)];
            BuildExpression(operand1, operand2, operation);
        }

        public void BuildExpression(float operand1, float operand2, MathOperation operation)
        {
            float result = MathF.Round(operation.Calculate(operand1, operand2), 2);

            this.Answer = result;
            this.Expression = $"{operand1} {operation.Operator} {operand2} = ?";
        }

        public override bool TestAnswer(float answer)
        {
            return this.Answer == answer;
        }
    }
}
