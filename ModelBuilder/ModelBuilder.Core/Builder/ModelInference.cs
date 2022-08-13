using Microsoft.ML;
using Microsoft.ML.Data;
using ModelBuilder.Core.Helpers;
using ModelBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Builder
{
    public class ModelInference
    {
        public class ModelInput:DynamicClass
        {
            public ModelInput(List<Field> cols):base(cols)
            {
                
            }
            public ModelInput()
            {

            }
        }

        public class ModelOutput:DynamicClass
        {
            public ModelOutput()
            {
                
            }
            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"Score")]
            public float Score { get; set; }
        }

        public OutputCls Predict(string ModelPath, ModelInput inputData)
        {
            var output = new OutputCls();
            try
            {
                MLContext mlContext = new MLContext();
                // Create sample data to do a single prediction with it 


                ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);


                // Try a single prediction
                ModelOutput predictionResult = predEngine.Predict(inputData);

                ConsoleHelper.WriteToLog($"Prediction [hasil] --> Predicted value: {predictionResult.Score}");

                output.Data = predictionResult;
                output.IsSucceed = true;
            }
            catch (Exception ex)
            {
                output.IsSucceed = false;
                output.Message = ex.ToString();
                
            }
            return output;
        }
    }
}
