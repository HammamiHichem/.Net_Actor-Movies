#pragma warning disable CS8618
// We can disable our warnings safely because we know the framework will assign non-null values 
// when it constructs this class for us.
using Microsoft.EntityFrameworkCore;
namespace ActrosMovies.Models;
// the MyContext class represents a session with our MySQL database, allowing us to query for or save data
// DbContext is a class that comes from EntityFramework, we want to inherit its features
public class MyContext : DbContext 
{   
    // This line will always be here. It is what constructs our context upon initialization  
    public MyContext(DbContextOptions options) : base(options) { }    
    // We need to create a new DbSet<Model> for every model in our project that is making a table
    // The name of our table in our database will be based on the name we provide here
    // This is where we provide a plural version of our model to fit table naming standards    
    public DbSet<User> Users { get; set; }
     public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; } 
    public DbSet<Association> Associations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la relation many-to-many entre Movies et Actors via Association
            modelBuilder.Entity<Association>()
                .HasOne(a => a.Actor)
                .WithMany(b => b.AllMovies)
                .HasForeignKey(a => a.ActorId);

            modelBuilder.Entity<Association>()
                .HasOne(a => a.Movie)
                .WithMany(b => b.AllActors)
                .HasForeignKey(a => a.MovieId);
        }
    }
