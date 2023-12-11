using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Extensions;
using Dashboard.API.DataServices.Repositories;
using Dashboard.API.DataServices.Specifications;
using Dashboard.Infrastructure.dbContext;
using Dashboard.SharedKernel.Repository;

namespace Dashboard.API.DataServices.Services;

public class ProductDataService : IProductService
{
    private readonly IProductReadOnlyRepository _productRepository;
    public ProductDataService(IProductReadOnlyRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<PagedResults<ProductDTO>> QueryProductsAsync(ProductQuerySpecification productRequest)
    {
        
        var pagedResults = await _productRepository.QueryProductsAsync(productRequest);
        return new PagedResults<ProductDTO>()
        {
            Results = pagedResults.Results?.Select(p => p.ToDTO()).ToList(),
            PageSize = pagedResults.PageSize,
            PagesCount = pagedResults.PagesCount,
            CurrentPageNumber = pagedResults.CurrentPageNumber,
            TotalRecordsCount = pagedResults.TotalRecordsCount,
            CurrentPageRecordsCount = pagedResults.CurrentPageNumber
        };
    }

    public async Task<ProductDTO?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return null;
        
        return product.ToDTO();
    }

    public async Task<IReadOnlyList<ProductCategoryDTO>> GetCategoriesAsync(string? searchTerm)
    {
        var results = await _productRepository.GetProductCategoriesAsync(searchTerm);
        return results.Select(p => p.ToDTO()).ToList();
    }
}