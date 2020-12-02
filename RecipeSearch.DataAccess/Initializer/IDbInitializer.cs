using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Initializer
{
    public interface IDbInitializer
    {

        void Initialize();
        void Load(string jsonPath);
    }
}
