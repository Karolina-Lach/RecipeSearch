using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class FoundRecipe
    {
        public Recipe Recipe { get; set; }
        public bool IsLiked { get; set; }
        public int NumberOfProductsMatched { get; set; }
        public int NumberOfPropertiesMatched { get; set; }
        public bool HasMealType { get; set; }
        public bool HasCuisine { get; set; }
        public bool IsInTime { get; set; }

        public bool ProductsDisplay { get; set; }
        public bool PropertiesDisplay { get; set; }
        public bool MealDisplay { get; set; }
        public bool CuisineDisplay { get; set; }
        public bool TimeDisplay { get; set; }
    }
}
