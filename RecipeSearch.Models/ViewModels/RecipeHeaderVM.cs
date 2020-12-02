using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class RecipeHeaderVM
    {
        public RecipeHeaderVM() {}
        public RecipeHeaderVM(Recipe recipe)
        {
            RecipeId = recipe.Id;
            Name = recipe.Name;
            Summary = recipe.Summary;
            PrepTime = recipe.PrepTime;
            PrepTimeDisplay = recipe.PrepTimeDisplay;
            Servings = recipe.Servings;
            ImageUrl = recipe.ImageUrl;
            Source = recipe.Source;
            IngredientsCount = recipe.Ingredients.Count;
        }

        public RecipeHeaderVM(Recipe recipe, bool isLiked)
        {
            RecipeId = recipe.Id;
            Name = recipe.Name;
            Summary = recipe.Summary;
            PrepTime = recipe.PrepTime;
            PrepTimeDisplay = recipe.PrepTimeDisplay;
            Servings = recipe.Servings;
            ImageUrl = recipe.ImageUrl;
            Source = recipe.Source;
            IngredientsCount = recipe.Ingredients.Count;
            IsLiked = isLiked;
        }
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public int PrepTime { get; set; }
        public string PrepTimeDisplay { get; set; }
        public int Servings { get; set; }
        public string ImageUrl { get; set; }
        public string Source { get; set; }
        public int IngredientsCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
