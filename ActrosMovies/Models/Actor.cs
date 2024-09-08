#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ActrosMovies.Models;


public class Actor
{
    [Key]
    [Required]
    public int ActorId { get; set; }

    [Required]
    [MinLength(2)]
    public string Name { get; set; }

    [Required]
    [MinLength(3)]
    public string Image { get; set; }

    // Navigation propreties
    public List<Association> AllMovies { get; set; } = new List<Association>();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}