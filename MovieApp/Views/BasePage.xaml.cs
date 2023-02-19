using MovieApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BasePage : ContentPage
	{
        private BaseViewModel baseViewModel;

        public BasePage ()
		{
			InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            baseViewModel = BindingContext as BaseViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            baseViewModel?.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            baseViewModel?.OnDisappearing();
        }
    }
}