using System;

namespace module_03
{
    public static class StringExtension
    {
        public static bool IsPalindrome(this string str)
        {
            char[] ch = str.ToCharArray();
            Array.Reverse(ch);
            var rev = new string(ch);
            return str.Equals(rev, StringComparison.OrdinalIgnoreCase);
        }
    } 
}