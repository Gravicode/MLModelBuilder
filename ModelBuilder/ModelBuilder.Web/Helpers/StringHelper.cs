using System;
namespace ModelBuilder.Web.Helpers
{
	public class StringHelper
	{
		public StringHelper()
		{
		}
	}
    public static class StringExtensions
    {
        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    if (str.Contains(value))
                        return true;
                }
            }

            return false;
        }
    }

}

