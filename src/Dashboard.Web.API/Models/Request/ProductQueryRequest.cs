using System.Reflection;
using Dashboard.API.DataServices.Specifications;
using Dashboard.Demo.API.Extensions;
using Dashboard.SharedKernel.Specifications;
using Microsoft.Extensions.Primitives;

namespace Dashboard.Demo.API.Models.Request;
public class ProductQueryRequest : ProductQuerySpecification
{

    public static ValueTask<ProductQueryRequest> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string categoryKey = "category";
        const string searchTermKey = "searchTerm";
        const string currentPageKey = "pageNumber";
        const string pageSizeKey = "pageSize";
        const string sortByKey = "sortBy";
        const string sortDirectionKey = "sortDir";
        
        int currentPage = context.Request.Query[currentPageKey].TryParseInt(1);
        int pageSize = context.Request.Query[pageSizeKey].TryParseInt(5);
   
        string? sortBy = context.Request.Query[sortByKey];
        Enum.TryParse<SortDirection>(context.Request.Query[sortDirectionKey],
            ignoreCase: true, out var sortDirection);
        
        var result = new ProductQueryRequest()
        {
            Category = context.Request.Query[categoryKey],
            SearchTerm = context.Request.Query[searchTermKey],
            Paging = new  PagingInfo(pageSize, currentPage, new OrderByInfo(sortBy! ?? "Id", sortDirection) )
        };
      
         return ValueTask.FromResult<ProductQueryRequest>(result);
    }



    /*private static int GetIntFromQuery(HttpContext httpContext, string key, int? defaultValue)
    {
        bool canParse = int.TryParse(httpContext.Request.Query[key], out var parsed);
        return canParse ? parsed : (defaultValue ?? 0);
    }*/
}