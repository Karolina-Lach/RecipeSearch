using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IBaseRepositoryAsync<Product>
    {
        new Task AddAsync(Product product); 
        new Task AddAsync(JToken entity);
        Task<IEnumerable<SelectListItem>> PopulateProductData();
        void Update(Product product);
    }
}
