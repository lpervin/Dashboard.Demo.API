using Dashboard.API.DataServices.Specifications;
using Dashboard.Core.Models;
using Dashboard.SharedKernel.Repository;

namespace Dashboard.API.DataServices.Repositories;

public interface IProductReadOnlyRepository : IGenericReadOnlyRepository<Product>
{
    Task<PagedResults<Product>> QueryProductsAsync(ProductQuerySpecification productQuery);
}