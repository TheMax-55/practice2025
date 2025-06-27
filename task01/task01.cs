using System;
namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            input = input.ToLower();
            string cleaned = string.Empty;
            string reversed = string.Empty;
            foreach (char sym in input)
            {
                if (!Char.IsWhiteSpace(sym) && !Char.IsPunctuation(sym))
                {
                    cleaned += sym;
                    reversed = sym + reversed;
                }
            }
            if (string.IsNullOrEmpty(cleaned))
                return false;
            return cleaned == reversed;
        }
    }
}
