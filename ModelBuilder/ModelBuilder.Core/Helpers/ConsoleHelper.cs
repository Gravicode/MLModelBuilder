﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML.Data;
using Microsoft.ML;
using static Microsoft.ML.TrainCatalogBase;
using System.Diagnostics;
using Microsoft.ML.AutoML;
using System.Text;

namespace ModelBuilder.Core.Helpers
{
    public static class ConsoleHelper
    {
        private const int Width = 114;
        static ILogger _consoleLogger = new ConsoleLogger();
        static EventLogger _eventLogger = new EventLogger();


        public static EventLogger GetEventLogger()
        {
            return _eventLogger;
        }
        public static void WriteToLog()
        {
            if (_eventLogger.IsReady())
                _eventLogger.WriteLine();
            else
                _consoleLogger.WriteLine();
        }
        public static void WriteToLog(string Message)
        {
            if (_eventLogger.IsReady())
                _eventLogger.WriteLine(Message);
            else
                _consoleLogger.WriteLine(Message);

        }
        public static void WriteToLog(string Format, params string[] Args)
        {
            if (_eventLogger.IsReady())
                _eventLogger.WriteLine(Format,Args);
            else
                _consoleLogger.WriteLine(Format, Args);

        }
        public static void ReadKey()
        {
            if (_eventLogger.IsReady())
                _eventLogger.ReadKey();
            else
                _consoleLogger.ReadKey();
        }
        public static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
        {
            WriteToLog($"*************************************************");
            WriteToLog($"*       Metrics for {name} regression model      ");
            WriteToLog($"*------------------------------------------------");
            WriteToLog($"*       LossFn:        {metrics.LossFunction:0.##}");
            WriteToLog($"*       R2 Score:      {metrics.RSquared:0.##}");
            WriteToLog($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            WriteToLog($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
            WriteToLog($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
            WriteToLog($"*************************************************");
        }

        public static void PrintBinaryClassificationMetrics(string name, BinaryClassificationMetrics metrics)
        {
            WriteToLog($"************************************************************");
            WriteToLog($"*       Metrics for {name} binary classification model      ");
            WriteToLog($"*-----------------------------------------------------------");
            WriteToLog($"*       Accuracy: {metrics.Accuracy:P2}");
            WriteToLog($"*       Area Under Curve:      {metrics.AreaUnderRocCurve:P2}");
            WriteToLog($"*       Area under Precision recall Curve:  {metrics.AreaUnderPrecisionRecallCurve:P2}");
            WriteToLog($"*       F1Score:  {metrics.F1Score:P2}");
            WriteToLog($"*       PositivePrecision:  {metrics.PositivePrecision:#.##}");
            WriteToLog($"*       PositiveRecall:  {metrics.PositiveRecall:#.##}");
            WriteToLog($"*       NegativePrecision:  {metrics.NegativePrecision:#.##}");
            WriteToLog($"*       NegativeRecall:  {metrics.NegativeRecall:P2}");
            WriteToLog($"************************************************************");
        }

        public static void PrintMulticlassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
        {
            WriteToLog($"************************************************************");
            WriteToLog($"*    Metrics for {name} multi-class classification model    ");
            WriteToLog($"*-----------------------------------------------------------");
            WriteToLog($"    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value from 0 and 1, where closer to 1.0 is better");
            WriteToLog($"    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value from 0 and 1, where closer to 1.0 is better");
            WriteToLog($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
            WriteToLog($"    LogLoss for class 1 = {metrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
            WriteToLog($"    LogLoss for class 2 = {metrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
            WriteToLog($"    LogLoss for class 3 = {metrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
            WriteToLog($"************************************************************");
        }

        public static void PrintRankingMetrics(string name, RankingMetrics metrics, uint optimizationMetricTruncationLevel)
        {
            WriteToLog($"************************************************************");
            WriteToLog($"*             Metrics for {name} ranking model              ");
            WriteToLog($"*-----------------------------------------------------------");
            WriteToLog($"    Normalized Discounted Cumulative Gain (NDCG@{optimizationMetricTruncationLevel}) = {metrics?.NormalizedDiscountedCumulativeGains?[(int)optimizationMetricTruncationLevel - 1] ?? double.NaN:0.####}, a value from 0 and 1, where closer to 1.0 is better");
            WriteToLog($"    Discounted Cumulative Gain (DCG@{optimizationMetricTruncationLevel}) = {metrics?.DiscountedCumulativeGains?[(int)optimizationMetricTruncationLevel - 1] ?? double.NaN:0.####}");
        }

        public static void ShowDataViewInConsole(MLContext mlContext, IDataView dataView, int numberOfRows = 4)
        {
            string msg = string.Format("Show data in DataView: Showing {0} rows with the columns", numberOfRows.ToString());
            ConsoleWriteHeader(msg);

            var preViewTransformedData = dataView.Preview(maxRows: numberOfRows);

            foreach (var row in preViewTransformedData.RowView)
            {
                var ColumnCollection = row.Values;
                string lineToPrint = "Row--> ";
                foreach (KeyValuePair<string, object> column in ColumnCollection)
                {
                    lineToPrint += $"| {column.Key}:{column.Value}";
                }
                WriteToLog(lineToPrint + "\n");
            }
        }

        internal static void PrintIterationMetrics(int iteration, string trainerName, BinaryClassificationMetrics metrics, double? runtimeInSeconds)
        {
            CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.Accuracy ?? double.NaN,9:F4} {metrics?.AreaUnderRocCurve ?? double.NaN,8:F4} {metrics?.AreaUnderPrecisionRecallCurve ?? double.NaN,8:F4} {metrics?.F1Score ?? double.NaN,9:F4} {runtimeInSeconds.Value,9:F1}", Width);
        }

        internal static void PrintIterationMetrics(int iteration, string trainerName, MulticlassClassificationMetrics metrics, double? runtimeInSeconds)
        {
            CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.MicroAccuracy ?? double.NaN,14:F4} {metrics?.MacroAccuracy ?? double.NaN,14:F4} {runtimeInSeconds.Value,9:F1}", Width);
        }

        internal static void PrintIterationMetrics(int iteration, string trainerName, RegressionMetrics metrics, double? runtimeInSeconds)
        {
            CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.RSquared ?? double.NaN,8:F4} {metrics?.MeanAbsoluteError ?? double.NaN,13:F2} {metrics?.MeanSquaredError ?? double.NaN,12:F2} {metrics?.RootMeanSquaredError ?? double.NaN,8:F2} {runtimeInSeconds.Value,9:F1}", Width);
        }

        internal static void PrintIterationMetrics(int iteration, string trainerName, RankingMetrics metrics, double? runtimeInSeconds)
        {
            CreateRow($"{iteration,-4} {trainerName,-15} {metrics?.NormalizedDiscountedCumulativeGains[0] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[2] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[9] ?? double.NaN,9:F4} {metrics?.DiscountedCumulativeGains[9] ?? double.NaN,9:F4} {runtimeInSeconds.Value,9:F1}", Width);
        }

        internal static void PrintIterationException(Exception ex)
        {
            WriteToLog($"Exception during AutoML iteration: {ex}");
        }

        internal static void PrintBinaryClassificationMetricsHeader()
        {
            CreateRow($"{"",-4} {"Trainer",-35} {"Accuracy",9} {"AUC",8} {"AUPRC",8} {"F1-score",9} {"Duration",9}", Width);
        }

        internal static void PrintMulticlassClassificationMetricsHeader()
        {
            CreateRow($"{"",-4} {"Trainer",-35} {"MicroAccuracy",14} {"MacroAccuracy",14} {"Duration",9}", Width);
        }

        internal static void PrintRegressionMetricsHeader()
        {
            CreateRow($"{"",-4} {"Trainer",-35} {"RSquared",8} {"Absolute-loss",13} {"Squared-loss",12} {"RMS-loss",8} {"Duration",9}", Width);
        }

        internal static void PrintRankingMetricsHeader()
        {
            CreateRow($"{"",-4} {"Trainer",-15} {"NDCG@1",9} {"NDCG@3",9} {"NDCG@10",9} {"DCG@10",9} {"Duration",9}", Width);
        }

        private static void CreateRow(string message, int width)
        {
            WriteToLog("|" + message.PadRight(width - 2) + "|");
        }

        public static void ConsoleWriteHeader(params string[] lines)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteToLog(" ");
            foreach (var line in lines)
            {
                WriteToLog(line);
            }
            var maxLength = lines.Select(x => x.Length).Max();
            WriteToLog(new string('#', maxLength));
            Console.ForegroundColor = defaultColor;
        }

        public static void Print(ColumnInferenceResults results)
        {
            WriteToLog("Inferred dataset columns --");
            new ColumnInferencePrinter(results).Print();
            WriteToLog();
        }

        public static string BuildStringTable(IList<string[]> arrValues)
        {
            int[] maxColumnsWidth = GetMaxColumnsWidth(arrValues);
            var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

            var sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < arrValues.Count; rowIndex++)
            {
                if (rowIndex == 0)
                {
                    sb.AppendFormat("  {0} ", headerSpliter);
                    sb.AppendLine();
                }

                for (int colIndex = 0; colIndex < arrValues[0].Length; colIndex++)
                {
                    // Print cell
                    string cell = arrValues[rowIndex][colIndex];
                    cell = cell.PadRight(maxColumnsWidth[colIndex]);
                    sb.Append(" | ");
                    sb.Append(cell);
                }

                // Print end of line
                sb.Append(" | ");
                sb.AppendLine();

                // Print splitter
                if (rowIndex == 0)
                {
                    sb.AppendFormat(" |{0}| ", headerSpliter);
                    sb.AppendLine();
                }

                if (rowIndex == arrValues.Count - 1)
                {
                    sb.AppendFormat("  {0} ", headerSpliter);
                }
            }

            return sb.ToString();
        }

        private static int[] GetMaxColumnsWidth(IList<string[]> arrValues)
        {
            var maxColumnsWidth = new int[arrValues[0].Length];
            for (int colIndex = 0; colIndex < arrValues[0].Length; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < arrValues.Count; rowIndex++)
                {
                    int newLength = arrValues[rowIndex][colIndex].Length;
                    int oldLength = maxColumnsWidth[colIndex];

                    if (newLength > oldLength)
                    {
                        maxColumnsWidth[colIndex] = newLength;
                    }
                }
            }

            return maxColumnsWidth;
        }

        class ColumnInferencePrinter
        {
            private static readonly string[] TableHeaders = new[] { "Name", "Data Type", "Purpose" };

            private readonly ColumnInferenceResults _results;

            public ColumnInferencePrinter(ColumnInferenceResults results)
            {
                _results = results;
            }

            public void Print()
            {
                var tableRows = new List<string[]>();

                // Add headers
                tableRows.Add(TableHeaders);

                // Add column data
                var info = _results.ColumnInformation;
                AppendTableRow(tableRows, info.LabelColumnName, "Label");
                AppendTableRow(tableRows, info.ExampleWeightColumnName, "Weight");
                AppendTableRow(tableRows, info.SamplingKeyColumnName, "Sampling Key");
                AppendTableRows(tableRows, info.CategoricalColumnNames, "Categorical");
                AppendTableRows(tableRows, info.NumericColumnNames, "Numeric");
                AppendTableRows(tableRows, info.TextColumnNames, "Text");
                AppendTableRows(tableRows, info.IgnoredColumnNames, "Ignored");

                WriteToLog(ConsoleHelper.BuildStringTable(tableRows));
            }

            private void AppendTableRow(ICollection<string[]> tableRows,
                string columnName, string columnPurpose)
            {
                if (columnName == null)
                {
                    return;
                }

                tableRows.Add(new[]
                {
                    columnName,
                    GetColumnDataType(columnName),
                    columnPurpose
                });
            }

            private void AppendTableRows(ICollection<string[]> tableRows,
                IEnumerable<string> columnNames, string columnPurpose)
            {
                foreach (var columnName in columnNames)
                {
                    AppendTableRow(tableRows, columnName, columnPurpose);
                }
            }

            private string GetColumnDataType(string columnName)
            {
                return _results.TextLoaderOptions.Columns.First(c => c.Name == columnName).DataKind.ToString();
            }
        }
    }

}
