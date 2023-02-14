namespace ModelBuilder.Web.Helpers
{
    public class UIDHelper
    {
        public static string CreateNewUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}
