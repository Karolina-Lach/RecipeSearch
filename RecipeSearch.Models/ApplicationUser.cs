using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeSearch.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public List<ListOfFavourites> ListsOfFavourites { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
