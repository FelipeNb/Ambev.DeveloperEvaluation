using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Base;

public abstract class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;

    protected RepositoryBase(DbContext context)
    {
        _context = context;
    }

    protected IQueryable<T> ApplyOrdering(IQueryable<T> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query;

        var tokens = order.Split(',');
        IOrderedQueryable<T>? orderedQuery = null;

        for (int i = 0; i < tokens.Length; i++)
        {
            var parts = tokens[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            var propertyName = parts[0];
            var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(param, propertyName);
            var lambda = Expression.Lambda(property, param);

            string methodName = i == 0
                ? (descending ? "OrderByDescending" : "OrderBy")
                : (descending ? "ThenByDescending" : "ThenBy");

            var resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                orderedQuery == null ? query.Expression : orderedQuery.Expression,
                Expression.Quote(lambda)
            );

            orderedQuery = (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(resultExp);
        }

        return orderedQuery ?? query;
    }

    public virtual async Task<PagedResult<T>> GetAllAsync(int page, int size, string? order, CancellationToken ct)
    {
        var query = _context.Set<T>().AsQueryable();
        query = ApplyOrdering(query, order);

        var totalItems = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * size).Take(size).ToListAsync(ct);

        return new PagedResult<T>
        {
            Items = items,
            CurrentPage = page,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)size)
        };
    }
}
