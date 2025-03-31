using Antlr4.Runtime.Tree;
using System;

namespace ScalaParserCORE
{
    public class GilbMetricCalculator
    {
        public int AbsoluteComplexity { get; private set; }
        public double RelativeComplexity =>
            TotalStatements == 0 ? 0 : AbsoluteComplexity / (double)TotalStatements;
        public int MaxNestingLevel { get; private set; }
        public int TotalStatements { get; private set; }

        private int currentNestingLevel;

        public void AnalyzeTree(IParseTree node)
        {
            if (IsConditionalNode(node))
            {
                AbsoluteComplexity++;
                currentNestingLevel++;
                MaxNestingLevel = Math.Max(MaxNestingLevel, currentNestingLevel);
            }

            if (IsStatementNode(node))
            {
                TotalStatements++;
            }

            for (int i = 0; i < node.ChildCount; i++)
            {
                AnalyzeTree(node.GetChild(i));
            }

            if (IsConditionalNode(node))
            {
                currentNestingLevel--;
            }
        }

        private static bool IsConditionalNode(IParseTree node)
        {
            return node is TerminalNodeImpl terminal &&
                   (terminal.GetText().ToLower() is "if" or "else" or "match");
        }

        private static bool IsStatementNode(IParseTree node)
        {
            if (node is not TerminalNodeImpl terminal) return false;

            string[] statements = { "=", "=>", "<-", "if", "else", "while", "for",
                                   "try", "catch", "finally", "return", "throw",
                                   "match", "case", "def", "val", "var" };
            return Array.Exists(statements, s => s == terminal.GetText().ToLower());
        }

        public void PrintMetrics()
        {
            Console.WriteLine("Метрика Джилба:");
            Console.WriteLine($"Абсолютная сложность (CL): {AbsoluteComplexity}");
            Console.WriteLine($"Относительная сложность (cl): {RelativeComplexity:0.000}");
            Console.WriteLine();
            Console.WriteLine("Расширение метрики Джилба:");
            Console.WriteLine($"Максимальный уровень вложенности (CLI): {MaxNestingLevel}");
        }
    }
}