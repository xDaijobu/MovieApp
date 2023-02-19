using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp.Controls.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VideoPopup : PopupPage
	{
		public VideoPopup (string url)
		{
			InitializeComponent ();

            webView.Source = url;
        }
	}
}