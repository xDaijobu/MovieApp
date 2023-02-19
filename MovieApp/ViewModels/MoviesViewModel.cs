using MovieApp.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MovieApp.ViewModels
{
    public class MoviesViewModel : BaseViewModel
    {
        public Genre Genre { get; set; }
        private ObservableRangeCollection<Movie> movies;

        public ObservableRangeCollection<Movie> Movies
        {
            get => movies;
            set => SetProperty(ref movies, value);
        }

        private int currentPage = 1;
        public IAsyncCommand ItemTresholdReachedCommand => new AsyncCommand(async () =>
        {
            var datas = await CurrentApp.MovieService.GetMovies(Genre.Id, page: currentPage++);

            Movies.AddRange(datas.Results);
        }, allowsMultipleExecutions: false);

        public MoviesViewModel(MovieList movieList)
        {
            Genre = movieList.Genre;
            Movies = new ObservableRangeCollection<Movie>(movieList.Movies);
        }
    }
}
