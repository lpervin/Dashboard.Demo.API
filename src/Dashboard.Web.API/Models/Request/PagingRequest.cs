using System.Reflection;
using Dashboard.Demo.API.Extensions;
using Dashboard.SharedKernel.Specifications;

namespace Dashboard.Demo.API.Models.Request;

public class PagingRequest : PagingInfo
{
    public PagingRequest(int pageSize, int currentPageNumber, OrderByInfo orderBy) : base(pageSize, currentPageNumber, orderBy)
    {
        
    }
    public static ValueTask<PagingRequest> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string currentPageKey = "pageNumber";
        const string pageSizeKey = "pageSize";
        const string sortByKey = "sortBy";
        const string sortDirectionKey = "sortDir";
        
        int currentPage = context.Request.Query[currentPageKey].TryParseInt(1);
        int pageSize = context.Request.Query[pageSizeKey].TryParseInt(5);
        string? sortBy = context.Request.Query[sortByKey];
        Enum.TryParse<SortDirection>(context.Request.Query[sortDirectionKey],
            ignoreCase: true, out var sortDirection);
        
        
        var pagingRequest = new PagingRequest(pageSize, currentPage, new OrderByInfo(sortBy ?? "id", sortDirection));
        return ValueTask.FromResult<PagingRequest>(pagingRequest);
    }
}