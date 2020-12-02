using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class LikeButtonVM
    {
        public LikeButtonVM(int recipeId, bool isLiked)
        {
            RecipeId = recipeId;
            IsLiked = isLiked;
        }
        public int RecipeId { get; set; }
        public bool IsLiked { get; set; }
    }
}
