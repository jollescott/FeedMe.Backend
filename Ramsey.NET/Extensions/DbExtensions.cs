
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ramsey.NET.Extensions
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();

            try
            {
                return !exists ? dbSet.Add(entity).Entity : dbSet.Single(predicate ?? throw new ArgumentNullException(nameof(predicate)));
            }
            catch(InvalidOperationException)
            {
                throw new Exception();
            }
        }

        public static double DoubleCount<T>(this IEnumerable<T> ts)
        {
            var count = ts.Count();
            return count;
        }
    }
}