using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            DashboardFilter req = new DashboardFilter();
            req.PageSize = 10;
            req.PageStart = 1;
            req.SortColumn = "ABC";
            req.SortOrder = "ASC";
            req.Filters = new Dictionary<string, DashboardFilterValue>();

            req.Filters.Add("JobName", new DashboardFilterValue());
            req.Filters.Add("AgencyID", new DashboardFilterValue());
            req.Filters.Add("payType", new DashboardFilterValue());
            req.Filters.Add("RunDate", new DashboardFilterValue());

            req.Filters["JobName"].Values = new List<string>();
            req.Filters["JobName"].Operator = "contains";
            req.Filters["JobName"].Values.Add("VA161537");

            req.Filters["AgencyID"].Values = new List<string>();
            req.Filters["AgencyID"].Operator = "is equal to";
            req.Filters["AgencyID"].Values.Add("450280000");

            req.Filters["RunDate"].Values = new List<string>();
            req.Filters["RunDate"].Operator = null;
            req.Filters["RunDate"].Values.Add("06/01/2016");
            req.Filters["RunDate"].Values.Add("07/01/2016");

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Newtonsoft.Json.Formatting.Indented                
            };
            var data = req.Filters.Find(p => p.Key == "JobName");

            List<Sample1> list = new List<Sample1>();
            list.Add(new Sample1 { Address = "Address1", Age = 25, City = "ABC1", FName = "FName1", ID = 1, LName = "LName1", Zip = "Zip1" });
            list.Add(new Sample1 { Address = "Address2", Age = 25, City = "ABC2", FName = "FName2", ID = 1, LName = "LName2", Zip = "Zip2" });
            list.Add(new Sample1 { Address = "Address3", Age = 25, City = "ABC3", FName = "FName3", ID = 1, LName = "LName3", Zip = "Zip3" });
            list.Add(new Sample1 { Address = "Address4", Age = 25, City = "ABC4", FName = "FName4", ID = 1, LName = "LName4", Zip = "Zip4" });
            list.Add(new Sample1 { Address = "Address5", Age = 25, City = "ABC5", FName = "FName5", ID = 2, LName = "LName5", Zip = "Zip5" });
            list.Add(new Sample1 { Address = "Address6", Age = 25, City = "ABC6", FName = "FName6", ID = 2, LName = "LName6", Zip = "Zip6" });
            list.Add(new Sample1 { Address = "Address7", Age = 25, City = "ABC7", FName = "FName7", ID = 2, LName = "LName7", Zip = "Zip7" });
            list.Add(new Sample1 { Address = "Address8", Age = 25, City = "ABC8", FName = "FName8", ID = 2, LName = "LName8", Zip = "Zip8" });

            var data1 = list.Search(x => x.ID == 2).Get(x=>x.Age).ToList();
            var item = list[0];
        }
    }
        
    public class DashboardFilter
    {
        public Dictionary<string, DashboardFilterValue> Filters { get; set; }
        public int PageStart { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        
    }

    public static class Extensions
    {
        public static IQueryable<T> Find<T>(this Dictionary<string, T> dict, string find)
        {
            var data = dict.Where(x => x.Key == find).Select(x=>x.Value).AsQueryable();
            return data;
        }

        public static IQueryable<T> Find<T>(this Dictionary<string, T> dict, Func<KeyValuePair<string, T>, bool> func)
        {
            var data = dict.Where(func).Select(x => x.Value).AsQueryable();
            return data;
        }

        public static IEnumerable<T> Search<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                    yield return item;
            }            
        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        public static IEnumerable<TOut> Get<T, TOut> (this IEnumerable<T> list, Expression<Func<T, TOut>> func)
        {
            var propname = (func.Body as MemberExpression).Member.Name;
            foreach (var item in list)
            {
                yield return (TOut)item.GetType().GetProperty(propname).GetValue(item);
            }            
        }
    }

    public class GenericCompare<T> : IEqualityComparer<T> where T : class
    {
        private Func<T, object> _expr = null;
        public GenericCompare(Func<T, object> expr)
        {
            _expr = expr;
        }
        public bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

    public class DashboardFilterValue
    {
        public string Operator { get; set; }
        public List<string> Values { get; set; }
    }



}
