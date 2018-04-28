using System;
using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = WithoutLinq.FindProductByPrice(products, 200, 500, "Odd-e");

            var expected = new List<Product>()
            {
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void MyOwnLINQ_Where()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = YourOwnLinq.MyOwnWhere(products, 
                p => p.Price >= 200 && p.Price <= 500 && p.Supplier == "Odd-e");

            var expected = new List<Product>()
            {
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}

internal class WithoutLinq
{
    public static List<Product> FindProductByPrice(IEnumerable<Product> products, int lowBoundary, int highBoundary,
        string supplier)
    {
        var matchedProducts = new List<Product>();
        foreach (var product in products)
        {
            if (product.Price >= lowBoundary && product.Price <= highBoundary && product.Supplier.Equals(supplier))
            {
                matchedProducts.Add(product);
            }
        }

        return matchedProducts;
    }
}

internal class YourOwnLinq
{
    public static List<Product> MyOwnWhere(IEnumerable<Product> products, Func<Product, bool> func)
    {
        var matchedProducts = new List<Product>();
        foreach (var product in products)
        {
            if (func(product))
            {
                matchedProducts.Add(product);
            }
        }

        return matchedProducts;
    }
}