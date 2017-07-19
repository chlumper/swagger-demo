using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_core.Controllers
{
    /// <summary>
    /// Movie related actions
    /// </summary>
    [Route("v1/")]
    public class MovieController : Controller
    {

        private readonly DataContext _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">EF context</param>
        public MovieController(DataContext context)
        {
            _context = context;

            if (_context.Movies.Count() == 0)
            {
                _context.Movies.Add(new Movie { 
                    Title = "Django Unchained",
                    ReleaseDate = new DateTime(2013, 01, 18),
                    Rating = 8.4,
                    ImdbId = "tt1853728"
                });
                _context.Movies.Add(new Movie { 
                    Title = "The Godfather",
                    ReleaseDate = new DateTime(1972, 03, 14),
                    Rating = 9.2,
                    ImdbId = "tt0068646"
                });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieve movies.
        /// </summary>
        [HttpGet("movies")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Movie>),200)]
        public async Task<IEnumerable<Movie>> Get()
        {
            return await _context.Movies.ToListAsync();
        }

        /// <summary>
        /// Retrieve single movie by id
        /// </summary>
        /// <param name="movieId">The internal database id of the movie</param>
        [HttpGet("movie/{movieId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(Movie),200)]
        public async Task<Movie> Get(long movieId)
        {
            if (movieId < 1) {
                BadRequest();
            }

            return await _context.Movies.FindAsync(movieId);
        }

        /// <summary>
        /// Add a movie
        /// </summary>
        /// <param name="movie">The movie</param>  
        [HttpPost("movie")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public IActionResult Post([FromBody]Movie movie)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            } 
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return Ok();        
        }
    }
}
