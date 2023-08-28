using MongoDB.Bson;

namespace Dashboard.Core.Models;

public class User
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}