using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace IronApp.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 10)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }


    [Required(ErrorMessage = "Date of birth is required")]
    public string? DateOfBirth { get; set; }


    [Required(ErrorMessage = "Weight is required")]
    public decimal Weight { get; set; }
}