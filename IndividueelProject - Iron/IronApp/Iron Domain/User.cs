namespace Iron_Domain;

public class User
{
    private string _db = "Server=localhost\\SQLEXPRESS;Database=iron;Trusted_Connection=True;Encrypt=False;";

    public int? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? DateOfBirth { get; set; }
    public decimal Weight { get; set; }

    public User()
    {
    }

    public User(string? userName, string? email, string? passwordHash, string? dateOfBirth, decimal weight)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }

    public User(int id, string? userName, string? email, string? passwordHash, string? dateOfBirth, decimal weight)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }






    

    
}