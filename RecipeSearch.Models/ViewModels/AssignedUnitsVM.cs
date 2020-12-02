using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.Models.ViewModels
{
    public class AssignedUnitsVM
    {
        public int UnitId { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
