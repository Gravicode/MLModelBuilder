using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBuilder.Core.Builder
{
    public class ColumnHelper
    {
        public static string GetFieldName(string ColumnName)
        {
            return ColumnName.Replace(" ", "_").Replace("-", "_").ToCamelCase().ToCleanASCII();
        }

        

    }
    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToLowerInvariant();
        }

        public static string ToCleanASCII(this string s)
        {
            StringBuilder sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if ((int)c > 127) // you probably don't want 127 either
                    continue;
                if ((int)c < 32)  // I bet you don't want control characters 
                    continue;
                if (c == ',')
                    continue;
                if (c == '"')
                    continue;
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
