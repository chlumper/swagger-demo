using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        [Range(0,5)]
        public int Rating {get;set;}
        
        /// <summary>
        /// The genre of the movie
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Genre Genre {get;set;}

        ///<summary>
        /// The IMDB id of the movie.
        ///</summary>
        /// <example>tt1234567</example>
        [RegularExpression(@"^tt\d{7}$")]
        [StringLength(9, MinimumLength = 9)]
        public string ImdbId {get;set;}
    }


    /// <summary>
    /// The movie genre
    /// </summary>
    public enum Genre {
        ///<summary>
        /// None
        ///</summary>
        None,
        ///<summary>
        /// Drama
        ///</summary>
        Drama,
        ///<summary>
        /// Comedy
        ///</summary>
        Comedy
    }

}