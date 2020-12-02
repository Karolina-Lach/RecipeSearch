using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Product { get; }
        IUnitRepository Unit { get; }
        IRecipeRepository Recipe { get; }
        IGroceryListRepository GroceryList{ get; }
   
        IListOfFavouritesRepository ListOfFavourites { get; }

        void Save();
    }
}
