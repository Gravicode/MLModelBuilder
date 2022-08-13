﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers;
using Microsoft.ML;

namespace MLModel1_WebApi1
{
    public partial class MLModel1
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"cylinders", @"cylinders"),new InputOutputColumnPair(@"displacement", @"displacement"),new InputOutputColumnPair(@"horsepower", @"horsepower"),new InputOutputColumnPair(@"acceleration", @"acceleration"),new InputOutputColumnPair(@"model year", @"model year"),new InputOutputColumnPair(@"origin", @"origin")})      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"weight",outputColumnName:@"weight"))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"car name",outputColumnName:@"car name"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"cylinders",@"displacement",@"horsepower",@"acceleration",@"model year",@"origin",@"weight",@"car name"}))      
                                    .Append(mlContext.Regression.Trainers.FastForest(new FastForestRegressionTrainer.Options(){NumberOfTrees=4,NumberOfLeaves=4,FeatureFraction=1F,LabelColumnName=@"mpg",FeatureColumnName=@"Features"}));

            return pipeline;
        }
    }
}