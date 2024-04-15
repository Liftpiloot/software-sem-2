namespace IronApp.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public decimal Weight { get; set; }
    public int Age { get; set; }
}