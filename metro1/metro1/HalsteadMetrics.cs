using Irony.Parsing;
using System;
using System.Collections.Generic;

public class MetricCalculator
{
    private int totalOperators;
    private int totalOperands;
    private HashSet<string> uniqueOperators = new HashSet<string>();
    private HashSet<string> uniqueOperands = new HashSet<string>();

    public void CountMetrics(ParseTreeNode node)
    {
        if (node.Term != null)
        {
            if (node.Term.Name == "operator")
            {
                totalOperators++;
                uniqueOperators.Add(node.Term.Name);
            }
            else if (node.Term.Name == "operand")
            {
                totalOperands++;
                uniqueOperands.Add(node.Term.Name);
            }
            else if (node.Term.Name == "identifier" || node.Term.Name == "number")
            {
                totalOperands++;
                uniqueOperands.Add(node.Token.Text);
            }
        }

        foreach (var child in node.ChildNodes)
        {
            CountMetrics(child);
        }
    }

    public (int operators, int operands, int uniqueOperators, int uniqueOperands) GetMetrics(string scalaCode)
    {
        var grammar = new ScalaGrammar();
        var parser = new Parser(grammar);
        var parseTree = parser.Parse(scalaCode);

        if (parseTree.HasErrors())
        {
            Console.WriteLine("Ошибка парсинга кода Scala:");
            foreach (var error in parseTree.ParserMessages)
            {
                Console.WriteLine($"[Строка {error.Location.Line + 1}, Позиция {error.Location.Column + 1}] {error.Message}");

                // Отображение проблемного кода с указателем на ошибку
                string[] lines = scalaCode.Split('\n');
                if (error.Location.Line < lines.Length)
                {
                    string errorLine = lines[error.Location.Line];
                    Console.WriteLine(" " + errorLine);
                    Console.WriteLine(" " + new string(' ', error.Location.Column) + "^ Здесь ошибка");
                }
            }
            return (0, 0, 0, 0);
        }

        CountMetrics(parseTree.Root);
        return (totalOperators, totalOperands, uniqueOperators.Count, uniqueOperands.Count);
    }
}
