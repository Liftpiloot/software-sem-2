namespace Iron_DAL.DTO;

public record UserDto
{
    public int Id { get; set; }
    public string UserName { get; init; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal Weight { get; set; }
}