using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Repositories;
using Dashboard.API.DataServices.Services;
using Dashboard.Demo.API.Extensions;
using Dashboard.Demo.API.Models.Request;
using Dashboard.Infrastructure.dbContext;
using Dashboard.SharedKernel.Repository;
using Dashboard.SharedKernel.Specifications;
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductService, ProductDataService>();
var app = builder.Build();
app.MapGet("/products", async Task<Results<BadRequest, Ok<PagedResults<ProductDTO>>>> (ProductQueryRequest? query, [FromServices]IProductService productDataService) =>
{
    System.Diagnostics.Debug.WriteLine(query);
    if (query?.Paging == null || string.IsNullOrEmpty(query?.Category))
        return TypedResults.BadRequest();
   
    var results = await productDataService.QueryProductsAsync(query);
        
    return TypedResults.Ok(results);
});

app.MapGet("/products/{id}", async Task<Results<NotFound, Ok<ProductDTO>>> (int id, IProductService productDataService) =>
{
    var productDto = await productDataService.GetByIdAsync(id);
    if (productDto == null)
        return TypedResults.NotFound();

    return TypedResults.Ok(productDto);

});

app.RegisterUsersEndpoints();

app.Run();
