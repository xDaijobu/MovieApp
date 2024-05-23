using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MovieApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MovieApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool HasInternet
        {
            get
            {
                try
                {
                    return Connectivity.NetworkAccess == NetworkAccess.Internet;
                }
                catch
                {
                    return false;
                }
            }
        }

        protected App CurrentApp => (Application.Current as App);
        protected INavigation CurrentNavigation => CurrentApp?.MainPage?.Navigation;

        string title = string.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public IAsyncCommand<Movie> GoToMovieDetailCommand => new AsyncCommand<Movie>(async (movie) =>
        {
            await CurrentNavigation.PushModalAsync(new MovieDetailPage(movie));
        }, allowsMultipleExecutions: false);

        public IAsyncCommand PopClickedCommand => new AsyncCommand(async () =>
        {
            await CurrentNavigation.PopModalAsync();
        }, allowsMultipleExecutions: false);

        public virtual void OnAppearing()
            => Debug.WriteLine($"OnAppearing - Page: {Title}");
        public virtual void OnDisappearing() 
            => Debug.WriteLine($"OnDisappearing - Page: {Title}");

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
