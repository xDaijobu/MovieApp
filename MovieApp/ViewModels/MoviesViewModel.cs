using MovieApp.Models;
using System;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

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
            try
            {
                Movies datas = await CurrentApp.MovieService.GetMovies(Genre.Id, page: currentPage++);

                if (datas != null && datas.TotalCount > 0) 
                {
                    Movies.AddRange(datas.Results);
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await CurrentApp.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                });
            }
        }, allowsMultipleExecutions: false);

        public MoviesViewModel(MovieList movieList)
        {
            Genre = movieList.Genre;
            Movies = new ObservableRangeCollection<Movie>(movieList.Movies);
        }
    }
}
