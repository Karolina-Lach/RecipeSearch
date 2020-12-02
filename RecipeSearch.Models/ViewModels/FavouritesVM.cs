using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class FavouritesVM
    {
        public int SelectedList { get; set; }
        public IEnumerable<ListOfFavourites> AvailableLists { get; set; }
        public IEnumerable<SelectListItem> FavouriteLists { get; set; }
    }
}
