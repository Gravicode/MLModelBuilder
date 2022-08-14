using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using ModelBuilder.Core.Helpers;
using ModelBuilder.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Builder
{
    public enum ModelTypes { Regression, BinaryClassification, MultiClassification, Ranking, Recommendation};
    public class ModelFactory
    {
        

        //Microsoft.ML.AutoML.ExperimentResult<T>
        /// <summary>
        /// Create model with Auto ML feature
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Tipe"></param>
        /// <param name="DataPath"></param>
        /// <param name="ModelPath"></param>
        /// <param name="LabelColumn"></param>
        /// <param name="TestFraction"></param>
        /// <param name="Separator"></param>
        /// <param name="TrainDuration"></param>
        /// <returns></returns>
        public static async Task<OutputCls> DoAutoML(ModelTypes Tipe, string DataPath, string ModelPath,string LabelColumn, char[] Separator, double TestFraction=0.2, uint TrainDuration = 60,CancellationTokenSource cts=null)
        {
            var hasil = await Task.Run<OutputCls>(() =>
            {
                var output = new OutputCls();
                var mlContext = new MLContext();
                if (!File.Exists(DataPath))
                {
                    output.IsSucceed = false;
                    output.Message = "Data training tidak ditemukan.";
                    return output;
                }
                var dt = DataConverter.ConvertCSVtoDataTable(DataPath);
                try
                {
                    // Load Data
                    var opt = new TextLoader.Options() { HasHeader = true, Separators = Separator, AllowSparse = false, AllowQuoting = true };
                    var cols = new List<TextLoader.Column>();
                    int idx = 0;
                    var dh = new DataHelper();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        var rowVal = dt.Rows[0][dc].ToString();
                        var tipe = dh.ParseString(rowVal);
                        var kind = DataKind.String;
                        switch (tipe)
                        {
                            case DataHelper.dataType.System_DateTime:
                                kind = DataKind.DateTime;
                                break;
                            case DataHelper.dataType.System_Int64:
                                kind = DataKind.Single;
                                break;
                            case DataHelper.dataType.System_String:
                                kind = DataKind.String;
                                break;
                            case DataHelper.dataType.System_Double:
                                kind = DataKind.Single;
                                break;
                            case DataHelper.dataType.System_Boolean:
                                kind = DataKind.Boolean;
                                break;
                            case DataHelper.dataType.System_Int32:
                                kind = DataKind.Single;
                                break;
                        }
                        cols.Add(new TextLoader.Column(dc.ColumnName, kind, idx++));
                    }
                    opt.Columns = cols.ToArray();
                    IDataView trainingDataView = mlContext.Data.LoadFromTextFile(path: DataPath, opt);
                    //0.75:0.25
                    var split = mlContext.Data.TrainTestSplit(trainingDataView, testFraction: TestFraction);

                    //var pre=  trainingDataView.Preview();
                    switch (Tipe)
                    {
                        case ModelTypes.Regression:
                            {
                                var handler = new RegressionExperimentProgressHandler();
                                var setting = new RegressionExperimentSettings() { };
                                setting.MaxExperimentTimeInSeconds = TrainDuration;
                                if (cts != null)
                                    setting.CancellationToken = cts.Token;
                                var experiment = mlContext.Auto().CreateRegressionExperiment(setting);

                                var result = experiment.Execute(split.TrainSet, labelColumnName: LabelColumn, progressHandler: handler);

                                ConsoleHelper.WriteToLog($"Best Trainer:{result.BestRun.TrainerName}");
                                ConsoleHelper.PrintRegressionMetrics("Model", result.BestRun.ValidationMetrics);
                                // Save model
                                SaveModel(mlContext, result.BestRun.Model, ModelPath, trainingDataView.Schema);
                                output.Data = result;
                                output.IsSucceed = true;
                            }
                            break;
                        case ModelTypes.BinaryClassification:
                            {
                                var handler = new BinaryExperimentProgressHandler();
                                var setting = new BinaryExperimentSettings() { };
                                setting.MaxExperimentTimeInSeconds = TrainDuration;
                                if (cts != null)
                                    setting.CancellationToken = cts.Token;
                                var experiment = mlContext.Auto().CreateBinaryClassificationExperiment(setting);
                                var result = experiment.Execute(split.TrainSet, labelColumnName: LabelColumn, progressHandler: handler);

                                ConsoleHelper.WriteToLog($"Best Trainer:{result.BestRun.TrainerName}");
                                ConsoleHelper.PrintBinaryClassificationMetrics("Model", result.BestRun.ValidationMetrics);
                                // Save model
                                SaveModel(mlContext, result.BestRun.Model, ModelPath, trainingDataView.Schema);
                                output.Data = result;
                                output.IsSucceed = true;
                            }
                            break;
                        case ModelTypes.MultiClassification:
                            {
                                var handler = new MulticlassExperimentProgressHandler();
                                var setting = new MulticlassExperimentSettings() { };
                                setting.MaxExperimentTimeInSeconds = TrainDuration;
                                if (cts != null)
                                    setting.CancellationToken = cts.Token;
                                var experiment = mlContext.Auto().CreateMulticlassClassificationExperiment(setting);
                                var result = experiment.Execute(split.TrainSet, labelColumnName: LabelColumn, progressHandler: handler);

                                ConsoleHelper.WriteToLog($"Best Trainer:{result.BestRun.TrainerName}");
                                ConsoleHelper.PrintMulticlassClassificationMetrics("Model", result.BestRun.ValidationMetrics);
                                // Save model
                                SaveModel(mlContext, result.BestRun.Model, ModelPath, trainingDataView.Schema);
                                output.Data = result;
                                output.IsSucceed = true;
                            }
                            break;
                        case ModelTypes.Ranking:
                            {
                                var handler = new RankingExperimentProgressHandler();
                                var setting = new RankingExperimentSettings() { };
                                setting.MaxExperimentTimeInSeconds = TrainDuration;
                                if (cts != null)
                                    setting.CancellationToken = cts.Token;
                                var experiment = mlContext.Auto().CreateRankingExperiment(setting);
                                var result = experiment.Execute(split.TrainSet, labelColumnName: LabelColumn, progressHandler: handler);

                                ConsoleHelper.WriteToLog($"Best Trainer:{result.BestRun.TrainerName}");
                                ConsoleHelper.PrintRankingMetrics("Model", result.BestRun.ValidationMetrics, 10);
                                // Save model
                                SaveModel(mlContext, result.BestRun.Model, ModelPath, trainingDataView.Schema);
                                output.Data = result;
                                output.IsSucceed = true;
                            }
                            break;
                        case ModelTypes.Recommendation:
                            {
                                var handler = new RegressionExperimentProgressHandler();
                                var setting = new RecommendationExperimentSettings() { };
                                setting.MaxExperimentTimeInSeconds = TrainDuration;
                                if (cts != null)
                                    setting.CancellationToken = cts.Token;
                                var experiment = mlContext.Auto().CreateRecommendationExperiment(setting);
                                var result = experiment.Execute(split.TrainSet, labelColumnName: LabelColumn, progressHandler: handler);

                                ConsoleHelper.WriteToLog($"Best Trainer:{result.BestRun.TrainerName}");
                                ConsoleHelper.PrintRegressionMetrics("Model", result.BestRun.ValidationMetrics);
                                // Save model
                                SaveModel(mlContext, result.BestRun.Model, ModelPath, trainingDataView.Schema);

                                output.Data = result;
                                output.IsSucceed = true;
                            }
                            break;
                    }

                    return output;
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteToLog("Failed to create model: " + ex.ToString());
                    output.Message = ex.ToString();
                    output.IsSucceed = false;
                    return output;
                }
            });
            return hasil;
        }
       
        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            ConsoleHelper.WriteToLog($"=============== Saving the model  ===============");
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            ConsoleHelper.WriteToLog("The model is saved to {0}", GetAbsolutePath(modelRelativePath));
        }

        public static string GetAbsolutePath(string relativePath)
        {
            Type t = MethodBase.GetCurrentMethod().DeclaringType;
            FileInfo _dataRoot = new FileInfo(t.Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

    }
}
