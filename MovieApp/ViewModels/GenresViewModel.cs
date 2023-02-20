using System;
using System.Collections.Generic;
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
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CurrentApp.MainPage.DisplayAlert("Error", "No internet connection", "Ok");
                        CurrentState = LayoutState.Error;
                    });
                    return;
                }

                var genres = await CurrentApp.MovieService.GetGenres(DataInfoType.Movie);

                foreach (var genre in genres)
                {
                    var moviesByGenre = (await CurrentApp.MovieService.GetMovies(genre.Id, page: 1));

                    movies.Add(new MovieList(genre, moviesByGenre.Results));
                }

                MovieList.ReplaceRange(movies);

                await Task.Delay(500);
                CurrentState = LayoutState.Success;
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await CurrentApp.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    CurrentState = LayoutState.Error;
                });
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}
