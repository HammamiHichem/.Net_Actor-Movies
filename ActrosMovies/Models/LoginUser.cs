#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ActrosMovies.Models;
public class LoginUser
{
    [Required]
    [EmailAddress]
    public string  LogEmail { get; set;}
    [Required]
    [MinLength(8, ErrorMessage ="Password is required")]
    [DataType(DataType.Password)]
    public string  LogPassword { get; set;}
}
