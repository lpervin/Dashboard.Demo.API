using Dashboard.API.DataServices.DTOs;
using Dashboard.SharedKernel.Repository;
using Dashboard.SharedKernel.Specifications;

namespace Dashboard.API.DataServices.Repositories;

public interface IUserRepository
{
    Task<PagedResults<UserInfoDTO>> ListUsersAsync(PagingInfo paging);

    Task UpdateUserAsync(string userId, UserInfoDTO userToUpdate);

    Task<UserInfoDTO> AddUserAsync(UserInfoDTO userToAdd);

    Task DeleteUserAsync(string userId);

    void SeedTestData();
}