using Microsoft.Maui.Controls;
using ScalaParserCORE;
using System.Collections.Generic;

namespace ScalaParser
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnParseButtonClicked(object sender, System.EventArgs e)
        {
            string scalaCode = TextBox.Text;

            if (string.IsNullOrWhiteSpace(scalaCode))
            {
                DisplayAlert("Ошибка", "Пожалуйста, введите код на Scala.", "OK");
                return;
            }

            try
            {
                // Используем ScalaAnalyzer для анализа кода
                var metrics = MyScalaAnalyzer.AnalyzeScalaCode(scalaCode);

                // Отображаем результаты
                DisplayMetrics(metrics);
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", $"Произошла ошибка при парсинге: {ex.Message}", "OK");
            }
        }

        private void DisplayMetrics(MetricCalculator metrics)
        {
            var metricsData = new List<object>();

            var operatorEnumerator = metrics.OperatorCounts.GetEnumerator();
            var operandEnumerator = metrics.OperandCounts.GetEnumerator();

            bool hasOperators = operatorEnumerator.MoveNext();
            bool hasOperands = operandEnumerator.MoveNext();

            while (hasOperators || hasOperands)
            {
                string operatorText = hasOperators ? operatorEnumerator.Current.Key : "";
                string operatorFreq = hasOperators ? operatorEnumerator.Current.Value.ToString() : "";
                string operandText = hasOperands ? operandEnumerator.Current.Key : "";
                string operandFreq = hasOperands ? operandEnumerator.Current.Value.ToString() : "";

                metricsData.Add(new
                {
                    Operator = operatorText,
                    OperatorFrequency = operatorFreq,
                    Operand = operandText,
                    OperandFrequency = operandFreq
                });

                if (hasOperators) hasOperators = operatorEnumerator.MoveNext();
                if (hasOperands) hasOperands = operandEnumerator.MoveNext();
            }

            MetricsTable.ItemsSource = metricsData;

            double programLength = metrics.TotalOperators + metrics.TotalOperands;
            double vocabularySize = metrics.OperatorCounts.Count + metrics.OperandCounts.Count;
            double volume = programLength * Math.Log2(vocabularySize > 0 ? vocabularySize : 1);

            HolstedInfoLabel.Text = $"Словарь программы: {vocabularySize}\n" +
                                    $"Длина программы: {programLength}\n" +
                                    $"Объем программы: {volume:F2}";
        }

    }
    public class MetricsEntry
    {
        public string Operator { get; set; }
        public string OperatorFrequency { get; set; }
        public string Operand { get; set; }
        public string OperandFrequency { get; set; }
    }
}