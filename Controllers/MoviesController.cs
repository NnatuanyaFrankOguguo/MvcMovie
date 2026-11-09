using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.ViewModels;

namespace MvcMovie.Controllers;

public class MoviesController(MvcMovieContext context) : Controller
{
    public async Task<IActionResult> Index(string movieGenre, string searchString, int? year)
    {
        var movies = context.Movie.AsNoTracking();
        var genreQuery = context.Movie.Select(movie => movie.Genre).Distinct().OrderBy(genre => genre);
        var yearQuery = context.Movie.Select(movie => movie.ReleaseDate.Year).Distinct().OrderByDescending(movieYear => movieYear);

        if (!string.IsNullOrWhiteSpace(searchString))
            movies = movies.Where(movie => movie.Title!.Contains(searchString));
        if (!string.IsNullOrWhiteSpace(movieGenre))
            movies = movies.Where(movie => movie.Genre == movieGenre);
        if (year.HasValue)
            movies = movies.Where(movie => movie.ReleaseDate.Year >= year.Value);

        return View(new MovieGenreViewModel
        {
            Movies = await movies.OrderBy(movie => movie.Title).ToListAsync(),
            Genres = new SelectList(await genreQuery.ToListAsync()),
            Years = new SelectList(await yearQuery.ToListAsync(), year),
            MovieGenre = movieGenre,
            SearchString = searchString,
            Year = year
        });
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var movie = await context.Movie.AsNoTracking().FirstOrDefaultAsync(movie => movie.Id == id);
        return movie == null ? NotFound() : View(movie);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (!ModelState.IsValid) return View(movie);
        context.Add(movie);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var movie = await context.Movie.FindAsync(id);
        return movie == null ? NotFound() : View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id) return NotFound();
        if (!ModelState.IsValid) return View(movie);
        try
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!MovieExists(movie.Id))
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var movie = await context.Movie.AsNoTracking().FirstOrDefaultAsync(movie => movie.Id == id);
        return movie == null ? NotFound() : View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await context.Movie.FindAsync(id);
        if (movie != null) context.Movie.Remove(movie);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id) => context.Movie.Any(movie => movie.Id == id);
}
