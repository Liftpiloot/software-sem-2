using Microsoft.Data.SqlClient;

namespace Iron_Domain;

public class Exercise
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
}