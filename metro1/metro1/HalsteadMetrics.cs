using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

public class MetricCalculator
{
    private int totalOperators = 0;
    private int totalOperands = 0;
    private Dictionary<string, int> operatorCounts = new Dictionary<string, int>();
    private Dictionary<string, int> operandCounts = new Dictionary<string, int>();

    public void AnalyzeTree(IParseTree node)
    {
        if (node is TerminalNodeImpl terminal)
        {
            string tokenText = terminal.GetText();
            int tokenType = terminal.Symbol.Type;

            if (IsOperator(tokenText))
            {
                totalOperators++;
                if (operatorCounts.ContainsKey(tokenText))
                {
                    operatorCounts[tokenText]++;
                }
                else
                {
                    operatorCounts[tokenText] = 1;
                }
            }
            else if (IsOperand(tokenText))
            {
                totalOperands++;
                if (operandCounts.ContainsKey(tokenText))
                {
                    operandCounts[tokenText]++;
                }
                else
                {
                    operandCounts[tokenText] = 1;
                }
            }
        }

        for (int i = 0; i < node.ChildCount; i++)
        {
            AnalyzeTree(node.GetChild(i));
        }
    }

    private bool IsOperator(string tokenText)
    {
        string[] operators = { "+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">=", "&&", "||", "::", ".", "=>", "match", "def", "object", "if", "else" };
        return Array.Exists(operators, op => op == tokenText);
    }

    private bool IsOperand(string tokenText)
    {
        return char.IsLetter(tokenText[0]) || char.IsDigit(tokenText[0]) || tokenText.StartsWith("\"");
    }

    public void PrintMetrics()
    {
        Console.WriteLine("\n📊 Метрики Холстеда:");
        Console.WriteLine($"🔹 Total Operators: {totalOperators}");
        Console.WriteLine($"🔹 Total Operands: {totalOperands}");
        Console.WriteLine($"🔹 Unique Operators: {operatorCounts.Count}");
        Console.WriteLine($"🔹 Unique Operands: {operandCounts.Count}");

        double programLength = totalOperators + totalOperands;
        double vocabularySize = operatorCounts.Count + operandCounts.Count;
        double volume = programLength * Math.Log2(vocabularySize > 0 ? vocabularySize : 1);
        double difficulty = (operatorCounts.Count / 2.0) * (totalOperands / (double)operandCounts.Count);
        double effort = difficulty * volume;

        Console.WriteLine($"\n📏 Дополнительные метрики:");
        Console.WriteLine($"📌 Program Length: {programLength}");
        Console.WriteLine($"📌 Vocabulary Size: {vocabularySize}");
        Console.WriteLine($"📌 Volume: {volume:F2}");
        Console.WriteLine($"📌 Difficulty: {difficulty:F2}");
        Console.WriteLine($"📌 Effort: {effort:F2}");

        Console.WriteLine("\n📊 Операторы и их количество:");
        foreach (var op in operatorCounts)
        {
            Console.WriteLine($"{op.Key}: {op.Value}");
        }

        Console.WriteLine("\n📊 Операнды и их количество:");
        foreach (var operand in operandCounts)
        {
            Console.WriteLine($"{operand.Key}: {operand.Value}");
        }
        Console.WriteLine($"\n🔸 Общее количество уникальных операторов: {operatorCounts.Count}");
        Console.WriteLine($"🔸 Общее количество уникальных операндов: {operandCounts.Count}");
    }
}