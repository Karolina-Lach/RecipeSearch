using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RecipeSearch.Models
{
    public class Product : BaseModel
    {
        public Product()
        {
            ProductUnit = new List<ProductUnit>();
        }
        public Product(string name)
        {
            Name = name;
            ProductUnit = new List<ProductUnit>();
        }
        public Product(string name, string pluralName)
        {
            Name = name;
            PluralName = pluralName;
            ProductUnit = new List<ProductUnit>();
        }

        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name ="Plural name")]
        [MaxLength(50, ErrorMessage ="The name is too long.")]
        public string PluralName { get; set; }

        [Display(Name="Units")]
        public ICollection<ProductUnit> ProductUnit { get; set; }

       

        public void AddProductUnit(Unit unit)
        {
            if (ProductUnit == null)
            {
                ProductUnit = new List<ProductUnit>();
            }
            if (ProductUnit.Where(pu => pu.Unit == unit).Count() == 0)
            {
                var productUnit = new ProductUnit { ProductId = this.Id, UnitId = unit.Id };
                ProductUnit.Add(productUnit);
            }
        }

        public void AddProductUnit(string unit)
        {
            if (ProductUnit == null)
            {
                ProductUnit = new List<ProductUnit>();
            }
            var productUnit = new ProductUnit { ProductId = this.Id, UnitId = int.Parse(unit) };
            if (!ProductUnit.Contains(productUnit))
            {
                ProductUnit.Add(productUnit);
            }
        }

        public void RemoveUnit(Unit unit)
        {
            if (ProductUnit != null)
            {
                ProductUnit unitsToRemove = ProductUnit.FirstOrDefault(p => p.UnitId == unit.Id);
                if (unitsToRemove != null)
                {
                    ProductUnit.Remove(unitsToRemove);
                }
            }
        }

        public IEnumerable<Unit> GetUnits()
        {
            var unitList = ProductUnit.Select(i => i.Unit);

            return unitList;
        }
    }
}
