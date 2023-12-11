using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Specifications;
using Dashboard.SharedKernel.Repository;

namespace Dashboard.API.DataServices.Services;

public interface IProductService
{
   Task<PagedResults<ProductDTO>> QueryProductsAsync(ProductQuerySpecification productRequest);
   Task<ProductDTO?> GetByIdAsync(int id);

   Task<IReadOnlyList<ProductCategoryDTO>> GetCategoriesAsync(string? searchTerm);
}