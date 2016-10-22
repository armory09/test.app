using System.Text.RegularExpressions;

namespace Me.DataLayer.Util
{
    public static class ExpressionFilter
    {
        public static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
    }
}
