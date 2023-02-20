using System;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Controls.Popups;
using MovieApp.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MovieApp.ViewModels
{
    public class MovieDetailViewModel : BaseViewModel
    {
        public Movie Movie { get; set; }
        public string Genres => "Genre: " + string.Join(", ", Movie.Genres.Select(x => x.Name));

        private ObservableRangeCollection<MediaCast> casts;
        public ObservableRangeCollection<MediaCast> Casts
        {
            get => casts;
            set => SetProperty(ref casts, value);
        }

        private ObservableRangeCollection<Video> videos;
        public ObservableRangeCollection<Video> Videos
        {
            get => videos;
            set => SetProperty(ref videos, value);
        }

        private ObservableRangeCollection<Review> reviews;
        public ObservableRangeCollection<Review> Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
        }

        private int currentReview = 1;

        public IAsyncCommand ItemTresholdReachedCommand => new AsyncCommand(async () =>
        {
            var datas = await CurrentApp.MovieService.GetReviews(Movie.Id, page: currentReview++);

            Reviews.AddRange(datas.Results);
        }, allowsMultipleExecutions: false);

        public IAsyncCommand<string> GoToVideoPopupCommand => new AsyncCommand<string>(async (url) =>
        {
            await CurrentNavigation.PushPopupAsync(new VideoPopup(url));
        }, allowsMultipleExecutions: false);

        public MovieDetailViewModel(Movie movie)
        {
            Movie = movie;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            _ = Task.Run(async () =>
            {
                try
                {
                    if (!HasInternet)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await CurrentApp.MainPage.DisplayAlert("Error", "No internet connection", "Ok");
                        });
                        return;
                    }

                    var credits = await CurrentApp.MovieService.GetCredits(Movie.Id);
                    Casts = new ObservableRangeCollection<MediaCast>(credits.Cast);

                    var videos = await CurrentApp.MovieService.GetVideos(Movie.Id);
                    Videos = new ObservableRangeCollection<Video>(videos);

                    var reviews = await CurrentApp.MovieService.GetReviews(Movie.Id, 1);
                    Reviews = new ObservableRangeCollection<Review>(reviews.Results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CurrentApp.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                    });
                }
            });
        }
    }
}
