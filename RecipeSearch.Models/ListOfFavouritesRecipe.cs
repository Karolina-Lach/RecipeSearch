using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeSearch.Models
{
    public class ListOfFavouritesRecipe
    {
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
        public int ListOfFavouritesId { get; set; }
        [ForeignKey("ListOfFavouritesId")]
        public ListOfFavourites ListOfFavourites { get; set; }
    }
}
