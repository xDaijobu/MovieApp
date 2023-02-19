using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieApp.Models;
using MovieApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MovieDetailPage : BasePage
	{
		public MovieDetailPage (Movie movie)
		{
			InitializeComponent ();

            BindingContext = new MovieDetailViewModel(movie);
        }
	}
}