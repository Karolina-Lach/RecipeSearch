using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeSearch.Models
{
    public abstract class BaseModel
    {
        public virtual int Id { get; set; }
        //[RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name cannot contain any numbers or special characters.")]
        public virtual string Name { get; set; }

    }
}
