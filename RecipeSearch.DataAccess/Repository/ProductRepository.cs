using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json.Linq;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository
{
    public class ProductRepository : BaseRepositoryAsync<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        new public async Task AddAsync(Product product)
        {
            if (product.Name != null)
            {
                product.Name = product.Name.ToLower();
            }
            if(product.PluralName !=null)
            {
                product.PluralName = product.PluralName.ToLower();
            }
            
            await _db.AddAsync(product);
        }
        new public async Task AddAsync(JToken entity)
        {
            Product product = entity.ToObject<Product>();
            await AddAsync(product);
        }

        public async Task<IEnumerable<SelectListItem>> PopulateProductData()
        {
            var productList = (await GetAllAsync(null, true)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return productList.OrderBy(x => x.Text);
        }
        public void Update(Product product)
        {
            product.Name = product.Name.ToLower();
            product.PluralName = product.PluralName.ToLower();
            _db.Update(product);
        }
    }
}
