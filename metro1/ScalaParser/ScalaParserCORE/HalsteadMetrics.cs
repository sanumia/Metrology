using Antlr4.Runtime.Tree;

namespace ScalaParserCORE
{
    public class MetricCalculator
    {
        public int TotalOperators => totalOperators;
        public int TotalOperands => totalOperands;
        public Dictionary<string, int> OperatorCounts => operatorCounts;
        public Dictionary<string, int> OperandCounts => operandCounts;

        private int totalOperators = 0;
        private int totalOperands = 0;
        private Dictionary<string, int> operatorCounts = new Dictionary<string, int>();
        private Dictionary<string, int> operandCounts = new Dictionary<string, int>();

        public void AnalyzeTree(IParseTree node)
        {
            if (node is TerminalNodeImpl terminal)
            {
                string tokenText = terminal.GetText();

                if (IsOperator(tokenText))
                {
                    totalOperators++;
                    if (operatorCounts.ContainsKey(tokenText))
                        operatorCounts[tokenText]++;
                    else
                        operatorCounts[tokenText] = 1;
                }
                else if (IsOperand(tokenText))
                {
                    totalOperands++;
                    if (operandCounts.ContainsKey(tokenText))
                        operandCounts[tokenText]++;
                    else
                        operandCounts[tokenText] = 1;
                }
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                AnalyzeTree(node.GetChild(i));
            }
        }

        private bool IsOperator(string tokenText)
        {
            string[] operators = {
                "+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">=",
                "&&", "||", "::", ".", "=>", "match", "def", "val", "var",
                "if", "else", "while", "for", "yield", "map", "filter",
                "foreach", "reduce", "foldLeft", "zip", "mkString"
            };
            return Array.Exists(operators, op => op == tokenText);
        }

        private bool IsOperand(string tokenText)
        {
            // Если токен - число
            if (int.TryParse(tokenText, out _) || double.TryParse(tokenText, out _))
                return true;

            // Если токен - строка в кавычках
            if (tokenText.StartsWith("\"") && tokenText.EndsWith("\""))
                return true;

            // Если токен - переменная (идентификатор)
            return char.IsLetter(tokenText[0]) || tokenText.StartsWith("_");
        }
    }
}
