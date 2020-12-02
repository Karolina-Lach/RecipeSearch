using Newtonsoft.Json;
using RecipeSearch.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RecipeSearch.Models
{
    public class Recipe : BaseModel
    {
        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
            Cuisines = new List<Cuisine>();
            MealTypes = new List<MealType>();
        }
        /******* RECIPE HEADER *****************/
        [MaxLength(200)]
        [AllowNull]
        public string Summary { get; set; }

        [Range(1, 1000)]
        [Display(Name = "Prepartion time in minutes")]
        [Required]
        public int PrepTime { get; set; }

        [Range(1, 500)]
        [Required]
        public int Servings { get; set; }

        [MaxLength(50)]
        public string Source { get; set; }

        [Display(Name="Image")]
        public string ImageUrl { get; set; }
        [NotMapped]
        public string PrepTimeDisplay
        {
            get
            {
                if (PrepTime > 60)
                {
                    int min = PrepTime % 60;
                    int hours = (PrepTime - min) / 60;

                    return hours + "h " + min + "min";
                }
                else
                {
                    return PrepTime + "min";
                }
            }
            set { }
        }

        // FLAGS
        [Display(Name = "No dairy")]
        public bool NoDairy { get; set; }
        [Display(Name ="No gluten")]
        public bool NoGluten { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool Healthy { get; set; }
        [Display(Name="Cuisine")]
        public string CuisinesString { get; set; }
        [Display(Name="Meal types")]
        public string MealTypesString { get; set; }



        /**************** RECIPE DETAILS *************************/
        //[NotMapped] ---> fluent API
        [JsonIgnore]
        public List<Step> Steps { get; set; }
        [JsonIgnore]
        //[NotMapped] ---> fluent API
        public List<Ingredient> Ingredients { get; set; }

        public List<ListOfFavouritesRecipe> ListsOfFavourites { get; set; }

        /***************** METHODS ******************/
        [NotMapped]
        [JsonIgnore]
        public IEnumerable<Cuisine> Cuisines
        {
            get
            {
                return EnumHelper.StringToList<Cuisine>(CuisinesString);
            }
            set
            { }
        }
        [NotMapped]
        [JsonIgnore]
        public IEnumerable<MealType> MealTypes
        {
            get
            {
                //if (MealTypesString == null || MealTypesString == "")
                //{
                //    return new List<MealType>();
                //}
                //else
                //{
                //    return MealTypesString.Split(",").Select(j => Enum.Parse<MealType>(j));
                //}
                return EnumHelper.StringToList<MealType>(MealTypesString);
            }
            set
            { }
        }

        public void AddCuisine(string cuisineString)
        {
            //if (Enum.IsDefined(typeof(Cuisine), cuisineString))
            //{
            //    if (CuisinesString != null)
            //    {
            //        CuisinesString = CuisinesString + "," + cuisineString;
            //    }
            //    else
            //    {
            //        CuisinesString = cuisineString;
            //    }
            //}
            CuisinesString = EnumHelper.AddToString<Cuisine>(CuisinesString, cuisineString);
        }
        public void RemoveCuisine(string cuisineString)
        {
            CuisinesString = EnumHelper.RemoveFromList(Cuisines, cuisineString, CuisinesString);
        }
        public void AddMealType(string mealTypeString)
        {
            MealTypesString = EnumHelper.AddToString<MealType>(MealTypesString, mealTypeString);
        }
        public void RemoveMealType(string mealTypesString)
        {
            //if (Enum.IsDefined(typeof(MealType), mealTypesString))
            //{
            //    if (MealTypesString != null)
            //    {
            //        var mealTypes = MealTypes.ToList();
            //        mealTypes.Remove(Enum.Parse<MealType>(mealTypesString));

            //        MealTypesString = MealTypesListToString(mealTypes);
            //    }
            //}

            MealTypesString = EnumHelper.RemoveFromList(MealTypes, mealTypesString, MealTypesString);
        }

        public IEnumerable<Product> GetListOfProducts()
        {
            IEnumerable<Product> products = Ingredients.Select(x => x.Product);

            return products;
        }

        public bool ContainsMealType(MealType meal)
        {
            return MealTypes.Contains(meal);
        }

        public bool ContainsMealType(string meal)
        {
            try
            {
                MealType mealEnum = Enum.Parse<MealType>(meal);
                bool result = ContainsMealType(mealEnum);
                return result;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool ContainsCuisine(Cuisine cuisine)
        {
            return Cuisines.Contains(cuisine);
        }
        public bool ContainsCuisine(string cuisine)
        {
            try
            {
                Cuisine cuisineEnum = Enum.Parse<Cuisine>(cuisine);
                bool result = ContainsCuisine(cuisineEnum);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
