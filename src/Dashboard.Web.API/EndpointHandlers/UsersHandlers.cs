using Dashboard.API.DataServices.DTOs;
using Dashboard.API.DataServices.Repositories;
using Dashboard.Demo.API.Models.Request;
using Dashboard.SharedKernel.Repository;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Dashboard.Demo.API.EndpointHandlers;

public static class UsersHandlers
{
    public static async Task<Results<BadRequest, Ok<PagedResults<UserInfoDTO>>>> ListUsersAsync(PagingRequest paging, IUserRepository userRepository)
    {
        if (paging.CurrentPageNumber == 0)
            return TypedResults.BadRequest();
        
        var users = await userRepository.ListUsersAsync(paging);
        return TypedResults.Ok(users);
    }

    public static async Task<Results<NotFound, NoContent>> UpdateUserAsync(string userId, UserInfoDTO userToUpdate, IUserRepository userRepository)
    {
        try
        {
            await userRepository.UpdateUserAsync(userId, userToUpdate);
            return TypedResults.NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound();
        }
    }

    public static async Task<Ok<UserInfoDTO>> AddUserAsync(UserInfoDTO userToAdd, IUserRepository userRepository)
    {
        var newUser = await userRepository.AddUserAsync(userToAdd);
        return TypedResults.Ok(newUser);
    }

    public static async Task<Results<NotFound, NoContent>> DeleteUserAsync(string userId, IUserRepository userRepository)
    {
        try
        {
            
            await userRepository.DeleteUserAsync(userId);
            return TypedResults.NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound();
        }
    }
}

