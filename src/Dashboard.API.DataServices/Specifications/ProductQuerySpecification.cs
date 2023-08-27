using Dashboard.SharedKernel.Specifications;

namespace Dashboard.API.DataServices.Specifications;

public class ProductQuerySpecification
{
    public string? Category { get; set; }
    public string? SearchTerm { get; set; }
    public required PagingInfo Paging { get; set; }
}