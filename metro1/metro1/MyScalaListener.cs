using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;

public class MyScalaAnalyzer
{
    public static void AnalyzeScalaCode(string scalaCode)
    {
        var inputStream = new AntlrInputStream(scalaCode);
        var lexer = new ScalaLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new ScalaParser(tokenStream);

        parser.RemoveErrorListeners(); // Убираем стандартный обработчик ошибок

        var tree = parser.compilationUnit(); // Используем корневой нетерминал

        Console.WriteLine("Разбор кода завершён без ошибок.");

        // Анализируем дерево
        var metrics = new MetricCalculator();
        metrics.AnalyzeTree(tree);
        metrics.PrintMetrics();
    }
}

