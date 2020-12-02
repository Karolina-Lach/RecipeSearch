using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RecipeSearch.DataAccess.Data;
using RecipeSearch.DataAccess.Repository;
using RecipeSearch.DataAccess.Repository.IRepository;
using RecipeSearch.Models;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeSearch.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        //private readonly IUnitRepository _unitRepo;
        //private readonly IProductRepository _productRepo;
        //private readonly IRecipeRepository _recipeRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db,
                             IUnitOfWork unitOfWork,
                             UserManager<IdentityUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            //_unitRepo = unitRepo;
            //_productRepo = productRepo;
            //_recipeRepo = recipeRepo;
            _unitOfWork = unitOfWork;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_db.Roles.Any(r => r.Name == SD.Role_Admin))
            {

                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "carolina.lach@gmail.com",
                    Email = "carolina.lach@gmail.com",
                    EmailConfirmed = true
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "carolina.lach@gmail.com").FirstOrDefault();

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            if (!_db.Units.Any())
            {
                Load(@"C:\Users\klach\source\repos\RecipeSearch\RecipeSearch\wwwroot\seed\units.json");
            }
            if (!_db.Products.Any())
            {
                Load(@"C:\Users\klach\source\repos\RecipeSearch\RecipeSearch\wwwroot\seed\products.json");
            }
            if (!_db.Recipes.Any())
            {
                Load(@"C:\Users\klach\source\repos\RecipeSearch\RecipeSearch\wwwroot\seed\recipes.json");
                Load(@"C:\Users\klach\source\repos\RecipeSearch\RecipeSearch\wwwroot\seed\recipes_part2.json");
                //Load(@"C:\Users\klach\source\repos\RecipeSearch\RecipeSearch\wwwroot\seed\recipes_part3.json");
            }
        }

        public void Load(string jsonPath)
        {
            var dataText = System.IO.File.ReadAllText(jsonPath);
            JObject source = JObject.Parse(dataText);
            foreach (var jsonEntity in source)
            {
                JObject entityObject = JObject.FromObject(jsonEntity);
                List<JToken> Lookups = entityObject.SelectTokens("..Lookup").ToList();
                foreach (var lookup in Lookups)
                {
                    string listName = lookup["Repository"].ToString();
                    if (listName != null)
                    {

                        //int? id = Lookup(listName, lookup["Name"].ToString());
                        dynamic lookupRepository = Lookup(listName);
                        int? id = lookupRepository.Lookup(lookup["Name"].ToString());
                        if (id != null || id != 0)
                        {
                            lookup.Parent.Parent.Replace(JToken.FromObject(id));
                        }



                    }
                }

                //Type TypeOfInstance = Type.GetType("Models.Product");
                dynamic repo = Lookup(jsonEntity.Key.ToString());

                foreach (var entityData in entityObject)
                {
                    foreach (var item in entityData.Value)
                    {
                        if(jsonEntity.Key.ToString() == "Recipe")
                        {
                            var ingredients = item["Ingredients"].AsEnumerable().Select(x => x.ToObject<Ingredient>());
                            var steps = item["Steps"].AsEnumerable().Select(x => x.ToObject<Step>());
                            repo.AddAsync(item, ingredients, steps);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            repo.AddAsync(item);
                        }
                    }
                }
            }
            _unitOfWork.Save();
        }

        private dynamic Lookup(string entityName)
        {
            return entityName switch
            {
                "Unit" => _unitOfWork.Unit,
                "Product" => _unitOfWork.Product,
                "Recipe" => _unitOfWork.Recipe,
                _ => null,
            };
        }
    }
}
