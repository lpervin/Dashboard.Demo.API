using System.Linq.Expressions;
using Dashboard.API.DataServices.Extensions;
using Dashboard.API.DataServices.Specifications;
using Dashboard.Core.Models;
using Dashboard.Infrastructure.dbContext;
using Dashboard.SharedKernel.Repository;
using Dashboard.SharedKernel.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.API.DataServices.Repositories;

public class ProductsReadOnlyRepository : IProductReadOnlyRepository
{
    private readonly ProductsDbContext _productsDbContext;
    public ProductsReadOnlyRepository(ProductsDbContext productsDbContext)
    {
        _productsDbContext = productsDbContext;
    }
    
    public async Task<Product?> GetByIdAsync(object? id)
    {
        return await _productsDbContext.Products.FindAsync(id);
    }
    
    public async Task<PagedResults<Product>> QueryAsync(QueryFilterSpecification query)
    {
        var productQuery = query.ToProductQuery();
        return await QueryProductsAsync(productQuery);
    }
    
    public async Task<PagedResults<Product>> QueryProductsAsync(ProductQuerySpecification productQuery)
    {
        var queryable = _productsDbContext.Products.Include(p => p.Category).AsQueryable();
        queryable = queryable.ApplyFiltering(productQuery);
        var totalRecordsCount = await queryable.CountAsync();

        queryable = queryable.ApplyOrdering(productQuery.Paging.OrderBy, new Dictionary<string, Expression<Func<Product, object>>>()
        {
            ["ProductName"] = p => p.ProductName,
            ["Description"] = p => p.Description,
            ["Price"] = p => p.Price
        });
        queryable = queryable.ApplyPaging(productQuery.Paging);
        
        var results = await queryable.ToListAsync();
        return new PagedResults<Product>()
        {
            Results = results,
            CurrentPageNumber = productQuery.Paging.CurrentPageNumber,
            PageSize = productQuery.Paging.PageSize,
            TotalRecordsCount = totalRecordsCount,
            PagesCount = (long)Math.Ceiling(totalRecordsCount/(double)productQuery.Paging.PageSize),
            CurrentPageRecordsCount = results.Count
        };

    }

    public async Task<IReadOnlyList<ProductCategory>> GetProductCategoriesAsync(string? searchTerm)
    {
       var results = _productsDbContext.ProductCategories.ApplyFiltering(searchTerm).OrderBy(p => p.Name);
       return await results.ToListAsync();
    }
}