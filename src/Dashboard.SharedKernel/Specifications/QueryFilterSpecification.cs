namespace Dashboard.SharedKernel.Specifications;

public class QueryFilterSpecification
{
    public IReadOnlyList<FilterPredicate> Filters { get; set; }
    public PagingInfo  Paging { get; set; }
}

