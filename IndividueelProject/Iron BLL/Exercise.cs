using Microsoft.Data.SqlClient;

namespace Iron_Domain;

public class Exercise
{
    public int Id { get;}
    public int? UserId { get; }
    public string Name { get;}
    public string Description { get;}
    public string Logo { get;}
    
    public Exercise(int id, int userId, string name, string description, string logo)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Description = description;
        Logo = logo;
    }
    
    public Exercise(int id, string name, string description, string logo)
    {
        Id = id;
        Name = name;
        Description = description;
        Logo = logo;
    }
}