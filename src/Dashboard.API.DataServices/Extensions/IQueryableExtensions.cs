using System.Linq.Expressions;
using Dashboard.API.DataServices.Specifications;
using Dashboard.Core.Models;
using Dashboard.SharedKernel.Specifications;

namespace Dashboard.API.DataServices.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<Product> ApplyFiltering(this IQueryable<Product> query, ProductQuerySpecification productQuery)
    {
        if (!string.IsNullOrEmpty(productQuery.Category))
            query = query.Where(p => p.Category.Name == productQuery.Category);

        if (!string.IsNullOrEmpty(productQuery.SearchTerm))
            query = query.Where(p =>
                p.ProductName.Contains(productQuery.SearchTerm) || p.Description.Contains(productQuery.SearchTerm));
        
        return query;
    }

    public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, OrderByInfo? orderBy, Dictionary<string, Expression<Func<T, object>>> columnsMap )
    {
        if (orderBy == null || !columnsMap.ContainsKey(orderBy.OrderByFieldName))
            return query;

        switch (orderBy.Sort)
        {
            case SortDirection.Asc:
                return query.OrderBy(columnsMap[orderBy.OrderByFieldName]);
            case SortDirection.Desc:
                return query.OrderByDescending(columnsMap[orderBy.OrderByFieldName]);
            default:
                return query;
        }
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingInfo paging)
    {
        if (paging.PageSize <= 0)
            paging.PageSize = 10;
        
        if (paging.CurrentPageNumber <= 0)
            paging.CurrentPageNumber = 1;

        return query.Skip((paging.CurrentPageNumber - 1) * paging.PageSize).Take(paging.PageSize);
    }
}