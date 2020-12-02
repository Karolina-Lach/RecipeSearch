using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeSearch.Utility
{
    public static class StringHelper
    {
        public static string FirstLetterToUpper(string stringToConvert)
        {
            return stringToConvert.First().ToString().ToUpper() + stringToConvert.Substring(1);
        }
    }
}
