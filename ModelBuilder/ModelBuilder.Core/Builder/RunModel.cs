using Microsoft.ML;
using Microsoft.ML.Data;
using System.Dynamic;
namespace MyApp
{
    public class RunModel
    {
        public class ModelInput
        {
            [ColumnName("mpg")] public float mpg { get; set; }
            [ColumnName("cylinders")] public float cylinders { get; set; }
            [ColumnName("displacement")] public float displacement { get; set; }
            [ColumnName("horsepower")] public float horsepower { get; set; }
            [ColumnName("weight")] public float weight { get; set; }
            [ColumnName("acceleration")] public float acceleration { get; set; }
            [ColumnName("model year")] public float model_year { get; set; }
            [ColumnName("origin")]
            public float origin { get; set; }
            [ColumnName("car name")]
            public string car_name { get; set; }
        }
        public class ModelOutput
        {
            [ColumnName("mpg")] public float mpg { get; set; }
            [ColumnName("cylinders")] public float cylinders { get; set; }
            [ColumnName("displacement")] public float displacement { get; set; }
            [ColumnName("horsepower")] public float horsepower { get; set; }
            [ColumnName("weight")] public float weight { get; set; }
            [ColumnName("acceleration")] public float acceleration { get; set; }
            [ColumnName("model year")] public float model_year { get; set; }
            [ColumnName("origin")] public float origin { get; set; }
            [ColumnName("car name")] public float[] car_name { get; set; }
            [ColumnName("Features")] public float[] Features { get; set; }
            [ColumnName("Score")] public float Score { get; set; }
        }
        public List<ExpandoObject> Run()
        {
            var inputs = new List<ModelInput>();
            var input1 = new ModelInput();
            input1.mpg = 0f; input1.cylinders = 8; input1.displacement = 370; input1.horsepower = 120; input1.weight = 3456; input1.acceleration = 12; input1.model_year = 70; input1.origin = 1; input1.car_name = "chevrolet chevelle malibu";
            inputs.Add(input1);
            var MPath = @"C:\\Users\\mifma\\AppData\\Local/model-builder/Models/model_mifmasterz@yahoo.com_9b6890eb_f454_401a_97cf_c01943258738.zip";
            var res = Predict(MPath, inputs); 
            return res;
        }
        public List<ExpandoObject> Predict(string ModelPath, List<ModelInput> inputDatas)
        {
            try
            {
                var results = new List<ExpandoObject>();
                MLContext mlContext = new MLContext();
                ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);
                foreach (var inputData in inputDatas)
                {
                    ModelOutput predictionResult = predEngine.Predict(inputData);
                    var expando = new ExpandoObject(); var dictionary = (IDictionary<string, object>)expando;
                    foreach (var property in predictionResult.GetType().GetProperties()) dictionary.Add(property.Name, property.GetValue(predictionResult));
                    var obj = (ExpandoObject)dictionary;
                    results.Add(obj);

                }
                return results;
            }
            catch (Exception ex)
            {
                dynamic output = new ExpandoObject();
                output.Message = ex.Message;
                var shell = new List<ExpandoObject>() { output };
                return shell;
            }
        }
    }
}
