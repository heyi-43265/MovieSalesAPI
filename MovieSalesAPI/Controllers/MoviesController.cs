﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSalesAPI.Data;

namespace MovieSalesAPI.Controllers
{
    /// <summary>
    /// Retrieve and interact with movie data.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        //Good practices to keep in mind:
        //https://blog.mwaysolutions.com/2014/06/05/10-best-practices-for-better-restful-api/

        private readonly IMovieData _movieData;

        /// <summary>
        /// Class Declaration
        /// </summary>
        /// <param name="movieData"></param>
        public MoviesController(
            IMovieData movieData
        )
        { 
            _movieData = movieData;
        }

        #region USERS API ACTIONS

        // GET: api/Movie
        /// <summary>
        /// Retrieve all movie details
        /// </summary>
        /// <returns>Returns a list of movie details.</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("users/all")]
        public List<IMovie> GetAllUsersMovieDetails
        (
            // [FromHeader] string authorization
        )
        {

            //Access the Claim.Name value
            //var userName = User.Identity.Name;
            /* List<Movie> movies = new List<Movie>
             {
                 new Movie
                 {
                     Name = "Test",
                     Description = "Test"
                 }
             };

             return movies;*/

            if (User.Identity.IsAuthenticated)
            {
                return _movieData.GetAllMovieDetails(User.Identity.Name);
            }
            else
            {
                return null;
            }
        }

        // GET: Movies/users/movieid
        /// <summary>
        /// Retrieve specific movie
        /// </summary>
        /// <param name="movieid">Movie Id</param>
        /// <returns>Return a movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("users/{movieid}")]
        public List<IMovie> GetSpecificMovieDetailsById([FromRoute] int movieid)
        {
            if (User.Identity.IsAuthenticated)
            {
                return _movieData.GetSpecificMovieDetailsById(movieid, User.Identity.Name);
            }
            else
            {
                return null;
            }
        }


        // GET: Movies/users/moviename
        /// <summary>
        /// Retrieve specific movie by name
        /// </summary>
        /// <param name="moviename">Movie Name</param>
        /// <returns>Return a movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpGet]
        [Route("users/{moviename}")]
        public List<IMovie> GetSpecificMovieDetailsByName([FromRoute] string moviename)
        {
            if (User.Identity.IsAuthenticated)
            {
                return _movieData.GetSpecificMovieDetailsByName(moviename, User.Identity.Name);
            }
            else
            {
                return null;
            }
        }


        // POST: Movies/movie
        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <param name="movie">Movie</param>
        /// <returns>Return the movie created</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPost]
        [Route("movie")]
        public List<IMovie> SaveMovieToDatabase([FromBody] IMovie movie)
        {
            return null;
        }

        // PUT: Movies/movie/id
        /// <summary>
        /// Update an entire movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movie"></param>
        /// <returns>Return the movie that was updated</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPut]
        [Route("movie/{id}")]
        public List<IMovie> UpdateEntireMovieInDatabaseById([FromRoute] int id, [FromBody] IMovie movie)
        {
            return null;
        }

        // PUT: Movies/movie/imoviename
        /// <summary>
        /// Update movie
        /// </summary>
        /// <param name="moviename"></param>
        /// <param name="movie"></param>
        /// <returns>Return updated movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPut]
        [Route("movie/{moviename}")]
        public List<IMovie> UpdateEntireMovieInDatabaseByName([FromRoute] string moviename, [FromBody] IMovie movie)
        {
            return null;
        }

        //PATCH Example: https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        //https://kimsereyblog.blogspot.com/2017/11/implement-patch-on-asp-net-core-with.html
        //If you do not have the PATCH Nuget package
        //Open Tools > Package Manager Console
        //Do this command:
        //Install-Package Microsoft.AspNetCore.JsonPatch

        //PATCH: Movies/movie/id
        /// <summary>
        /// Patch a portion of a movie
        /// </summary>
        /// <param name="movieid"></param>
        /// <param name="moviePatch"></param>
        /// <returns>Return the patched movie</returns>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpPatch]
        [Route("movie/{id}")]
        public List<IMovie> UpdatePartialMovieInDatabaseById([FromRoute]int movieid, [FromBody]JsonPatchDocument<IMovie> moviePatch)
        {
            //The idea is that you pull: somemoviefromdatabase
            //From the database
            //Then apply the 'moviePatch' to it
            //then save it back to the database
            //then reutrn the newly changed somemoviefromdatabase
            //object tback to the user

            List<IMovie> someMovie = _movieData.GetSpecificMovieDetailsById(movieid, User.Identity.Name);
            moviePatch.ApplyTo(someMovie[0]);

            _movieData.SaveMovieToDatabase(someMovie[0], User.Identity.Name);
            return someMovie;
        }

        // DELETE: Movies/movie/id
        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <param name="id"></param>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpDelete]
        [Route("movie/{id}")]
        public void DeleteMovieFromDatabaseById([FromRoute] int id)
        {
        }

        // DELETE: Movies/movie/name
        /// <summary>
        /// Delete a movie by name - *Could delete more than one movie with same name
        /// </summary>
        /// <param name="name"></param>
        [Authorize(Policy = "APIMovieAccess")]
        [HttpDelete]
        [Route("movie/{name}")]
        public void DeleteMovieFromDatabaseByName([FromRoute] string name)
        {
            
        }

        #endregion

    }
}
