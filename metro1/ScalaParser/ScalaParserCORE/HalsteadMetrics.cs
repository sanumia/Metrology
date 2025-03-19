using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

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

        private bool insideIf = false;
        private bool insideMatch = false;

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
            HashSet<string> operators = new HashSet<string>
            {
                "+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">=",
                "&&", "||", "::", ".", "=>", "<-", "++", "--", "+=", "-=", "*=",
                "/=", ",", ":", ";", "_", "def", "val", "var", "while", "for",
                "yield", "foreach"
            };

            if (tokenText == "if")
            {
                insideIf = true;
                return true;
            }

            if (tokenText == "else" && insideIf)
            {
                insideIf = false;
                return false; // else уже учтён вместе с if
            }

            if (tokenText == "match")
            {
                insideMatch = true;
                return true;
            }

            if (tokenText == "case" && insideMatch)
            {
                insideMatch = false;
                return false; // case уже учтён вместе с match
            }

            return operators.Contains(tokenText);
        }

        private bool IsOperand(string tokenText)
        {
            if (int.TryParse(tokenText, out _) || double.TryParse(tokenText, out _))
                return true;

            if ((tokenText.StartsWith("\"") && tokenText.EndsWith("\"")) ||
            (tokenText.StartsWith("\"\"") && tokenText.EndsWith("\"\"")))
                return true;

            HashSet<string> keywords = new HashSet<string>
            {
                "def", "val", "var", "if", "else", "while", "for", "match", "case", "class",
                "object", "trait", "extends", "with", "new", "return", "yield"
            };

            return !keywords.Contains(tokenText) && (char.IsLetter(tokenText[0]) || tokenText.StartsWith("_") || tokenText.Contains("."));
        }
    }
}
