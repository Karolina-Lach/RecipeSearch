using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeSearch.Models
{
    public class Step : BaseModel
    {
        [Range(1,100)]
        public int Number { get; set; }


        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
