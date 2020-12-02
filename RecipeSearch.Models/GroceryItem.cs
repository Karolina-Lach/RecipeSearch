using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeSearch.Models
{
    public class GroceryItem : BaseModel
    {
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int IngredientId { get; set; }
        [ForeignKey("IngredientId")]
        public Ingredient Ingredient { get; set; }

        public static Dictionary<string, List<Ingredient>> CreateGroceriesDictionary(IEnumerable<Ingredient> ingredients)
        {
            Dictionary<string, List<Ingredient>> groceriesDict = new Dictionary<string, List<Ingredient>>();
            foreach (var item in ingredients)
            {
                var key = StringHelper.FirstLetterToUpper(item.Product.Name);
                if (groceriesDict.ContainsKey(key))
                {

                    groceriesDict[key].Add(item);
                }
                else
                {
                    groceriesDict.Add(key, new List<Ingredient>() { item });
                }
            }
            return groceriesDict;
        }

    }
}
