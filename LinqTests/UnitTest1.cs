using System;
using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using NSubstitute.Routing.Handlers;
using TiIEnumerableExtension;
using Product = LinqTests.Product;

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
        public void MyOwnLINQ_find_products_that_price_between_200_and_500_supplier_odds()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MyOwnWhere(p => p.Price >= 200 && p.Price <= 500 && p.Supplier == "Odd-e");

           
            var expected = new List<Product>()
            {
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void MyOwnLINQ_Where_GetEmployee()
        {
            var employees = RepositoryFactory.GetEmployees();
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

        [TestMethod]
        public void ConvertToHttpsTest()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.MySelect(url => url.Replace("http:", "https:") + ":80");

            var expected = new List<string>()
            {
                "https://tw.yahoo.com:80",
                "https://facebook.com:80",
                "https://twitter.com:80",
                "https://github.com:80",
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void UrlLengthTest()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.MySelect(url=>url.Length);

            var expected = new List<int>()
            {
                19, 20, 19, 17
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void Url_tw_append_91_Test()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = YourOwnLinq.appendPort(urls, url => url.Contains("tw") ? url + ":91" : url);

            var expected = new List<string>()
            {
                "http://tw.yahoo.com:91",
                "https://facebook.com",
                "https://twitter.com:91",
                "http://github.com"
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void FindEmployeeTest()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.MyOwnWhere(e => e.Role == RoleType.Engineer).
                MySelect(e => e.MonthSalary);

            foreach (var item in actual)
            {
                Console.WriteLine(item.ToString());
            }

            var expected = new List<int>()
            {
                100, 140, 280, 120, 250,
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void MyTakeTest()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MyTake(3);

            var expected = new List<Product>()
            {
                new Product{Id=1, Cost=11, Price=110, Supplier="Odd-e" },
                new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [Ignore]
        [TestMethod]
        public void Select_with_index()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.MyTake(3).MySelect( (e, index) => $"{index+1}-{e.Name}" );

            var expected = new List<string>()
            {
                "1-Jeo",
                "2-Tom",
                "3-Kevin"
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void MySkipTest()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.MySkip(6);

            var expected = new List<Employee>()
            {
                new Employee{Name="Frank", Role=RoleType.Engineer, MonthSalary=120, Age=16, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void MySkipWhileTest()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MySkipWhile( p=>p.Price > 300, 4);

            var expected = new List<Product>()
            {
                new Product{Id=1, Cost=11, Price=110, Supplier="Odd-e" },
                new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=7, Cost=71, Price=710, Supplier="Yahoo" },
                new Product{Id=8, Cost=18, Price=780, Supplier="Yahoo" },
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void MyTakeWhileTest()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.MyTakeWhile(e => e.Age > 30);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
            };
            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        public void MySumTest()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MySum(p => p.Price);

            var expected = 3650;
            Assert.AreEqual(expected, actual);
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

        private static IEnumerable<Product> MyOwnForeach(IEnumerable<Product> products, Func<Product, bool> predicate)
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

        public static IEnumerable<TResult> MySelect<TSouce, TResult>(this IEnumerable<TSouce> source, Func<TSouce, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TResult> MySelect<TSouce, TResult>(this IEnumerable<TSouce> source, Func<TSouce, int, TResult> selector)
        {
            var index = 0;
            foreach (var item in source)
            {
                yield return selector(item, index);
                index++;
            }
        }

        public static IEnumerable<string> appendPort(IEnumerable<string> urls, Func<string, string> selector)
        {
            foreach (var url in urls)
            {
                yield return selector(url);
            }
        }

        public static IEnumerable<TSource> MyTake<TSource>(this IEnumerable<TSource> source, int top)
        {
            var enumerator = source.GetEnumerator();

            while (top > 0 && enumerator.MoveNext())
            {
                yield return enumerator.Current;
                top--;
            }
        }

        public static IEnumerable<TSource> MySkip<TSource>(this IEnumerable<TSource> source, int count)
        {
            var index = 0;
            var enumerator = source.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                if (index >= count)
                {
                    yield return enumerator.Current;
                }

                index++;
            }
        }

        public static IEnumerable<TSouce> MySkipWhile<TSouce>(this IEnumerable<TSouce> source, Func<TSouce, bool> predicate, int count)
        {
            var enumerator = source.GetEnumerator();
            var counter = 0;
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (!predicate(item))
                {
                    yield return item;
                }
                else
                {
                    if (counter >= count)
                    {
                        yield return item;
                    }
                    counter++;
                }
            }
        }

        public static IEnumerable<TSource> MyTakeWhile<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    yield return enumerator.Current;
                }
                else
                {
                    yield break;
                }
            }
        }

        public static int MySum<TSource>(this IEnumerable<TSource> sources, Func<TSource, int> selector)
        {
            var sum = 0;
            foreach (var item in sources)
            {
                sum += selector(item);
            }

            return sum;
        }
    }
}
