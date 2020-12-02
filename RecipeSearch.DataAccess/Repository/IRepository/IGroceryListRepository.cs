using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IGroceryListRepository : IBaseRepository<GroceryItem>
    {
        public Dictionary<string, List<Ingredient>> GetGroceriesDictionary(string userId);
        public IEnumerable<GroceryItem> GetAll(string product, string userId);
        void AddGroceries(Recipe recipe, string[] products, string userId);
    }

    
}
