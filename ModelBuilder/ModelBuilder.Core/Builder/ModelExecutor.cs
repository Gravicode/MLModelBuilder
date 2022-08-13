using ModelBuilder.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;
using System.Reflection;
/*


Directory: D:\experiment\MLModelBuilder\ModelBuilder\ModelBuilder.Web\bin\Debug\net6.0


Mode                 LastWriteTime         Length Name                                                                 
----                 -------------         ------ ----                                                                 
-a----          1/1/1980  12:00 AM          15872 parms.ReferencedAssemblies.Add("Blazored.LocalStorage.dll                                            ");
-a----          1/1/1980  12:00 AM          14848 parms.ReferencedAssemblies.Add("Blazored.SessionStorage.dll                                          ");
-a----         2/19/2020  10:14 PM          17920 parms.ReferencedAssemblies.Add("Blazored.Toast.dll                                                   ");
-a----          1/1/1980  12:00 AM          19968 parms.ReferencedAssemblies.Add("BlazorInputFile.dll                                                  ");
-a----         1/28/2022   3:49 PM         403288 parms.ReferencedAssemblies.Add("Google.Protobuf.dll                                                  ");
-a----          6/2/2020   1:07 PM         256952 parms.ReferencedAssemblies.Add("Humanizer.dll                                                        ");
-a----         9/19/2021   4:20 PM         204800 parms.ReferencedAssemblies.Add("ICSharpCode.SharpZipLib.dll                                          ");
-a----         2/24/2022   3:57 PM          46232 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Authorization.dll                               ");
-a----         1/23/2020   5:09 PM          18304 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Blazor.HttpClient.dll                           ");
-a----         2/24/2022   3:57 PM         217200 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Components.dll                                  ");
-a----         2/24/2022   3:57 PM          33432 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Components.Forms.dll                            ");
-a----         2/24/2022   3:58 PM         132248 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Components.Web.dll                              ");
-a----         2/24/2022   3:56 PM          15984 parms.ReferencedAssemblies.Add("Microsoft.AspNetCore.Metadata.dll                                    ");
-a----         12/7/2016   9:37 AM          22208 parms.ReferencedAssemblies.Add("Microsoft.Azure.KeyVault.Core.dll                                    ");
-a----          2/6/2020  11:17 PM         417144 parms.ReferencedAssemblies.Add("Microsoft.Azure.Storage.Blob.dll                                     ");
-a----          2/6/2020  11:17 PM         360824 parms.ReferencedAssemblies.Add("Microsoft.Azure.Storage.Common.dll                                   ");
-a----         2/25/2021   1:46 AM        5559184 parms.ReferencedAssemblies.Add("Microsoft.CodeAnalysis.CSharp.dll                                    ");
-a----         2/25/2021   1:43 AM        2450296 parms.ReferencedAssemblies.Add("Microsoft.CodeAnalysis.dll                                           ");
-a----         7/13/2022   4:50 AM         172448 parms.ReferencedAssemblies.Add("Microsoft.Data.Sqlite.dll                                            ");
-a----         7/13/2022   4:50 AM          31648 parms.ReferencedAssemblies.Add("Microsoft.EntityFrameworkCore.Abstractions.dll                       ");
-a----        11/18/2021   6:22 AM         391032 parms.ReferencedAssemblies.Add("Microsoft.EntityFrameworkCore.Design.dll                             ");
-a----         7/13/2022   4:50 AM        2100128 parms.ReferencedAssemblies.Add("Microsoft.EntityFrameworkCore.dll                                    ");
-a----         7/13/2022   4:50 AM        1500576 parms.ReferencedAssemblies.Add("Microsoft.EntityFrameworkCore.Relational.dll                         ");
-a----         7/13/2022   4:52 AM         206240 parms.ReferencedAssemblies.Add("Microsoft.EntityFrameworkCore.Sqlite.dll                             ");
-a----         1/15/2022   2:54 AM          38504 parms.ReferencedAssemblies.Add("Microsoft.Extensions.Caching.Memory.dll                              ");
-a----        10/23/2021   6:47 AM          75888 parms.ReferencedAssemblies.Add("Microsoft.Extensions.DependencyModel.dll                             ");
-a----         2/24/2022   7:12 AM          62064 parms.ReferencedAssemblies.Add("Microsoft.Extensions.Logging.Abstractions.dll                        ");
-a----         2/24/2022   3:57 PM          62064 parms.ReferencedAssemblies.Add("Microsoft.JSInterop.dll                                              ");
-a----         6/14/2022   7:49 AM         292480 parms.ReferencedAssemblies.Add("Microsoft.ML.AutoML.dll                                              ");
-a----         6/14/2022   7:45 AM         416400 parms.ReferencedAssemblies.Add("Microsoft.ML.Core.dll                                                ");
-a----         6/14/2022   7:45 AM          90256 parms.ReferencedAssemblies.Add("Microsoft.ML.CpuMath.dll                                             ");
-a----         6/14/2022   7:46 AM        1559160 parms.ReferencedAssemblies.Add("Microsoft.ML.Data.dll                                                ");
-a----         6/14/2022   7:45 AM          47744 parms.ReferencedAssemblies.Add("Microsoft.ML.DataView.dll                                            ");
-a----         6/14/2022   7:46 AM          14968 parms.ReferencedAssemblies.Add("Microsoft.ML.dll                                                     ");
-a----         6/14/2022   7:46 AM         476288 parms.ReferencedAssemblies.Add("Microsoft.ML.FastTree.dll                                            ");
-a----         6/14/2022   7:46 AM          75896 parms.ReferencedAssemblies.Add("Microsoft.ML.ImageAnalytics.dll                                      ");
-a----         6/14/2022   7:46 AM          55952 parms.ReferencedAssemblies.Add("Microsoft.ML.KMeansClustering.dll                                    ");
-a----         6/14/2022   7:46 AM          88208 parms.ReferencedAssemblies.Add("Microsoft.ML.LightGbm.dll                                            ");
-a----         6/14/2022   7:46 AM          71296 parms.ReferencedAssemblies.Add("Microsoft.ML.Mkl.Components.dll                                      ");
-a----         12/7/2021  10:48 AM         126376 parms.ReferencedAssemblies.Add("Microsoft.ML.OnnxRuntime.dll                                         ");
-a----         6/14/2022   7:46 AM         127096 parms.ReferencedAssemblies.Add("Microsoft.ML.OnnxTransformer.dll                                     ");
-a----         6/14/2022   7:46 AM          52856 parms.ReferencedAssemblies.Add("Microsoft.ML.PCA.dll                                                 ");
-a----         6/14/2022   7:46 AM          52888 parms.ReferencedAssemblies.Add("Microsoft.ML.Recommender.dll                                         ");
-a----         6/14/2022   7:45 AM          44152 parms.ReferencedAssemblies.Add("Microsoft.ML.SearchSpace.dll                                         ");
-a----         6/14/2022   7:46 AM         324224 parms.ReferencedAssemblies.Add("Microsoft.ML.StandardTrainers.dll                                    ");
-a----         6/14/2022   7:47 AM          59000 parms.ReferencedAssemblies.Add("Microsoft.ML.TensorFlow.dll                                          ");
-a----         6/14/2022   7:46 AM         230016 parms.ReferencedAssemblies.Add("Microsoft.ML.TimeSeries.dll                                          ");
-a----         6/14/2022   7:46 AM         763016 parms.ReferencedAssemblies.Add("Microsoft.ML.Transforms.dll                                          ");
-a----         6/14/2022   7:47 AM          85624 parms.ReferencedAssemblies.Add("Microsoft.ML.Vision.dll                                              ");
-a----        10/23/2021   6:40 AM          26224 parms.ReferencedAssemblies.Add("Microsoft.Win32.SystemEvents.dll                                     ");
-a----         8/13/2022   3:04 PM          57344 parms.ReferencedAssemblies.Add("ModelBuilder.Core.dll                                                ");
-a----         8/13/2022   3:04 PM           9728 parms.ReferencedAssemblies.Add("ModelBuilder.Models.dll                                              ");
-a----         8/13/2022   9:36 AM          36352 parms.ReferencedAssemblies.Add("ModelBuilder.Tools.dll                                               ");
-a----         8/13/2022   3:06 PM         212480 parms.ReferencedAssemblies.Add("ModelBuilder.Web.dll                                                 ");
-a----         3/29/2022   9:21 PM        8805888 parms.ReferencedAssemblies.Add("MudBlazor.dll                                                        ");
-a----         6/18/2017   1:57 PM         639488 parms.ReferencedAssemblies.Add("Newtonsoft.Json.dll                                                  ");
-a----          9/6/2020   1:54 PM        3621888 parms.ReferencedAssemblies.Add("NumSharp.Lite.dll                                                    ");
-a----         4/22/2021   3:06 AM         189952 parms.ReferencedAssemblies.Add("Postmark.dll                                                         ");
-a----         2/10/2020   2:57 AM          54272 parms.ReferencedAssemblies.Add("Protobuf.Text.dll                                                    ");
-a----        12/16/2020   8:53 PM          55296 parms.ReferencedAssemblies.Add("SendGrid.dll                                                         ");
-a----         3/10/2018  12:54 AM           6656 parms.ReferencedAssemblies.Add("shortid.dll                                                          ");
-a----          9/4/2021   1:04 AM           5120 parms.ReferencedAssemblies.Add("SQLitePCLRaw.batteries_v2.dll                                        ");
-a----          9/4/2021   1:04 AM          50688 parms.ReferencedAssemblies.Add("SQLitePCLRaw.core.dll                                                ");
-a----          9/4/2021   1:04 AM          38400 parms.ReferencedAssemblies.Add("SQLitePCLRaw.provider.e_sqlite3.dll                                  ");
-a----         6/23/2020   3:00 PM          25088 parms.ReferencedAssemblies.Add("StarkbankEcdsa.dll                                                   ");
-a----         5/15/2018   1:29 PM         187016 parms.ReferencedAssemblies.Add("System.CodeDom.dll                                                   ");
-a----        10/23/2021   6:49 AM         175216 parms.ReferencedAssemblies.Add("System.Drawing.Common.dll                                            ");
-a----        11/15/2019   8:37 AM         118344 parms.ReferencedAssemblies.Add("System.IO.Packaging.dll                                              ");
-a----         1/15/2022   2:49 AM          78448 parms.ReferencedAssemblies.Add("System.IO.Pipelines.dll                                              ");
-a----         6/26/2022   5:14 PM         164864 parms.ReferencedAssemblies.Add("System.Linq.Dynamic.Core.dll                                         ");
-a----         7/24/2015   2:15 PM          22232 parms.ReferencedAssemblies.Add("System.Net.Http.Extensions.dll                                       ");
-a----         7/24/2015   2:15 PM         185544 parms.ReferencedAssemblies.Add("System.Net.Http.Formatting.dll                                       ");
-a----         7/24/2015   2:15 PM          21720 parms.ReferencedAssemblies.Add("System.Net.Http.Primitives.dll                                       ");
-a----         9/30/2020   2:56 AM        1835008 parms.ReferencedAssemblies.Add("TensorFlow.NET.dll                                                   ");



*/
namespace ModelBuilder.Core.Builder
{
    public class ModelExecutor
    { 
        /// <summary>
        /// Using the class CSharpCode Provider
        /// </summary>
        public void ExecuteCode(List<ModelParameter> Params,string ModelPath)
        {
            var c = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            var className = $"CodeEvaler_{Guid.NewGuid().ToString("N")}";
            // doesn't work with or without netstandard reference
            var netstandard = Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51");
            cp.ReferencedAssemblies.Add(netstandard.Location);
            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            var sb = new StringBuilder("");

            sb.Append("namespace Jobs.Dynamic{ \n");
            sb.Append($"public class {className} {{\n");
            sb.Append($"public object RunSnippetCode()\n{{\n");
            sb.Append("\nreturn null;\n");
            sb.Append("\n}\n");
            sb.Append("}");
            sb.Append("}");

            CompilerResults cr = c.CompileAssemblyFromSource(cp, sb.ToString());


//            var Compiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

//            var parms = new CompilerParameters();
//            parms.ReferencedAssemblies.Add("System.dll");
//            parms.ReferencedAssemblies.Add("System.Core.dll");

//            parms.ReferencedAssemblies.Add("Microsoft.CodeAnalysis.CSharp.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.CodeAnalysis.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.AutoML.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Core.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.CpuMath.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Data.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.DataView.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.FastTree.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.ImageAnalytics.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.KMeansClustering.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.LightGbm.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Mkl.Components.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.OnnxRuntime.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.OnnxTransformer.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.PCA.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Recommender.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.SearchSpace.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.StandardTrainers.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.TensorFlow.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.TimeSeries.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Transforms.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.ML.Vision.dll");
//            parms.ReferencedAssemblies.Add("Microsoft.Win32.SystemEvents.dll");
//            parms.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
//            parms.ReferencedAssemblies.Add("NumSharp.Lite.dll");
//            parms.ReferencedAssemblies.Add("StarkbankEcdsa.dll");
//            parms.ReferencedAssemblies.Add("System.CodeDom.dll");
//            parms.ReferencedAssemblies.Add("System.IO.Packaging.dll");
//            parms.ReferencedAssemblies.Add("System.IO.Pipelines.dll");
//            parms.ReferencedAssemblies.Add("System.Linq.Dynamic.Core.dll");
//            parms.ReferencedAssemblies.Add("TensorFlow.NET.dll");

//            parms.GenerateInMemory = true;

//            var classCode = @"
//using Microsoft.ML;
//using Microsoft.ML.Data;
//using System.Dynamic;
//namespace MyApp
//{
//    public class RunModel
//    {
//         public class ModelInput
//        {";
//            foreach (var param in Params)
//            {
//                classCode += $@"
//            [ColumnName(""{param.ColName}"")]
//            public {param.ColType} {param.FieldName} {{ get; set; }}";
//            }
//            classCode+= @"
//    }

//    public class ModelOutput
//    {
//";
//            foreach (var param in Params)
//            {
//                classCode += $@"
//            [ColumnName(""{param.ColName}"")]
//            public {param.ColOutType} {param.FieldName} {{ get; set; }}";
//            }

//            classCode += @"
//        [ColumnName(""Features"")]
//        public float[] Features { get; set; }

//        [ColumnName(""Score"")]
//        public float Score { get; set; }
//    }

//    public ExpandoObject Run()
//    {

//        var input = new ModelInput();
        
//";
//            foreach (var param in Params)
//            {
//                classCode += $@"
//            input.{param.FieldName} = { (param.ColType == "string" ? $@"""{param.ColData}""" : param.ColData)};";

//            }
//            classCode += $@"var MPath = @""{ModelPath}"";";
//            classCode += @"
        
//        var res = Predict(MPath, input);
//        return res;

//    }

//    public ExpandoObject Predict(string ModelPath, ModelInput inputData)
//    {

//        try
//        {
//            MLContext mlContext = new MLContext();

//            ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
//            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);

           
//            ModelOutput predictionResult = predEngine.Predict(inputData);

           
//            var expando = new ExpandoObject();
//            var dictionary = (IDictionary<string, object>)expando;

//            foreach (var property in predictionResult.GetType().GetProperties())
//                dictionary.Add(property.Name, property.GetValue(predictionResult));
//            return (ExpandoObject)dictionary;
//        }
//        catch (Exception ex)
//        {
//            dynamic output = new ExpandoObject();
//            output.Message = ex.Message;
//            return output;

//        }

//    }
//}
//}
//";

//            CompilerResults result = Compiler.CompileAssemblyFromSource(parms, classCode);

//            if (result.Errors.Count > 0)
//            {
//                Console.WriteLine("*** Compilation Errors");
//                foreach (var error in result.Errors)
//                {
//                    Console.WriteLine("- " + error);

//                    return;
//                }
//            }





//            var ass = result.CompiledAssembly;

//            dynamic inst = ass.CreateInstance("MyApp.RunModel");
//            var res = inst.Run() as System.Dynamic.ExpandoObject;

//            Console.WriteLine(res);
        }
    }
}
