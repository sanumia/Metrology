using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

public class MetricCalculator
{
    private int totalOperators = 0;
    private int totalOperands = 0;
    private HashSet<string> uniqueOperators = new HashSet<string>();
    private HashSet<string> uniqueOperands = new HashSet<string>();

    public void AnalyzeTree(IParseTree node)
    {
        if (node is TerminalNodeImpl terminal)
        {
            string tokenText = terminal.GetText();
            int tokenType = terminal.Symbol.Type;

            if (IsOperator(tokenText))
            {
                totalOperators++;
                uniqueOperators.Add(tokenText);
            }
            else if (IsOperand(tokenText))
            {
                totalOperands++;
                uniqueOperands.Add(tokenText);
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
        Console.WriteLine($"🔹 Unique Operators: {uniqueOperators.Count}");
        Console.WriteLine($"🔹 Unique Operands: {uniqueOperands.Count}");

        double programLength = totalOperators + totalOperands;
        double vocabularySize = uniqueOperators.Count + uniqueOperands.Count;
        double volume = programLength * Math.Log2(vocabularySize > 0 ? vocabularySize : 1);
        double difficulty = (uniqueOperators.Count / 2.0) * (totalOperands / (double)uniqueOperands.Count);
        double effort = difficulty * volume;

        Console.WriteLine($"\n📏 Дополнительные метрики:");
        Console.WriteLine($"📌 Program Length: {programLength}");
        Console.WriteLine($"📌 Vocabulary Size: {vocabularySize}");
        Console.WriteLine($"📌 Volume: {volume:F2}");
        Console.WriteLine($"📌 Difficulty: {difficulty:F2}");
        Console.WriteLine($"📌 Effort: {effort:F2}");
    }
}
