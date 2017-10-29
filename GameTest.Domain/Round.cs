using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace GameTest.Domain
{
    public abstract class Round<TAnswer>
    {
        public int RoundNumber { get; set; }
        public string Expression { get; set; }
        public Player Winner { get; set; }

        [JsonIgnore]
        public TAnswer Answer { get; set; }

        public abstract bool TestAnswer(TAnswer answer);

        public static readonly List<MathOperation> Operations = new List<MathOperation>()
        {
            new MathOperation("+", (operand1, operand2) => { return operand1+operand2; }),
            new MathOperation("-", (operand1, operand2) => { return operand1-operand2; }),
            new MathOperation("/", (operand1, operand2) => { return operand1/operand2; }),
            new MathOperation("*", (operand1, operand2) => { return operand1*operand2; })
        };
    }
}
