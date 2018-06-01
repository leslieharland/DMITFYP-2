using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;

namespace WebApp.Models
{
    public class Entity
    {
        public int Id { get; set; }
    }
    public static class Db
    {
        public static IList<StudentContact> StudentContacts = new List<StudentContact>();

        public static int Gid = 75;

        public static object Set<T>()
        {
            if (typeof(T) == typeof(StudentContact)) return StudentContacts;
            return null;
        }

        public static IDictionary<Type, IList<object>> Tables = new Dictionary<Type, IList<object>>
            {
                { typeof(StudentContact), StudentContacts.Cast<object>().ToList() }
            };

        public static T Insert<T>(T o) where T : Entity
        {
            o.Id = ++Gid;
            ((IList<T>)Set<T>()).Add(o);
            return o;
        }

        public static void Update<T>(T o) where T : Entity
        {
            var t = Get<T>(o.Id);

        }

        public static T Get<T>(int? id) where T : Entity
        {
            return ((IList<T>)Set<T>()).SingleOrDefault(o => o.Id == id);
        }

        public static void Delete<T>(int id) where T : Entity
        {
            ((IList<T>)Set<T>()).Remove(Get<T>(id));
        }

        static Db()
        {
            //Db Seed
        }
    }
}
