using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MovieApp.Models;
using MovieApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace MovieApp.ViewModels
{
    public class GenresViewModel : BaseViewModel
    {
        private LayoutState currentState = LayoutState.Loading;

        public LayoutState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private bool isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        private ObservableRangeCollection<MovieList> movieList;
        public ObservableRangeCollection<MovieList> MovieList
        {
            get => movieList;
            set => SetProperty(ref movieList, value);
        }

        public IAsyncCommand<MovieList> MoreClickedCommand => new AsyncCommand<MovieList>(async (movieList) =>
        {
            await CurrentNavigation.PushModalAsync(new MoviesPage(movieList));
        }, allowsMultipleExecutions: false);

        public IAsyncCommand GetDataCommand => new AsyncCommand(GetData);

        public GenresViewModel()
        {
            Title = "Genre";

            _ = Task.Run(async () =>
            {
                await GetData();
            });
        }

        private async Task GetData()
        {
            try
            {
                IsRunning = true;
                CurrentState = LayoutState.Loading;

                MovieList = new ObservableRangeCollection<MovieList>();
                var movies = new List<MovieList>();
                if (!HasInternet)
                {
                    ShowAlert("Sorry, we couldn't load the data. Please check your internet connection and try again later.");
                    return;
                }

                var genres = await CurrentApp.MovieService.GetGenres(DataInfoType.Movie);

                foreach (var genre in genres)
                {
                    var moviesByGenre = (await CurrentApp.MovieService.GetMovies(genre.Id, page: 1));

                    foreach (var movie in moviesByGenre.Results)
                    {
                        UriImageSource imageSource = new UriImageSource
                        {
                            Uri = new Uri(movie.Poster),
                            CachingEnabled = true, // Enable caching if needed
                            CacheValidity = TimeSpan.FromDays(1) // Cache validity period
                        };
                        movie.ImageSource = imageSource;
                    }                    
                    
                    movies.Add(new MovieList(genre, moviesByGenre.Results));
                }

                MovieList.ReplaceRange(movies);

                // await Task.Delay(500);
                CurrentState = LayoutState.Success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ShowAlert(ex.Message);
            }
            finally
            {
                IsRunning = false;
            }
        }

        private void ShowAlert(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await CurrentApp.MainPage.DisplayAlert("Error", message, "Ok");
                CurrentState = LayoutState.Error;
            });
        }
    }
}
