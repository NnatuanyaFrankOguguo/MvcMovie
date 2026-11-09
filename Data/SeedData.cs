using MvcMovie.Models;

namespace MvcMovie.Data;

public static class SeedData
{
    public static void Initialize(MvcMovieContext context)
    {
        if (context.Movie.Any())
        {
            return;
        }

        context.Movie.AddRange(
            new Movie { Title = "Inception", ReleaseDate = new DateTime(2010, 7, 16), Genre = "Sci-Fi", Price = 12.99M, Rating = "PG-13" },
            new Movie { Title = "The Matrix", ReleaseDate = new DateTime(1999, 3, 31), Genre = "Sci-Fi", Price = 10.99M, Rating = "R" },
            new Movie { Title = "Toy Story", ReleaseDate = new DateTime(1995, 11, 22), Genre = "Animation", Price = 9.99M, Rating = "G" });

        context.SaveChanges();
    }
}
