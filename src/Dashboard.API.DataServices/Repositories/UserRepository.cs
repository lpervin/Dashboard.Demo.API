using System.Linq.Expressions;
using Bogus;
using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Extensions;
using Dashboard.Core.Models;
using Dashboard.SharedKernel.Repository;
using Dashboard.SharedKernel.Specifications;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SortDirection = Dashboard.SharedKernel.Specifications.SortDirection;

namespace Dashboard.API.DataServices.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoDatabase _mongoDatabase;
    private IMongoCollection<User> _usersMongoCollection;

    public UserRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UserManagementDb");
        var mongoClient = new MongoClient(connectionString);
        _mongoDatabase = mongoClient.GetDatabase("AppManagement");
        _usersMongoCollection = _mongoDatabase.GetCollection<User>("Users");
        SeedTestData();
    }
    public async Task<PagedResults<UserInfoDTO>> ListUsersAsync(PagingInfo paging)
    {
        var countFacet = AggregateFacet.Create("count",
            PipelineDefinition<User, AggregateCountResult>.Create(
                new[] {PipelineStageDefinitionBuilder.Count<User>()}
            ));

        var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<User,User>.Create(new []
                {
                    PipelineStageDefinitionBuilder.Sort(BuildSortingExp(paging.OrderBy.OrderByFieldName, paging.OrderBy.Sort)),
                    PipelineStageDefinitionBuilder.Skip<User>((paging.CurrentPageNumber - 1) * paging.PageSize),
                    PipelineStageDefinitionBuilder.Limit<User>(paging.PageSize)}
            ));
        var filter = Builders<User>.Filter.Empty;
        
      
        var collation = new Collation("en", numericOrdering: true);
        var options = new AggregateOptions()
        {
            Collation = collation
        };
        var aggregation = await _usersMongoCollection.Aggregate(options)
            .Match(filter)
            .Facet(countFacet, dataFacet)
           
            .ToListAsync();

        var count = aggregation.First()
            .Facets.First(x => x.Name == "count")
            .Output<AggregateCountResult>()?.FirstOrDefault()?.Count ?? 0;
        var totalPages = (double) count /  (double) paging.PageSize;


        var pageData = aggregation.First()
            .Facets.First(x => x.Name == "data")
            .Output<User>();

        return new PagedResults<UserInfoDTO>()
        {
            TotalRecordsCount = count,
            CurrentPageNumber = paging.CurrentPageNumber, 
            CurrentPageRecordsCount = pageData.Count,
            PagesCount = (int) Math.Ceiling(totalPages),
            PageSize = paging.PageSize,
            Results = pageData.Select(p => p.ToDTO()).ToList()
        };
    }

    public async Task UpdateUserAsync(string userId, UserInfoDTO userToUpdate)
    {
        var objUserId =  ObjectId.Parse(userId);
        var userQuery = Builders<User>.Filter.Eq("_id", objUserId);
        User? userEntity = (await _usersMongoCollection.FindAsync(userQuery)).FirstOrDefault();
        if (userEntity == null)
            throw new KeyNotFoundException(userId);

        userEntity.Name = userToUpdate.Name;
        userEntity.Email = userToUpdate.Email;
        userEntity.Age = userToUpdate.Age;

        var result = await _usersMongoCollection.ReplaceOneAsync(userQuery, userEntity);
        if (result.ModifiedCount < 1)
            throw new ApplicationException("Updated Failed");
    }

    public async Task<UserInfoDTO> AddUserAsync(UserInfoDTO userToAdd)
    {
        var userDocument = userToAdd.ToEntity();
        await _usersMongoCollection.InsertOneAsync(userDocument);
        return userDocument.ToDTO();
    }

    public async Task DeleteUserAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(userId));
        var result = await  _usersMongoCollection.DeleteOneAsync(filter);
        if (result.DeletedCount != 1)
            throw new KeyNotFoundException(userId);
    }

    public void SeedTestData()
    {
        var usersCount =  _usersMongoCollection.CountDocuments(FilterDefinition<User>.Empty);
        if (usersCount>2)
            return;

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Age, f => f.Random.Number(18, 100));

        // Generate 1000 random users
        var users = userFaker.Generate(1000);
        _usersMongoCollection.InsertMany(users);
    }
    
    private SortDefinition<User> BuildSortingExp(string? pagingSortByFieldName, SortDirection pagingOrder)
    {
        Expression<Func<User, object>>? orderExpr = null;// (x => x.Id);

        switch (pagingSortByFieldName)
        {
            case "Id":
                orderExpr = (x => x.Id);
                break;
            case "Name":
                orderExpr = (x => x.Name);
                break;
            case "Age":
                orderExpr = (x => x.Age);
                break;
            case "Email":
                orderExpr = (x => x.Email);
                break;
            default:
                throw new ArgumentException($"Never heard of {pagingSortByFieldName}");
        }
        switch (pagingOrder)
        {
            case SortDirection.Asc:
                return Builders<User>.Sort.Ascending(orderExpr);
            case SortDirection.Desc:
                return Builders<User>.Sort.Descending(orderExpr);
            default:
                return Builders<User>.Sort.Ascending(x => x.Id);
        }
    }
}