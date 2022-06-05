using API_Authentication__Minimal_API.Models;
using API_Authentication__Minimal_API.Repositories;

namespace API_Authentication__Minimal_API.Services
{
    public class MovieService : IMovovieService
    {
        public Movie Create(Movie movie)
        {
            movie.Id = MovieRepository.Movies.Count + 1;
            MovieRepository.Movies.Add(movie);
            return movie;
        }

        public bool Delete(int id)
        {
           var movie=MovieRepository.Movies.FirstOrDefault(o=>o.Id==id);
            if(movie is null)return false;
            MovieRepository.Movies.Remove(movie);
            return true;
        }

        public Movie Get(int id)
        {
            var movie = MovieRepository.Movies.FirstOrDefault(o => o.Id == id);
            if(movie is null)return null;
            return movie;
        }

        public List<Movie> List()
        {
            var movies=MovieRepository.Movies.ToList();
            return movies;
        }

        public Movie Update(Movie Newmovie)
        {
            var oldMovie = MovieRepository.Movies.FirstOrDefault(o => o.Id == Newmovie.Id);
            if(oldMovie is null)return null;
           
            oldMovie.Title=Newmovie.Title;
            oldMovie.Description = Newmovie.Description;
            oldMovie.Rate=Newmovie.Rate;
            return Newmovie;
        }
    }
}
