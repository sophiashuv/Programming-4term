using System;
using System.Globalization;

namespace Ex_pr_02
{
    public class Validation
    {
        public static double ValidatePrice(double value)
        {
            string strValue = value.ToString(CultureInfo.InvariantCulture).
                IndexOf(".", StringComparison.Ordinal) == -1 ? value.ToString(CultureInfo.InvariantCulture) 
                                                               + "." : value.ToString(CultureInfo.InvariantCulture);
            if (strValue.Substring(strValue.IndexOf(".", StringComparison.Ordinal)).Length > 3)
            {
                throw new ArgumentException("Price must have two digits after coma.");
            }
            return value;
        }
        
        public static double ValidatePositive(double value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Mast be positive.");
            }
            return value;
        }
        
        public static string ValidateFile(string value, string end)
        {
            if (!value.EndsWith(end))
                throw new WrongFileFormatException($"Incorrect .{end} format.");
            return value;
        }
    }
    
    [Serializable]
    public class WrongFileFormatException : Exception
    {
        public WrongFileFormatException() { }

        public WrongFileFormatException(string message)
            : base(message) { }
    }
}