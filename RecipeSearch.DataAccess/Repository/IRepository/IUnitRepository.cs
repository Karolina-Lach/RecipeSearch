using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IUnitRepository : IBaseRepositoryAsync<Unit>
    {
        new Task AddAsync(Unit unit);
        new Task AddAsync(JToken entity);
        Task<IEnumerable<SelectListItem>> PopulateUnitData();
        void Update(Unit unit);
    }
}
