using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeSearch.Models;
using RecipeSearch.Models.Comparers;
using RecipeSearch.Search;
using System.Security.Cryptography.X509Certificates;

namespace RecipesTest
{
    [TestClass]
    public class ProductComparerTest
    {
        private ProductNameComparer productNameComparer = new ProductNameComparer();
        [TestMethod]
        public void Equals_TheSameName()
        {
            var t1 = new Product() { Name = "apple", PluralName = "apples" }; 
            var t2 = new Product() { Name = "apple", PluralName = "apples" };


            var result = productNameComparer.Equals(t1, t2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_NotTheSameName()
        {
            var t1 = new Product() { Name = "apple", PluralName = "apples" };
            var t2 = new Product() { Name = "orange", PluralName = "oranges" };


            var result = productNameComparer.Equals(t1, t2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_NotTheSameName()
        {
            var t1 = new Product() { Name = "apple", PluralName = "apples" };
            var t2 = new Product() { Name = "orange", PluralName = "oranges" };


            var result = productNameComparer.GetHashCode(t1);
            var result2 = productNameComparer.GetHashCode(t2);

            Assert.AreNotEqual(result, result2);
        }
        [TestMethod]
        public void GetHashCode_TheSameName()
        {
            var t1 = new Product() { Name = "apple", PluralName = "apples" };
            var t2 = new Product() { Name = "apple", PluralName = "apples" };


            var result = productNameComparer.GetHashCode(t1);
            var result2 = productNameComparer.GetHashCode(t2);


            Assert.AreEqual(result2, result);
        }

        [TestMethod]
        public void Equals_TheSameReference()
        {
            var t1 = new Product() { Name = "apple", PluralName = "apples" };
            var t2 = t1;

            var result = productNameComparer.Equals(t1, t2);


            Assert.IsTrue(result);
        }
    }
}
