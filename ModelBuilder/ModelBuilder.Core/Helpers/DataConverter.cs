using Microsoft.ML.Data;
using SoftCircuits.CsvParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Helpers
{
    public class DataHelper
    {
        public enum dataType
        {
            System_Boolean = 0,
            System_Int32 = 1,
            System_Int64 = 2,
            System_Double = 3,
            System_DateTime = 4,
            System_String = 5
        }

        public dataType ParseString(string str)
        {

            bool boolValue;
            Int32 intValue;
            Int64 bigintValue;
            double doubleValue;
            DateTime dateValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (bool.TryParse(str, out boolValue))
                return dataType.System_Boolean;
            else if (Int32.TryParse(str, out intValue))
                return dataType.System_Int32;
            else if (Int64.TryParse(str, out bigintValue))
                return dataType.System_Int64;
            else if (double.TryParse(str, out doubleValue))
                return dataType.System_Double;
            else if (DateTime.TryParse(str, out dateValue))
                return dataType.System_DateTime;
            else return dataType.System_String;

        }


        /// <summary>
        /// Gets the datatype for the Datacolumn column
        /// </summary>
        /// <param name="column">Datacolumn to get datatype of</param>
        /// <param name="dt">DataTable to get datatype from</param>
        /// <param name="colSize">ref value to return size for string type</param>
        /// <returns></returns>
        public Type GetColumnType(DataColumn column, DataTable dt, ref int colSize)
        {

            Type T;
            DataView dv = new DataView(dt);
            //get smallest and largest values
            string colName = column.ColumnName;

            dv.RowFilter = "[" + colName + "] = MIN([" + colName + "])";
            DataTable dtRange = dv.ToTable();
            string strMinValue = dtRange.Rows[0][column.ColumnName].ToString();
            int minValueLevel = (int)ParseString(strMinValue);

            dv.RowFilter = "[" + colName + "] = MAX([" + colName + "])";
            dtRange = dv.ToTable();
            string strMaxValue = dtRange.Rows[0][column.ColumnName].ToString();
            int maxValueLevel = (int)ParseString(strMaxValue);
            colSize = strMaxValue.Length;

            //get max typelevel of first n to 50 rows
            int sampleSize = Math.Max(dt.Rows.Count, 50);
            int maxLevel = Math.Max(minValueLevel, maxValueLevel);

            for (int i = 0; i < sampleSize; i++)
            {
                maxLevel = Math.Max((int)ParseString(dt.Rows[i][column].ToString()), maxLevel);
            }

            string enumCheck = ((dataType)maxLevel).ToString();
            T = Type.GetType(enumCheck.Replace('_', '.'));

            //if typelevel = int32 check for bit only data & cast to bool
            if (maxLevel == 1 && Convert.ToInt32(strMinValue) == 0 && Convert.ToInt32(strMaxValue) == 1)
            {
                T = Type.GetType("System.Boolean");
            }

            if (maxLevel != 5) colSize = -1;


            return T;
        }

        
    }
    public class DataConverter
    {
        public static string[] GetColumnTypes(DataTable dt)
        {
            if (dt.Columns.Count <= 0 || dt.Rows.Count <= 0) return null;
            try
            {
                var cols = new List<string>();
                var dh = new DataHelper();
                foreach (DataColumn dc in dt.Columns)
                {
                    var rowVal = dt.Rows[0][dc].ToString();
                    var tipe = dh.ParseString(rowVal);
                    string kind = "System.String";
                    switch (tipe)
                    {
                        case DataHelper.dataType.System_DateTime:
                            kind = "System.DateTime";
                            break;
                        case DataHelper.dataType.System_Int64:
                            kind = "System.Single";
                            break;
                        case DataHelper.dataType.System_String:
                            kind = "System.String";
                            break;
                        case DataHelper.dataType.System_Double:
                            kind = "System.Single";
                            break;
                        case DataHelper.dataType.System_Boolean:
                            kind = "System.Boolean";
                            break;
                        case DataHelper.dataType.System_Int32:
                            kind = "System.Single";
                            break;
                    }
                    cols.Add(kind);
                }
                return cols.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
          
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath, char Separator=',')
        {
            DataTable dt = new DataTable();
            string[]? columns;

            using CsvReader reader = new CsvReader(strFilePath, new CsvSettings() { ColumnDelimiter = Separator, QuoteCharacter = '"' }); 
            var row = 0;
            string[] headers = null;
            while ((columns = reader.ReadRow()) != null)
            {
                if (row <= 0)
                {
                    headers = columns;
                    foreach (string header in columns)
                    {
                        dt.Columns.Add(header);
                    }

                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = columns[i];
                    }
                    dt.Rows.Add(dr);

                }
                //Console.WriteLine(string.Join(", ", columns));
                row++;
            }
            /*
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }*/


            return dt;
        }

        public static DataTable GetDataPreview(string strFilePath, int LimitRow=10)
        {
            int RowIndex = 0;
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    RowIndex++;
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                    if (RowIndex >= LimitRow) break;
                }

            }


            return dt;
        }
        public static TextLoader.Column[] GetHeaderColumns(string strFilePath)
        {
            var Cols = new List<TextLoader.Column>();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                int idx = 0;
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    Cols.Add(new TextLoader.Column(header,DataKind.String,idx++));
                    
                }
                return Cols.ToArray();
            }
        }
    }
}
