using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet_core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_core.Controllers
{
    /// <summary>
    /// Movie related actions
    /// </summary>
    [Route("v1/")]
    [Authorize]
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
                    Rating = 4,
                    ImdbId = "tt1853728",
                    Genre = Genre.Drama
                });
                _context.Movies.Add(new Movie { 
                    Title = "The Godfather",
                    ReleaseDate = new DateTime(1972, 03, 14),
                    Rating = 5,
                    ImdbId = "tt0068646",
                    Genre = Genre.None
                });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Retrieve movies.
        /// </summary>
        /// <response code="200">Successful operation</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("movies")]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<Movie>),200)]
        public IActionResult Get()
        {
            return Ok(_context.Movies.ToList());
        }

        /// <summary>
        /// Retrieve single movie by id
        /// </summary>
        /// <param name="movieId">The internal database id of the movie</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid Input</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("movie/{movieId}")]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Movie),200)]        
        public IActionResult Get(long movieId)
        {
            if (movieId < 1) {
                return BadRequest();
            }
            var movie = _context.Movies.Find(movieId);
            if (movie == null) {
                return NotFound();                
            }
            return Ok(movie);
        }

        /// <summary>
        /// Add a movie
        /// </summary>
        /// <param name="movie">The movie</param> 
        /// <response code="200">Saved Successful</response>
        /// <response code="400">Invalid Input</response>
        /// <response code="500">Internal Server Error</response>
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
