using API_Authentication__Minimal_API.Models;

namespace API_Authentication__Minimal_API.Services
{
    public interface IMovovieService
    {
        public Movie Create(Movie movie);
        public Movie Get(int id);
        public List<Movie> List();
        public Movie Update(Movie movie);
        public bool Delete(int id);

    }
}
