using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace metro1
{

    public class MetricCalculator
    {
        private int totalOperators;
        private int totalOperands;

        public void CountMetrics(ParseTreeNode node)
        {
            if (node.Term != null)
            {
                if (node.Term.Name == "operator")
                    totalOperators++;
                else if (node.Term.Name == "operand")
                    totalOperands++;
            }

            foreach (var child in node.ChildNodes)
            {
                CountMetrics(child);
            }
        }

        public (int operators, int operands) GetMetrics(string scalaCode)
        {
            var grammar = new ScalaGrammar();
            var parser = new Parser(grammar);
            var parseTree = parser.Parse(scalaCode);

            if (parseTree.HasErrors())
            {
                foreach (var error in parseTree.ParserMessages)
                {
                    Console.WriteLine(error.Message);
                }
                return (0, 0);
            }

            CountMetrics(parseTree.Root);
            return (totalOperators, totalOperands);
        }
    }
}
