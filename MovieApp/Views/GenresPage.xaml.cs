using MovieApp.ViewModels;
using Xamarin.Forms.Xaml;

namespace MovieApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GenresPage : BasePage
    {
		public GenresPage ()
		{
			InitializeComponent ();
            BindingContext = new GenresViewModel();
        }
	}
}