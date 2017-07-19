using System;
using System.ComponentModel.DataAnnotations;

namespace dotnet_core.Models
{
    ///<summary>
    /// The movie entity.
    ///</summary>
    public class Movie {

        ///<summary>
        /// The internal database id of the movie.
        ///</summary>
        public long Id {get;set;}

        ///<summary>
        /// The title of the movie.
        ///</summary>
        [Required]
        public string Title {get;set;}
        
        ///<summary>
        /// The release date of the movie.
        ///</summary>
        [Required]
        public DateTime ReleaseDate {get;set;}

        ///<summary>
        /// The rating for the movie.
        ///</summary>
        public double Rating {get;set;}
        
        ///<summary>
        /// The IMDB id of the movie.
        ///</summary>
        [RegularExpression(@"^tt\d{7}$")]
        public string ImdbId {get;set;}
    }

}