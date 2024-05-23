using MovieApp.Services;
using MovieApp.Views;
using Xamarin.Forms;

namespace MovieApp
{
    public partial class App : Application
    {
        public MovieService MovieService { get; set; }
        public App()
        {
            InitializeComponent();

            MovieService = new MovieService(apiKey: "c6a572dac0934dcfa8680f958a3cfefb");
            MainPage = new GenresPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
