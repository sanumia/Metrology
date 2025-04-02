using ScalaParserCORE;

namespace ScalaParser
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnHalsteadButtonClicked(object sender, System.EventArgs e)
        {
            string scalaCode = TextBox.Text;

            if (string.IsNullOrWhiteSpace(scalaCode))
            {
                DisplayAlert("Ошибка", "Пожалуйста, введите код на Scala.", "OK");
                return;
            }

            try
            {
                var halsteadMetrics = MyScalaAnalyzer.AnalyzeScalaCode(scalaCode);
                DisplayHalsteadMetrics(halsteadMetrics);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Произошла ошибка при парсинге: {ex.Message}", "OK");
            }
        }

        private void OnJilbaButtonClicked(object sender, System.EventArgs e)
        {
            string scalaCode = TextBox.Text;

            if (string.IsNullOrWhiteSpace(scalaCode))
            {
                DisplayAlert("Ошибка", "Пожалуйста, введите код на Scala.", "OK");
                return;
            }

            try
            {
                var jilbaMetrics = MyScalaAnalyzer.AnalyzeCode(scalaCode);
                DisplayJilbaMetrics(jilbaMetrics);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Произошла ошибка при парсинге: {ex.Message}", "OK");
            }
        }
        private void DisplayHalsteadMetrics(MetricCalculator metrics)
        {
            var metricsData = new List<object>();

            var operatorEnumerator = metrics.OperatorCounts.GetEnumerator();
            var operandEnumerator = metrics.OperandCounts.GetEnumerator();

            bool hasOperators = operatorEnumerator.MoveNext();
            bool hasOperands = operandEnumerator.MoveNext();

            int operatorNum = 0;
            int operandNum = 0;

            while (hasOperators || hasOperands)
            {
                string operatorText = hasOperators ? operatorEnumerator.Current.Key : "";
                string operatorFreq = hasOperators ? operatorEnumerator.Current.Value.ToString() : "";
                string operandText = hasOperands ? operandEnumerator.Current.Key : "";
                string operandFreq = hasOperands ? operandEnumerator.Current.Value.ToString() : "";

                if (hasOperators)
                {
                    hasOperators = operatorEnumerator.MoveNext();
                    operatorNum++;
                };
                if (hasOperands)
                {
                    hasOperands = operandEnumerator.MoveNext();
                    operandNum++;
                }
                metricsData.Add(new
                {
                    Operator = operatorText,
                    OperatorFrequency = operatorFreq,
                    Operand = operandText,
                    OperandFrequency = operandFreq,
                    OperatorNumber = operatorNum,
                    OperandNumber = operandNum
                });
                metricsData.Add(new
                {
                    Operator = "______________________________",
                    OperatorFrequency = "______________________________",
                    Operand = "______________________________",
                    OperandFrequency = "______________________________",
                    OperatorNumber = "________________________",
                    OperandNumber = "________________________"
                });
            }

            MetricsTable.ItemsSource = metricsData;

            double programLength = metrics.TotalOperators + metrics.TotalOperands;
            double vocabularySize = metrics.OperatorCounts.Count + metrics.OperandCounts.Count;
            double volume = programLength * Math.Log2(vocabularySize > 0 ? vocabularySize : 1);

            HolstedInfoLabel.Text = $"Словарь программы: η1 + η2 = {metrics.OperatorCounts.Count} + {metrics.OperandCounts.Count} = {vocabularySize}\n" +
                                    $"Длина программы: N1 + N2 = {metrics.TotalOperators} + {metrics.TotalOperands} = {programLength}\n" +
                                    $"Объем программы: V = {volume:F2}";
        }

        private void DisplayJilbaMetrics(GilbMetricCalculator jilbaMetrics)
        {
            int absoluteComplexity = (int)(jilbaMetrics.CL);
            double relativeComplexity = (jilbaMetrics.CL)/ (double)jilbaMetrics.OperatorCounts.Count;
            var maxNestingLevel = jilbaMetrics.CLI;

            JilbaInfoLabel.Text = $"Абсолютная сложность программы (CL): {absoluteComplexity} \n" +
                                  $"Относительная сложность программы (cl): {relativeComplexity} \n" +
                                  $"Максимальный уровень вложенности (CLI): {maxNestingLevel - 1} ";
        }

    }
    public class MetricsEntry
    {
        public string? Operator { get; set; }
        public string? OperatorFrequency { get; set; }
        public string? Operand { get; set; }
        public string? OperandFrequency { get; set; }
    }

}