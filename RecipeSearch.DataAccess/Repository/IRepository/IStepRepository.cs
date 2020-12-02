using RecipeSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeSearch.DataAccess.Repository.IRepository
{
    public interface IStepRepository : IBaseRepository<Step>
    {
        void Update(Step step);
    }
}
