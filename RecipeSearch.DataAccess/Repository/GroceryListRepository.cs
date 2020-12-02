using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeSearch.DataAccess.Repository
{
    public class GroceryListRepository : BaseRepository<GroceryItem>, IGroceryListRepository
    {
        private readonly ApplicationDbContext _db;
        public GroceryListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Dictionary<string, List<Ingredient>> GetGroceriesDictionary(string userId)
        {
            var list = GetAll(x => x.ApplicationUserId == userId,
                includeProperties: "Ingredient,Ingredient.Product,Ingredient.Unit,Ingredient.Recipe").Select(x => x.Ingredient).ToList();

            return GroceryItem.CreateGroceriesDictionary(list);
        }

        public IEnumerable<GroceryItem> GetAll(string product, string userId)
        {
            var list = GetAll(x => x.ApplicationUserId == userId, includeProperties: "Ingredient,Ingredient.Product")
                                    .Where(x => x.Ingredient.Product.Name == product.ToLower());

            return list;
        }
        public void AddGroceries(Recipe recipe, string[] products, string userId)
        {
            List<Ingredient> productToIngredient = new List<Ingredient>();
            foreach(var product in products)
            {
                var item = recipe.Ingredients.Where(x => x.Product.Name == product.ToLower());
                productToIngredient.AddRange(item);
            }
            IEnumerable<GroceryItem> groceries = productToIngredient.Select(x => new GroceryItem() { ApplicationUserId =userId, IngredientId = x.Id }).ToList();
            AddRange(groceries);
        }
    }
}
