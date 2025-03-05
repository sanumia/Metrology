using Irony.Parsing;
using metro1;
using System.Globalization;
using System.Text.RegularExpressions;

class Program()
{
   
    static void Main()
    {
        var scalaCode = "def mainLoop(): Unit = {\r\n    displayWelcome()\r\n    \r\n    var playAgain = true\r\n    while (playAgain) {\r\n      val config = chooseDifficulty()\r\n      playGame(config)\r\n      playAgain = askForReplay()\r\n    }\r\n    \r\n    coloredPrint(\"\\nСпасибо за игру! До встречи!\", ANSI_GREEN)\r\n  }"; // Пример Scala-кода

        var metricCalculator = new MetricCalculator();
        var (operators, operands) = metricCalculator.GetMetrics(scalaCode);

        Console.WriteLine($"Total Operators: {operators}");
        Console.WriteLine($"Total Operands: {operands}");
    }

}