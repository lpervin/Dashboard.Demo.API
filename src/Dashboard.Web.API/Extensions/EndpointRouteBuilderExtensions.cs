using Dashboard.API.DataServices.DTOs;
using Dashboard.Demo.API.EndpointHandlers;

namespace Dashboard.Demo.API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterUsersEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var userEndpoints = endpointRouteBuilder.MapGroup("/users");
        userEndpoints.MapGet("", UsersHandlers.ListUsersAsync);
        userEndpoints.MapPost("", UsersHandlers.AddUserAsync).Accepts<UserInfoDTO>("application/json");
        userEndpoints.MapPut("/{userId}", UsersHandlers.UpdateUserAsync).Accepts<UserInfoDTO>("application/json");
        userEndpoints.MapDelete("/{userId}", UsersHandlers.DeleteUserAsync);

    }
}