using ModelBuilder.Models;
using CSScriptLib;
using System.Dynamic;
using ModelBuilder.Tools;
using System.IO;

namespace ModelBuilder.Core.Builder
{
    public class ModelExecutor
    {
        public static void ClearAsmJunk()
        {
            var asmPath = GetAsmPath();
            var files = Directory.GetFiles(asmPath, "*.dll");
            foreach(var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"cannot delete:{ex.ToString()}");
                }
               
            }
        }
        static string GetAsmPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var AsmPath = System.IO.Path.Join(path, $"/model-builder/Asm");
            if (!Directory.Exists(AsmPath))
            {
                Directory.CreateDirectory(AsmPath);
            }
            return AsmPath;
        }
        /// <summary>
        /// Using the CS-Script
        /// </summary>
        public async Task<ExpandoObject> ExecuteCode(List<ModelParameter> Params, string ModelPath,string MLType)
        {
            var AsmPath = string.Empty;
           
            var hasil = await Task.Run<ExpandoObject>(() =>
            {
                try
                {
                    AsmPath = GetAsmPath() + $"/script_{NumberGen.GenerateNumber(5)}.dll";
                    var info = new CompileInfo { RootClass = "ml_script", AssemblyFile = AsmPath};
                    var classCode = @"
            using Microsoft.ML;
            using Microsoft.ML.Data;
            using System.Dynamic;
            using System.Collections.Generic;
            using System;

                public class RunModel
                {
                     public class ModelInput
                    {";
                    foreach (var param in Params)
                    {
                        classCode += $@"
                        [ColumnName(""{param.ColName}"")]
                        public {param.ColType} {param.FieldName} {{ get; set; }}";
                    }
                    classCode += @"
                }

                public class ModelOutput
                {
            ";
                    if (MLType != "Recommendation" && MLType != "Ranking")
                    {
                        foreach (var param in Params)
                        {
                            classCode += $@"
                        [ColumnName(""{param.ColName}"")]
                        public {param.ColOutType} {param.FieldName} {{ get; set; }}";
                        }
                       
                        /*
                        //disable because not supported anymore
                        classCode += $@"
                        [ColumnName(""Features"")]
                        public float[] Features {{ get; set; }}";
                        */
                        
                    }
                    var ScoreType = MLType switch
                    {
                        "MultiClassification" => "Microsoft.ML.Data.VBuffer<float>",
                        //"BinaryClassification" => "Microsoft.ML.Data.VBuffer<float>",
                        _ => "float"
                    };
                    classCode += $@"
                    [ColumnName(""Score"")]
                    public {ScoreType} Score {{ get; set; }}
                }}

                public ExpandoObject Run()
                {{

                    var input = new ModelInput();

            ";
                    foreach (var param in Params)
                    {
                        classCode += $@"
                        input.{param.FieldName} = ";
                        classCode+= param.ColType switch
                        {
                            "string" => $@"""{param.ColData}"";",
                            "float" => "(float)" + float.Parse(param.ColData).ToString() + ";",
                            _ => param.ColData + ";"
                        };
                        
                        
                    }
                    classCode += $@"var MPath = @""{ModelPath}"";";
                    classCode += @"

                    var res = Predict(MPath, input);
                    return res;

                }

                public ExpandoObject Predict(string ModelPath, ModelInput inputData)
                {

                    try
                    {
                        MLContext mlContext = new MLContext();

                        ITransformer mlModel = mlContext.Model.Load(ModelPath, out DataViewSchema inputSchema);
                        var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel, inputSchema);


                        ModelOutput predictionResult = predEngine.Predict(inputData);


                        var expando = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expando;

                        foreach (var property in predictionResult.GetType().GetProperties())
                            dictionary.Add(property.Name, property.GetValue(predictionResult));
                        return (ExpandoObject)dictionary;
                    }
                    catch (Exception ex)
                    {
                        dynamic output = new ExpandoObject();
                        output.Message = ex.Message;
                        return output;

                    }

                }
            }
            
            ";
                    var ml_asm = CSScript.Evaluator
                                              .CompileCode(
                                                  classCode, info);
                    //GetType("ml_script+RunModel").
                    dynamic instance = ml_asm.CreateInstance("ml_script+RunModel");
                    var res = instance.Run();//GetMethod("Run").Invoke(null, null);
                    if (res is ExpandoObject obj)
                    {
                        return Task.FromResult(obj);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    dynamic errorObj = new ExpandoObject();
                    errorObj.Message = ex.ToString();
                    return Task.FromResult(errorObj);
                }
                finally
                {
                    if (File.Exists(AsmPath))
                    {
                        //File.Delete(AsmPath);
                    }
                }
                dynamic defaultObj = new ExpandoObject();
                defaultObj.Message = "Nothing to see..";
                return Task.FromResult(defaultObj);
            });
            return hasil;

        }

        /// <summary>
        /// Using the CS-Script
        /// </summary>
        public async Task<List<ExpandoObject>> ExecuteCodeBatch(List<ModelParameter[]> ListParams, string ModelPath, string MLType)
        {
            string AsmPath=string.Empty;
            var hasil = await Task.Run<List<ExpandoObject>>(() =>
            {
                try
                {
                    
                    AsmPath = GetAsmPath() + $"/batch_script_{NumberGen.GenerateNumber(5)}.dll";
                    var info = new CompileInfo { RootClass = "ml_script", AssemblyFile =AsmPath  };
                    var classCode = @"
            using Microsoft.ML;
            using Microsoft.ML.Data;
            using System.Dynamic;
            using System.Collections.Generic;
            using System;

                public class RunModel
                {
                     public class ModelInput
                    {";
                    foreach (var param in ListParams[0])
                    {
                        classCode += $@"
                        [ColumnName(""{param.ColName}"")]
                        public {param.ColType} {param.FieldName} {{ get; set; }}";
                    }
                    classCode += @"
                }

                public class ModelOutput
                {
            ";
                    if (MLType != "Recommendation" && MLType != "Ranking")
                    {
                        foreach (var param in ListParams[0])
                        {
                            classCode += $@"[ColumnName(""{param.ColName}"")] public {param.ColOutType} {param.FieldName} {{ get; set; }}";
                            classCode += Environment.NewLine;
                        }
                        /*
                        //disable because not supported anymore
                        classCode += $@"[ColumnName(""Features"")] public float[] Features {{ get; set; }}";
                        */
                    }
                    var ScoreType = MLType switch
                    {
                        "MultiClassification" => "Microsoft.ML.Data.VBuffer<float>",
                        //"BinaryClassification" => "Microsoft.ML.Data.VBuffer<float>",
                        _ => "float"
                    };
                    classCode += $@"
                    [ColumnName(""Score"")]
                    public {ScoreType} Score {{ get; set; }}
                }}

                public List<ExpandoObject> Run()
                {{
                    var inputs = new List<ModelInput>();
                    

            ";
                    int count = 0;
                    foreach (var Params in ListParams)
                    {
                        
                        classCode += $"var input{count} = new ModelInput();";
                        foreach (var param in Params)
                        {
                            classCode += $@"
                        input{count}.{param.FieldName} =";
                        classCode += param.ColType switch
                            {
                                "string" => $@"""{param.ColData}"";",
                                "float" =>  "(float)" + float.Parse( param.ColData).ToString() + ";",
                                _ => param.ColData + ";"
                            };

                        }
                        classCode += $"inputs.Add(input{count});";
                        count++;
                    }
                    classCode += $@"var MPath = @""{ModelPath}"";";
                    classCode += @"

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
                        var shell = new List<ExpandoObject>() { output };
                        return shell;

                    }

                }
            }
            
            ";
                    var ml_asm = CSScript.Evaluator
                                              .CompileCode(
                                                  classCode, info);
                    //GetType("ml_script+RunModel").
                    dynamic instance = ml_asm.CreateInstance("ml_script+RunModel");
                    var res = instance.Run();//GetMethod("Run").Invoke(null, null);
                    if (res is List<ExpandoObject> obj)
                    {
                        return Task.FromResult(obj);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    dynamic errorObj = new ExpandoObject();
                    errorObj.Message = ex.ToString();
                    var shell = new List<ExpandoObject>() { errorObj };
                    return Task.FromResult(shell);
                }
                finally
                {
                    if (File.Exists(AsmPath))
                    {
                        //File.Delete(AsmPath);
                    }
                }
                dynamic defaultObj = new ExpandoObject();
                defaultObj.Message = "Nothing to see..";
                var shell2 = new List<ExpandoObject>() { defaultObj };
                return Task.FromResult(shell2);
            });
            return hasil;

        }
    }
}

//unused code
/*
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
          */

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