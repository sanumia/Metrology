using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace ScalaParserCORE
{
    public class GilbMetricCalculator
    {
        public int CL { get; private set; } = 0;      // Количество условий if/match
        public double cl { get; private set; } = 0;
        public int CLI { get; private set; } = 0;     // Максимальная вложенность
        public int TotalStatements { get; private set; } = 0;
        public Dictionary<string, int> OperatorCounts => operatorCounts;
        public Dictionary<string, int> OperandCounts => operandCounts;

        private int totalOperands = 0;
        private double totalOperators = 0;

        private Dictionary<string, int> operatorCounts = new Dictionary<string, int>();
        private Dictionary<string, int> operandCounts = new Dictionary<string, int>();
        private Stack<int> conditionalStack = new Stack<int>(); // Стек для отслеживания вложенности

        public void AnalyzeTree(IParseTree node)
        {
            if (node is TerminalNodeImpl terminal)
            {
                string tokenText = terminal.GetText();

                switch (tokenText)
                {
                    case "if":
                        CL++;
                        conditionalStack.Push(conditionalStack.Count + 1);
                        CLI = Math.Max(CLI, conditionalStack.Peek());
                        break;

                    case "match":
                        // Для match увеличиваем CL, но не учитываем вложенность
                        CL++;
                        conditionalStack.Push(conditionalStack.Count + 1);
                        CLI = Math.Max(CLI, conditionalStack.Peek());
                        break;
                }

                if (IsOperator(tokenText))
                {
                    totalOperators++;
                    TotalStatements++;

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

            // Обработка блоков
            if (node is Antlr4.Runtime.ParserRuleContext ctx)
            {
                bool isConditionalBlock = false;

                // Проверяем, начинается ли блок с if или match
                if (node.ChildCount > 0 && node.GetChild(0) is TerminalNodeImpl firstChild)
                {
                    string firstToken = firstChild.GetText();
                    isConditionalBlock = firstToken == "if" || firstToken == "match";
                }

                if (isConditionalBlock)
                {
                    for (int i = 0; i < node.ChildCount; i++)
                    {
                        AnalyzeTree(node.GetChild(i));
                    }

                    // Уменьшаем стек только для if
                    if (node.GetChild(0) is TerminalNodeImpl term && (term.GetText() == "if" || term.GetText() == "match"))
                    {
                        if (conditionalStack.Count > 0)
                            conditionalStack.Pop();
                    }
                }
                else
                {
                    for (int i = 0; i < node.ChildCount; i++)
                    {
                        AnalyzeTree(node.GetChild(i));
                    }
                }
            }
            else
            {
                for (int i = 0; i < node.ChildCount; i++)
                {
                    AnalyzeTree(node.GetChild(i));
                }
            }
        }

        private bool IsOperator(string tokenText)
        {
            string[] operators = {
                "+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">=",
                "&&", "||", "::", ".", "=>", "match", "case", "def", "val", "var",
                "if", "else", "while", "for", "yield", "map", "filter",
                "foreach", "reduce", "foldLeft", "println", "args"
            };
            return Array.Exists(operators, op => op == tokenText);
        }

        private bool IsOperand(string tokenText)
        {
            if (int.TryParse(tokenText, out _) || double.TryParse(tokenText, out _))
                return true;

            if (tokenText.StartsWith("\"") && tokenText.EndsWith("\""))
                return true;

            return char.IsLetter(tokenText[0]) || tokenText.StartsWith("_");
        }
    }
}