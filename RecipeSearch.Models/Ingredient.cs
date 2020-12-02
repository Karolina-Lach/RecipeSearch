using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RecipeSearch.Models
{
    public class Ingredient : BaseModel
    {

        public string Comment { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [Display(Name ="Amount")]
        [Range(0.01, 99999.99)]
        [RegularExpression(@"^([0-9]+(?:[\.\,][0-9]{1,2})?)$", ErrorMessage = "Enter decimal number with maximum 2 decimal places.")]
        public decimal Measurement { get; set; }


        [RequiredIf("Product.PluralName", ErrorMessage = "Unit cannot be null when using this product.")]
        public int? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; }
        
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }

        public string Display()
        {
            string result = Measurement.ToString();
            if (UnitId != null)
            {
                if (Measurement > 1)
                {
                    result = result + " " + Unit.PluralName + " " + Product.Name;
                }
                else
                {
                    result = result + " " + Unit.Name + " " + Product.Name;
                }
            }
            else
            {
                if (Measurement > 1 && Product.PluralName != null)
                {
                    result = result + " " + Product.PluralName;
                }
                else
                {
                    result = result + " " + Product.Name;
                }
            }
            return result;
        }
        public string DisplayMeasurement()
        {
            string result = Measurement.ToString() + " ";
            if(Unit != null)
            {
                if(Measurement > 1)
                {
                    result += Unit.PluralName + " ";
                }else
                {
                    result += Unit.Name + " ";
                }
            }
            result += "for recipe " + Recipe.Name;

            return result;
        }
        class RequiredIfAttribute : ValidationAttribute
        {
            public string PropertyName { get; set; }
            public RequiredIfAttribute(string propertyName, string errorMessage ="")
            {
                PropertyName = propertyName;
                ErrorMessage = errorMessage;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var instance = validationContext.ObjectInstance;
                var type = instance.GetType();
                var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

                if(propertyValue == null && value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;

            }
        }
    }
}

