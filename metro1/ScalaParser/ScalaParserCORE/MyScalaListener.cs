using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ScalaParserCORE { 
public class MyScalaAnalyzer
{
    public static MetricCalculator AnalyzeScalaCode(string scalaCode)
    {
        var inputStream = new AntlrInputStream(scalaCode);
        var lexer = new ScalaLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new ScalaParser(tokenStream);

        parser.RemoveErrorListeners(); // Убираем стандартный обработчик ошибок

        var tree = parser.compilationUnit(); // Используем корневой нетерминал

        // Анализируем дерево
        var metrics = new MetricCalculator();
        metrics.AnalyzeTree(tree);

        return metrics; // Возвращаем результаты анализа
    }
}
}
