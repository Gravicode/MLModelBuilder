using Microsoft.ML;
using Microsoft.ML.Data;
using System.Dynamic;
using System.Collections.Generic;
using System;
namespace ModelBuilder.Web.Data
{
    using Microsoft.ML;
    using Microsoft.ML.Data;
    using System.Dynamic;
    using System.Collections.Generic;
    using System;

    public class RunModel
    {
        public class ModelInput
        {
            [ColumnName("category")]
            public bool category { get; set; }

            [ColumnName("content")]
            public string content { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("category")]
            public bool category { get; set; }

            [ColumnName("content")]
            public string content { get; set; }

            [ColumnName("Score")]
            public float Score { get; set; }
        }

        public List<ExpandoObject> Run()
        {
            var inputs = new List<ModelInput>();
            var input0 = new ModelInput();
            input0.category = true;
            input0.content = "Ok lar... Joking wif u oni...";
            inputs.Add(input0);
            var input1 = new ModelInput();
            input1.category = true;
            input1.content = "Ok lar... Joking wif u oni...";
            inputs.Add(input1);
            var MPath = @"C:\\Users\\mifma\\AppData\\Local/model-builder/Models/model_mifmasterz@yahoo.com_ca61571d_ea13_4c46_832d_7fda2ad3283c.zip";
            var res = Predict(MPath, inputs);
            return res;
        }

        public List<ExpandoObject> Predict(string ModelPath, List<ModelInput> inputDatas)
        {
            try
            {
                var results = new List<ExpandoObject>();
                MLContext mlContext = new MLContext();
                ModelOutput predictionResult;
                ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);
                foreach (var inputData in inputDatas)
                {
                    predictionResult = predEngine.Predict(inputData);
                    var expando = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expando;
                    foreach (var property in predictionResult.GetType().GetProperties())
                        dictionary.Add(property.Name, property.GetValue(predictionResult));
                    var obj = (ExpandoObject)dictionary;
                    results.Add(obj);
                }

                return results;
            }
            catch (Exception ex)
            {
                dynamic output = new ExpandoObject();
                output.Message = ex.Message;
                var shell = new List<ExpandoObject>()
            {output};
                return shell;
            }
        }
    }
    /*
    public class RunModel
    {
        public class ModelInput
        {
            [ColumnName("SepalLengthCm")]
            public float SepalLengthCm { get; set; }

            [ColumnName("SepalWidthCm")]
            public float SepalWidthCm { get; set; }

            [ColumnName("PetalLengthCm")]
            public float PetalLengthCm { get; set; }

            [ColumnName("PetalWidthCm")]
            public float PetalWidthCm { get; set; }

            [ColumnName("Species")]
            public string Species { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("SepalLengthCm")]
            public float SepalLengthCm { get; set; }

            [ColumnName("SepalWidthCm")]
            public float SepalWidthCm { get; set; }

            [ColumnName("PetalLengthCm")]
            public float PetalLengthCm { get; set; }

            [ColumnName("PetalWidthCm")]
            public float PetalWidthCm { get; set; }

            [ColumnName("Species")]
            public UInt32 Species { get; set; }

            //[ColumnName("Features")]
            //public float[] Features { get; set; }

            [ColumnName("Score")]
            public Microsoft.ML.Data.VBuffer<float> Score { get; set; }
        }

        public List<ExpandoObject> Run()
        {
            var inputs = new List<ModelInput>();
            var input0 = new ModelInput();
            input0.SepalLengthCm = (float)4.9;
            input0.SepalWidthCm = (float)3;
            input0.PetalLengthCm = (float)1.4;
            input0.PetalWidthCm = (float)0.2;
            input0.Species = "Iris-setosa";
            inputs.Add(input0);
            var input1 = new ModelInput();
            input1.SepalLengthCm = (float)4.9;
            input1.SepalWidthCm = (float)3;
            input1.PetalLengthCm = (float)1.4;
            input1.PetalWidthCm = (float)0.2;
            input1.Species = "Iris-setosa";
            inputs.Add(input1);
            var MPath = @"C:\\Users\\mifma\\AppData\\Local/model-builder/Models/model_mifmasterz@yahoo.com_72319859_8ff6_4f71_8ba3_61179ef7ef8e.zip";
            var res = Predict(MPath, inputs);
            return res;
        }

        public List<ExpandoObject> Predict(string ModelPath, List<ModelInput> inputDatas)
        {
            try
            {
                var results = new List<ExpandoObject>();
                MLContext mlContext = new MLContext();
                ModelOutput predictionResult;
                ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);
                foreach (var inputData in inputDatas)
                {
                    predictionResult = predEngine.Predict(inputData);
                    var expando = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expando;
                    foreach (var property in predictionResult.GetType().GetProperties())
                        dictionary.Add(property.Name, property.GetValue(predictionResult));
                    var obj = (ExpandoObject)dictionary;
                    results.Add(obj);
                }

                return results;
            }
            catch (Exception ex)
            {
                dynamic output = new ExpandoObject();
                output.Message = ex.Message;
                var shell = new List<ExpandoObject>()
            {output};
                return shell;
            }
        }
    }
    */
}
