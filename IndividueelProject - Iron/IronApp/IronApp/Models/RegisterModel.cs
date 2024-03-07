using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace IronApp.Models;

public class RegisterModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    
    
    [Required]
    public JSType.Date DateOfBirth { get; set; }
    
    
    [Required]
    public decimal Weight { get; set; }
}