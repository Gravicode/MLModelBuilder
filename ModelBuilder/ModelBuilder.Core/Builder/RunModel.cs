using Microsoft.ML;
using Microsoft.ML.Data;
using System.Dynamic;
namespace MyApp { public class RunModel { 
        public class ModelInput { 
            [ColumnName("mpg")] public float mpg { get; set; } 
            [ColumnName("cylinders")] public float cylinders { get; set; } 
            [ColumnName("displacement")] public float displacement { get; set; } 
            [ColumnName("horsepower")] public float horsepower { get; set; } 
            [ColumnName("weight")] public float weight { get; set; } 
            [ColumnName("acceleration")] public float acceleration { get; set; } 
            [ColumnName("model year")] public float model_year { get; set; } [ColumnName("origin")] 
            public float origin { get; set; } 
            [ColumnName("car name")] 
            public string car_name { get; set; } }
        public class ModelOutput { [ColumnName("mpg")] public float mpg { get; set; }
            [ColumnName("cylinders")] public float cylinders { get; set; }
            [ColumnName("displacement")] public float displacement { get; set; }
            [ColumnName("horsepower")] public float horsepower { get; set; }
            [ColumnName("weight")] public float weight { get; set; }
            [ColumnName("acceleration")] public float acceleration { get; set; } 
            [ColumnName("model year")] public float model_year { get; set; }
            [ColumnName("origin")] public float origin { get; set; }
            [ColumnName("car name")] public float[] car_name { get; set; } 
            [ColumnName("Features")] public float[] Features { get; set; }
            [ColumnName("Score")] public float Score { get; set; } } 
        public ExpandoObject Run() {
            var input = new ModelInput(); input.mpg = 0f; input.cylinders = 8; input.displacement = 370; input.horsepower = 120; input.weight = 3456; input.acceleration = 12; input.model_year = 70; input.origin = 1; input.car_name = "chevrolet chevelle malibu";
            var MPath = @"C:\\Users\\mifma\\AppData\\Local/model-builder/Models/model_mifmasterz@yahoo.com_9b6890eb_f454_401a_97cf_c01943258738.zip"; 
            var res = Predict(MPath, input); return res; }
        public ExpandoObject Predict(string ModelPath, ModelInput inputData) { try { MLContext mlContext = new MLContext(); ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema); var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema); ModelOutput predictionResult = predEngine.Predict(inputData); var expando = new ExpandoObject(); var dictionary = (IDictionary<string, object>)expando; foreach (var property in predictionResult.GetType().GetProperties()) dictionary.Add(property.Name, property.GetValue(predictionResult)); return (ExpandoObject)dictionary; } catch (Exception ex) { dynamic output = new ExpandoObject(); output.Message = ex.Message; return output; } } } }
//using Microsoft.ML;
//using Microsoft.ML.Data;
//using System.Dynamic;

//namespace ModelBuilder.Core.Builder
//{
//    public class RunModel
//    {
//        public class ModelInput
//        {
//            [ColumnName("Input")]
//            public float Input { get; set; }
//        }

//        public class ModelOutput
//        {

//            [ColumnName("Features")]
//            public float[] Features { get; set; }

//            [ColumnName("Score")]
//            public float Score { get; set; }
//        }

//        public ExpandoObject Run()
//        {

//            var input = new ModelInput();
//            input.Input = 0f;
//            var MPath = "xxx";
//            var res = Predict(MPath, input);
//            return res;

//        }

//        public ExpandoObject Predict(string ModelPath, ModelInput inputData)
//        {

//            try
//            {
//                MLContext mlContext = new MLContext();

//                ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
//                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);

//                // Try a single prediction
//                ModelOutput predictionResult = predEngine.Predict(inputData);

//                //ConsoleHelper.WriteToLog($"Prediction [hasil] --> Predicted value: {predictionResult.Score}");

//                //output.Data = predictionResult;
//                var expando = new ExpandoObject();
//                var dictionary = (IDictionary<string, object>)expando;

//                foreach (var property in predictionResult.GetType().GetProperties())
//                    dictionary.Add(property.Name, property.GetValue(predictionResult));
//                return (ExpandoObject)dictionary;
//            }
//            catch (Exception ex)
//            {
//                dynamic output = new ExpandoObject();
//                output.Message = ex.Message;
//                return output;

//            }

//        }

//    }

//}
