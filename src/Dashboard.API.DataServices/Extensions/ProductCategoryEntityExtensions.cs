using Dashboard.API.DataServices.DTOs;
using Dashboard.Core.Models;

namespace Dashboard.API.DataServices.Extensions;

public static class ProductCategoryEntityExtensions
{
    public static ProductCategoryDTO ToDTO(this ProductCategory category)
    {
        return new ProductCategoryDTO()
        {
            Id = category.Id,
            Category = category.Name!
            
        };
    }
}