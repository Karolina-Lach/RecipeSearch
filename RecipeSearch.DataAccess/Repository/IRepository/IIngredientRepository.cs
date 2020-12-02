using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IIngredientRepository : IBaseRepository<Ingredient>
    {
        void Update(Ingredient ingredient);
    }
}
