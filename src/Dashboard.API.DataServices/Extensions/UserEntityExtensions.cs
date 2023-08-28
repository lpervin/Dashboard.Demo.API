using Dashboard.API.DataServices.DTOs;
using Dashboard.Core.Models;

namespace Dashboard.API.DataServices.Extensions;

public static class UserEntityExtensions
{
    public static UserInfoDTO ToDTO(this User userEntity)
    {
        return new UserInfoDTO()
        {
            Id = userEntity.Id.ToString(),
            Email = userEntity.Email,
            Name = userEntity.Name,
            Age = userEntity.Age
        };
    }
}