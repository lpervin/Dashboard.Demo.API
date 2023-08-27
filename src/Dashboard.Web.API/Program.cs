using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Repositories;
using Dashboard.API.DataServices.Services;
using Dashboard.Demo.API.Models.Request;
using Dashboard.Infrastructure.dbContext;
using Dashboard.SharedKernel.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration["ConnectionStrings:eCommerceDb"]!.ToString();
builder.Services.AddDbContext<ProductsDbContext>(opt =>
{
    opt.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IProductReadOnlyRepository, ProductsReadOnlyRepository>();
builder.Services.AddTransient<IProductService, ProductDataService>();
var app = builder.Build();
app.MapGet("/products", async Task<Results<BadRequest, Ok<PagedResults<ProductDTO>>>> (ProductQueryRequest? query, [FromServices]IProductService productDataService) =>
{
    System.Diagnostics.Debug.WriteLine(query);
    if (query?.Paging == null)
        return TypedResults.BadRequest();
   
    var results = await productDataService.QueryProductsAsync(query);
   
    return TypedResults.Ok(results);
});

app.Run();
