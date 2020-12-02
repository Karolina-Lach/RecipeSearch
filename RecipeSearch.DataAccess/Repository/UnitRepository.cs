using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class UnitRepository : BaseRepositoryAsync<Unit>, IUnitRepository
    {
        private readonly ApplicationDbContext _db;

        public UnitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        new public async Task AddAsync(Unit unit)
        {
            unit.Name = unit.Name.ToLower();
            unit.PluralName = unit.PluralName.ToLower();
            await _db.AddAsync(unit);
        }
        new public async Task AddAsync(JToken entity)
        {
            Unit unit = entity.ToObject<Unit>();
            await AddAsync(unit);
        }

        public async Task<IEnumerable<SelectListItem>> PopulateUnitData()
        {
            var unitList = (await GetAllAsync(null, true)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return unitList.OrderBy(x => x.Text);
        }
        public void Update(Unit unit)
        {
            _db.Update(unit);
            _db.SaveChanges();
        }
    }
}
