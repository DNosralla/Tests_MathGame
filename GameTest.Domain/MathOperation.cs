using System;

namespace GameTest.Domain
{
    public class MathOperation
    {
        public Func<float, float, float> Calculate;
        public string Operator { get; private set; }

        public MathOperation(string @operator, Func<float, float, float> operation)
        {
            this.Calculate = operation;
            this.Operator = @operator;
        }
    }
}
