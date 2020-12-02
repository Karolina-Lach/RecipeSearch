using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models
{
    public class ProductUnit
    {
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
