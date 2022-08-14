using ModelBuilder.Core.Builder;
using ModelBuilder.Models;
using ModelBuilder.Web.Data;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;

namespace ModelBuilder.Web.Services
{
    public class BatchInferenceService
    {
        bool IsInferencing;
        string[] Columns;
        string[] ColumnTypes;
        MLModel TaskObject;
        public List<ModelParameter> ParamObj { set; get; }

        public BatchInferenceService(long ModelId)
        {
            MLModelService svc = new MLModelService();
            TaskObject = svc.GetDataById(ModelId);
            GenerateColumns();
        }
        void GenerateColumns()
        {

            Columns = TaskObject.Kolom.Split(",");
            ColumnTypes = TaskObject.TipeKolom.Split(",");
            if (ParamObj == null)
            {
                ParamObj = new();
            }
            for (var i = 0; i < Columns.Length; i++)
            {

                var fieldName = ColumnHelper.GetFieldName(Columns[i]);
                object data = null;
                object dataOut = null;
                string OutTypes = "float[]";
                string InTypes = "string";
                switch (ColumnTypes[i])
                {
                    case "System.String":
                        data = string.Empty;
                        OutTypes = "string";
                        InTypes = "string";
                        break;
                    case "System.Boolean":
                        data = true;
                        OutTypes = "bool";
                        InTypes = "bool";
                        break;
                    case "System.Single":
                        data = 0f;
                        OutTypes = "float";
                        InTypes = "float";
                        break;
                    default:
                        data = string.Empty;
                        break;

                }
                if (Columns[i] == TaskObject.LabelName)
                {

                    switch (TaskObject.Tipe)
                    {
                        case "MultiClassification":
                        case "BinaryClassification":
                            OutTypes = "UInt32";


                            break;
                        default:
                            break;
                    }

                }
                ParamObj.Add(new ModelParameter() { ColOutType = OutTypes, FieldName = fieldName, ColName = Columns[i], ColType = InTypes, ColData = string.Empty });
            }

        }

        public async Task<OutputCls> Inference(InferenceModelParam param)
        {
            var output = new OutputCls();
            if (IsInferencing)
            {
                output.Message = "masih ada batch yang berlangsung..";
                return output;
            }
            IsInferencing = true;
            try
            {
                ModelExecutor exec = new ModelExecutor();
                for (var i = 0; i < ParamObj.Count; i++)
                {
                    if (string.IsNullOrEmpty(ParamObj[i].ColData))
                    {
                        if (ParamObj[i].ColType == "float")
                        {
                            ParamObj[i].ColData = "0";
                        }
                    }
                }
                var ParamObjs = new List<ModelParameter[]>();
                foreach (var payload in param.Payload)
                {
                    var obj = ParamObj.ToArray();
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
                    foreach (var item in values)
                    {
                        var fieldName = item.Key; //ColumnHelper.GetFieldName(item.Key);
                        var col = obj.FirstOrDefault(x => x.FieldName == fieldName);
                        if (col != null)
                        {
                            col.ColData = item.Value;
                        }
                    }
                    ParamObjs.Add(obj);
                }
                var res = await exec.ExecuteCodeBatch(ParamObjs, TaskObject.ModelPath, TaskObject.Tipe);

                output.Data = res;
                output.IsSucceed = true;

            }
            catch (Exception ex)
            {
                output.Message = ex.ToString();
                output.IsSucceed = false;
            }
           
            //var Result = PropertyList(res);
            IsInferencing = false;
            return output;
        }
        public string PropertyList(System.Dynamic.ExpandoObject obj)
        {
            var props = obj.GetType().GetProperties();
            var sb = new StringBuilder();

            foreach (KeyValuePair<string, object> kvp in obj) // enumerating over it exposes the Properties and Values as a KeyValuePair
                if (kvp.Value is Microsoft.ML.Data.VBuffer<Single>)
                {
                    var vals = string.Join(",", ((Microsoft.ML.Data.VBuffer<Single>)kvp.Value).GetValues().ToArray());
                    sb.AppendLine($"{kvp.Key} = {vals}");
                }
                else
                    sb.AppendLine($"{kvp.Key} = {kvp.Value}");
            return sb.ToString();
        }
    }
}
