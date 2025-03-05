using Irony.Parsing;
using metro1;
using System.Globalization;
using System.Text.RegularExpressions;

class Proram()
{
   
    static void Main()
    {
        var scalaCode = "def add(x) { x + 1 }"; // Пример Scala-кода

        var metricCalculator = new MetricCalculator();
        var (operators, operands) = metricCalculator.GetMetrics(scalaCode);

        Console.WriteLine($"Total Operators: {operators}");
        Console.WriteLine($"Total Operands: {operands}");
    }

}