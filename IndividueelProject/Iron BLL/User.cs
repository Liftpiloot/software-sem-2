using System.Security.Cryptography;
using System.Text;

namespace IronDomain;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal Weight { get; set; }
    

    public User(int id, string userName, string email, string passwordHash, DateTime dateOfBirth, decimal weight)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        DateOfBirth = dateOfBirth;
        Weight = weight;
    }
    
    public void HashPassword()
    {
        // Create sha256 hash
        Byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(PasswordHash));
        StringBuilder builder = new StringBuilder();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        PasswordHash = builder.ToString();
    }
}