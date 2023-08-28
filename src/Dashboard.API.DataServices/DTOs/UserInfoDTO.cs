using Dashboard.Core.Models;
using MongoDB.Bson;

namespace Dashboard.API.DataServices.DTOs;

public class UserInfoDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public User ToEntity()
    {
        return new User()
        {
            Name = this.Name,
            Age = this.Age,
            Email = this.Email,
            Id = string.IsNullOrEmpty(this.Id) ? ObjectId.Empty : new ObjectId(this.Id)
        };
    }
}
