using Dashboard.API.DataServices.DTOs;
using Dashboard.Core.Models;

namespace Dashboard.API.DataServices.Extensions;

public static class ProductEntityExtensions
{
    public static ProductDTO ToDTO(this Product product)
    {
        return new ProductDTO()
        {
            ProductId = product.Id,
            ProductName = product.ProductName,
            ProductDescription = product.Description,
            ProductCategoryId = product.CategoryId,
            ProductCategory = product.Category?.Name,
            Price = product.Price!.Value
        };
    }
}