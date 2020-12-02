using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeSearch.Utility
{
    public enum Cuisine
    {
        american = 0,
        british = 1,
        chinese = 2,
        polish = 3,
        french = 4,
        german = 5,
        italian = 6,
        japanese = 7,
        mediterranean = 8,
        mexican = 9,
        spanish = 10,
        greek = 11,
        vietnamese = 12,
        thai = 13,
        indian = 14,
        [Display(Name="Middle Eastern")]
        middleEastern = 15
    }
}
