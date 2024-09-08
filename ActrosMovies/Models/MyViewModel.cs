#pragma warning disable CS8618
namespace ActrosMovies.Models;

public class MyViewModel
{
    public Actor? Actor { get; set; }
    public List<Actor>? AllActors { get; set; }
    public Movie? Movie { get; set; }
    public List<Movie>? AllMovies { get; set; }
}