using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeSearch.Models
{
    public class ListOfFavourites : BaseModel
    {
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public List<ListOfFavouritesRecipe> FavouriteRecipes { get; set; }
    }
}
