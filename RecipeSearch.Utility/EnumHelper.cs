using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RecipeSearch.Utility
{
    public static class EnumHelper 
    {
        public static IEnumerable<T> StringToList<T>(string enumString) where T : struct
        {
            if (enumString == null || enumString == "")
            {
                return new List<T>();
            }
            else
            {
                return enumString.Split(",").Select(j => Enum.Parse<T>(j)).ToList();
            }
        }
        public static string ListToString<T>(IEnumerable<T> list) where T : struct
        {
            string baseString = null;
            foreach (var item in list)
            {
                if (baseString == null)
                {
                    baseString = item.ToString();
                }
                else
                {
                    baseString = baseString + "," + item.ToString();
                }
            }

            return baseString;
        }
        public static string AddToString<T>(string baseString, string newString) where T : struct
        {
            if (Enum.IsDefined(typeof(T), newString))
            {
                if (baseString != null)
                {
                    baseString = baseString + "," + newString;
                }
                else
                {
                   baseString = newString;
                }
            }

            return baseString;
        }
        public static string RemoveFromList<T>(IEnumerable<T> list, string itemToRemove, string baseString) where T : struct
        {
            if (Enum.IsDefined(typeof(T), itemToRemove))
            {
                if (baseString != null)
                {
                    list.ToList().Remove(Enum.Parse<T>(itemToRemove));
                    baseString = ListToString(list);
                }
            }

            return baseString;
        }


    }
}
