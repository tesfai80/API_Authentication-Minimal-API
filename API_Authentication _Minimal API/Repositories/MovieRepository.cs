using API_Authentication__Minimal_API.Models;

namespace API_Authentication__Minimal_API.Repositories
{
    public class MovieRepository
    {
        public static List<Movie> Movies = new()
        {
            new Movie()
            {
                Id=1,
                Title="Game of thrones",
                Description="a fantasy tv series",
                Rate=8.9
            },
             new Movie()
            {
                Id=2,
                Title="Rambo",
                Description="an action movie with silvester stalona",
                Rate=9.0
            },
              new Movie()
            {
                Id=3,
                Title="The bold and the beautiful",
                Description="a fantasy tv series",
                Rate=8.9
            },
               new Movie()
            {
                Id=4,
                Title="Blood in Blood Out",
                Description="an action movie",
                Rate=4.9
            },
        };
    }
}
