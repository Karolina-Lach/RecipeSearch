using Microsoft.Extensions.Configuration;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Unit = new UnitRepository(_db);
            Recipe = new RecipeRepository(_db);
            GroceryList = new GroceryListRepository(_db);
            ListOfFavourites = new ListOfFavouritesRepository(_db, configuration);
        }

        public IProductRepository Product { get;  private set; }
        public IUnitRepository Unit { get;  private set; }

       

        public IRecipeRepository Recipe { get; private set; }
        public IGroceryListRepository GroceryList { get; private set; }
        public IListOfFavouritesRepository ListOfFavourites { get;  private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
