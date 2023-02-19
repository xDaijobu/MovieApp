using MovieApp.Services;
using MovieApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
    public partial class App : Application
    {
        public MovieService MovieService { get; set; }
        public App()
        {
            InitializeComponent();

            MovieService = new MovieService(apiKey: "b7f8ec37c15f9a1853235c57710176c4");
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
