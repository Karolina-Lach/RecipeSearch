using RecipeSearch.Utility;
using System.Collections.Generic;

namespace RecipeSearch.Models.ViewModels
{
    public class SearchCriteria
    {
        public SearchCriteria() {}
        public SearchCriteria(string products, string properties, string cuisines, string meals, bool allMatched, int prepTime)
        {

            products = products.Replace(@"\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            Products = products == "" ? new string[0] : products.Split(',');
            
            properties = properties.Replace(@"\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            Properties = properties == "" ? new string[0] : properties.Split(',');

            cuisines = cuisines.Replace(@"\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            SelectedCuisines = cuisines == "" ? new string[0] : cuisines.Split(',');

            meals = meals.Replace(@"\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            SelectedMeals = meals == "" ? new string[0] : meals.Split(',');

            AreAllPropertiesMatched = allMatched;
            PrepTime = prepTime;
        }
        public List<string> AvailableProducts { get; set; }
        public List<Cuisine> AvailableCuisines { get; set; }
        public List<MealType> AvailableMealTypes { get; set; }


        public string[] Properties { get; set; }
        public string[] SelectedCuisines { get; set; }
        public string[] SelectedMeals { get; set; }
        public string[] Products { get; set; }
        public bool AreAllPropertiesMatched { get; set; }
        public int PrepTime { get; set; }
    }
}
