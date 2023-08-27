namespace Dashboard.SharedKernel.Specifications;

public class FilterPredicate
{
    public string? FilterOnFieldName { get; set; }
    public object? FilterByValue { get; set; }
    public ComparisonOperation? ComparisonOperation { get; set; }
}

public enum ComparisonOperator
{
    Equals,
    NotEquals,
    Contains,
    GreaterThan,
    LessThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    In,
    NotIn,
    IsNull,
    NotEqualNull
}

public class ComparisonOperation
{
    private readonly ComparisonOperator _comparisonOperator;
    public ComparisonOperation(ComparisonOperator comparisonOperator)
    {
        _comparisonOperator = comparisonOperator;
    }

    public ComparisonOperator ComparisonOperator => _comparisonOperator;

    public static ComparisonOperation Eq => new ComparisonOperation(ComparisonOperator.Equals);
    public static ComparisonOperation Ne => new ComparisonOperation(ComparisonOperator.NotEquals);
    public static ComparisonOperation Gn => new ComparisonOperation(ComparisonOperator.GreaterThan);
    public static ComparisonOperation Ln => new ComparisonOperation(ComparisonOperator.LessThan);
    public static ComparisonOperation Gne => new ComparisonOperation(ComparisonOperator.GreaterThanOrEqual);
    public static ComparisonOperation Lne => new ComparisonOperation(ComparisonOperator.LessThanOrEqual);
    public static ComparisonOperation Contains => new ComparisonOperation(ComparisonOperator.Contains);
    public static ComparisonOperation In => new ComparisonOperation(ComparisonOperator.In);
    public static ComparisonOperation NotIn => new ComparisonOperation(ComparisonOperator.NotIn);
    public static ComparisonOperation IsNull => new ComparisonOperation(ComparisonOperator.IsNull);
    public static ComparisonOperation NotNull => new ComparisonOperation(ComparisonOperator.NotEqualNull);
    

}