using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class RecipeUpsertVM
    {
        public RecipeUpsertVM()
        {
            Recipe = new Recipe();
            Product = new Product();      
        }
        public Recipe Recipe { get; set; }
        public Product Product { get; set; }
        public Ingredient Ingredient { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
    }
}
