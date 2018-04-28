using System;
using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TiIEnumerableExtension;

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

            var expected = new List<T>()
            {
                new T{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new T{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void MyOwnLINQ_find_products_that_price_between_200_and_500_supplier_odds()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MyOwnWhere(p => p.Price >= 200 && p.Price <= 500 && p.Supplier == "Odd-e");

            foreach (var item in actual)
            {
                Console.WriteLine(item.Price);
            }
            var expected = new List<T>()
            {
                new T{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new T{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void MyOwnLINQ_Where_GetEmployee()
        {
            var employees = RepositoryFactory.GetEmployees();
            //var actual = YourOwnLinq.MyOwnWhere(
            //    employees, e => e.Age >= 25 && e.Age <= 40).ToList();
            var actual = employees.MyOwnWhere(e => e.Age >= 25 && e.Age <= 40).ToList();

            var expected = new List<Employee>()
            {
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Mary", Role=RoleType.OP, MonthSalary=180, Age=26, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6}
            };

            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}

internal class WithoutLinq
{
    public static List<T> FindProductByPrice(IEnumerable<T> products, int lowBoundary, int highBoundary,
        string supplier)
    {
        var matchedProducts = new List<T>();
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

namespace TiIEnumerableExtension
{
    internal static class YourOwnLinq
    {
        public static IEnumerable<TSource> MyOwnWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<T> MyOwnForeach(IEnumerable<T> products, Func<T, bool> predicate)
        {
            //foreach (var p in source)
            //{
            //    if (predicate(p))
            //    {
            //        //yield會延遲執行，只有真正要用時才實際執行
            //        yield return p;
            //    }
            //}

            var enumerator = products.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}
