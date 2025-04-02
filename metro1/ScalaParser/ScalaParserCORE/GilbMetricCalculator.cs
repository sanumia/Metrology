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
        private Stack<int> ifStack = new Stack<int>(); // Стек для отслеживания вложенности

        public void AnalyzeTree(IParseTree node)
        {
            if (node is TerminalNodeImpl terminal)
            {
                string tokenText = terminal.GetText();

                switch (tokenText)
                {
                    case "if":
                    case "match":
                        // Нашли новое условие
                        CL++;
                        ifStack.Push(ifStack.Count + 1); // Увеличиваем глубину
                        CLI = Math.Max(CLI, ifStack.Peek());
                        break;

                    case "{":
                        // Вход в новый блок
                        if (ifStack.Count > 0)
                        {
                            CLI = Math.Max(CLI, ifStack.Peek());
                        }
                        break;

                    case "}":
                        // Выход из блока
                        if (ifStack.Count > 0 && node.Parent.ChildCount > 0)
                        {
                            // Проверяем, был ли это блок условия
                            var firstChild = node.Parent.GetChild(0);
                            if (firstChild is TerminalNodeImpl firstTerminal &&
                                (firstTerminal.GetText() == "if" || firstTerminal.GetText() == "match"))
                            {
                                ifStack.Pop();
                            }
                        }
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