#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ActrosMovies.Models;

public class Association
{
    [Key]
    public int AssociationId { get; set; }
    [Required]
    public int ActorId { get; set; }
    [Required]
    public int MovieId { get; set; }
     // Navigation propreties
     public Actor? Actor { get; set; }
     public Movie? Movie{ get; set; }
}