namespace Dashboard.SharedKernel.Specifications;

public class PagingInfo
{
    public PagingInfo() : this(5,1, new OrderByInfo())
    {
        
    }
    public PagingInfo(int pageSize, int currentPageNumber, OrderByInfo orderBy)
    {
        OrderBy = orderBy;
        PageSize = pageSize;
        CurrentPageNumber = currentPageNumber;
    }

    public int CurrentPageNumber { get; set; }
    public int PageSize { get; set; }
    public OrderByInfo OrderBy { get; set; }
       
}

public class OrderByInfo
{
    public OrderByInfo():this("id", SortDirection.Asc)
    {
        
    }

    public OrderByInfo(string orderByFieldName, SortDirection sort)
    {
        this.OrderByFieldName = orderByFieldName;
        this.Sort = sort;
    }
    public string OrderByFieldName { get; set; }
    public SortDirection Sort { get; set; }
}

public enum SortDirection
{
    Asc,
    Desc
}