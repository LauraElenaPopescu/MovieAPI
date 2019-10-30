using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieDataAccess;

namespace MovieAPI.Controllers
{
    public class MovieController : ApiController
    {
        // retrieve all movies
        public IEnumerable<Movies> GetMovies()
        {
            using (MovieDbEntities entities = new MovieDbEntities())
            {
                return entities.Movie.ToList();
            }
        }

        // retrieve a single movie
        public Movies Get(int id)
        {
            using (MovieDbEntities entities = new MovieDbEntities())
            {
                return entities.Movie.FirstOrDefault(e => e.ID == id);
            }
        }


        // create a new movie
        public HttpResponseMessage Post([FromBody] Movies Movie)
        {
            try
            {
                using (MovieDbEntities entities = new MovieDbEntities())
                {
                    entities.Movie.Add(Movie);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, Movie);
                    message.Headers.Location = new Uri(Request.RequestUri + Movie.ID.ToString());
                    return message;

                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }

        }

        // edit an existing movie
        public HttpResponseMessage Put(int id, [FromBody]Movies Movie)
        {
            try
            {
                using (MovieDbEntities entities = new MovieDbEntities())
                {
                    var entity = entities.Movie.FirstOrDefault(e => e.ID == id);
                    entity.Title = Movie.Title;
                    entity.Year = Movie.Year;
                    entity.Director = Movie.Director;
                    entity.Genre = Movie.Genre;
                    entity.Synopsis = Movie.Synopsis;
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK, Movie);
                    return message;
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }

        }

        // delete a movie

        public HttpResponseMessage Delete(Movies Movie, int id)
        {
            try
            {
                using (MovieDbEntities entities = new MovieDbEntities())
                {
                    entities.Movie.Remove(entities.Movie.FirstOrDefault(e => e.ID == id));
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK);
                    return message;
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }

        }

    }
}
