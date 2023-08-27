using Dashboard.SharedKernel.Specifications;

namespace Dashboard.API.DataServices.Specifications;

public static class QueryFilterSpecificationsExtensions
{
    public static ProductQuerySpecification ToProductQuery(this QueryFilterSpecification queryFilter)
    {
        var categoryPredicate = queryFilter.Filters.FirstOrDefault(q => q.FilterOnFieldName == "Category");
        var searchTermPredicate = queryFilter.Filters.FirstOrDefault(q => q.FilterOnFieldName == "SearchTerm");

        return new ProductQuerySpecification()
        {
            Category = categoryPredicate?.FilterByValue?.ToString(),
            SearchTerm = searchTermPredicate?.FilterByValue?.ToString(),
            Paging = queryFilter.Paging
        };
    }
}