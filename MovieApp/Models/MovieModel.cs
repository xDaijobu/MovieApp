using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MovieApp.Models
{
    public class Constants
    {
        public static string BASE_IMAGE_URL = "https://image.tmdb.org/t/p/w500/";
    }
    public enum DataInfoType
    {
        Movie,
        Television,
        Combined
    }

    public class BaseModel : INotifyPropertyChanged
    {
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

    public abstract class Resource
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }
    }


    public class Movie : Resource, INotifyPropertyChanged
    {
        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; internal set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; internal set; }

        [JsonProperty("overview")]
        public string Overview { get; internal set; }

        private string poster;
        [JsonProperty("poster_path")]
        public string Poster
        {
            get => Constants.BASE_IMAGE_URL + poster;
            set => poster = value;
        }
        
        private ImageSource imgSource;
        public ImageSource ImageSource
        {
            get => imgSource;
            set => SetProperty(ref imgSource, value);
        }

        private string backdrop;

        [JsonProperty("backdrop_path")]
        public string Backdrop
        {
            get => Constants.BASE_IMAGE_URL + backdrop;
            set => backdrop = value;
        }

        [JsonProperty("adult")]
        public bool Adult { get; internal set; }

        [JsonProperty("genre_ids")]
        public IEnumerable<int> GenreIds { get; internal set; }
        public IList<Genre> Genres { get; internal set; }

        [JsonProperty("video")]
        public bool Video { get; private set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; internal set; }

        [JsonProperty("vote_average")]
        public double? VoteAverage { get; internal set; }

        [JsonProperty("vote_count")]
        public double? VoteCount { get; internal set; }

        [JsonProperty("popularity")]
        public double? Populariry { get; internal set; }

        public Movie()
        {
            Genres = new List<Genre>();
        }

        
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

    public class Movies : PagedResult<Movie>
    {

    }

    public class MovieList : BaseModel
    {
        private Genre genre;
        public Genre Genre
        {
            get => genre;
            set => SetProperty(ref genre, value);
        }

        private ObservableCollection<Movie> movies;
        public ObservableCollection<Movie> Movies
        {
            get => movies;
            set => SetProperty(ref movies, value);
        }

        public MovieList(Genre genre, IEnumerable<Movie> movies)
        {
            Genre = genre;
            Movies = new ObservableCollection<Movie>(movies);
        }
    }

    public class Genre : Resource
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        public string Image => "https://img.icons8.com/000000/" + Name.Replace(" ", "-").ToLower();
    }

    internal class Genres
    {
        [JsonProperty("genres")]
        public IEnumerable<Genre> Results { get; internal set; }
    }

    public class Review
    {
        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("author")]
        public string Author { get; internal set; }

        [JsonProperty("author_details")]
        public AuthorDetails AuthorDetails { get; internal set; }

        [JsonProperty("content")]
        public string Content { get; internal set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; internal set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; internal set; }

        [JsonProperty("url")]
        public string Url { get; internal set; }
    }

    public class Reviews : PagedResult<Review>
    {
    }

    public class AuthorDetails
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("username")]
        public string UserName { get; internal set; }

        private string avatarPath;

        [JsonProperty("avatar_path")]
        public string AvatarPath
        {
            get => Constants.BASE_IMAGE_URL + avatarPath;
            set => avatarPath = value;
        }

        [JsonProperty("rating")]
        public double? Rating { get; internal set; }
    }

    public class MediaCredits
    {
        [JsonProperty("cast")]
        public IEnumerable<MediaCast> Cast { get; internal set; }

        [JsonProperty("crew")]
        public IEnumerable<MediaCrew> Crew { get; internal set; }
    }

    public abstract class MediaCredit
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        private string profile;

        [JsonProperty("profile_path")]
        public string Profile
        {
            get => Constants.BASE_IMAGE_URL + profile;
            set => profile = value;
        }
    }

	public class MediaCast : MediaCredit
	{
		[JsonProperty("character")]
		public string Character { get; internal set; }
	}

	public class MediaCrew : MediaCredit
	{
		[JsonProperty("department")]
		public string Department { get; internal set; }

		[JsonProperty("job")]
		public string Job { get; internal set; }
	}

    public class Videos
    {
        [JsonProperty("results")]
        public IEnumerable<Video> Results { get; internal set; }
    }

    public class Video
    {
        [JsonProperty("id")]
        public string Id { get; internal set; }

        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("iso_639_1")]
        public string LanguageCode { get; internal set; }

        [JsonProperty("key")]
        public string Key { get; internal set; }

        public string YoutubeUrl => "https://www.youtube.com/watch?v=" + Key;

        public string Duration => "1m 45s";

        public string Thumbnail
        {
            get
            {
                string youTubeThumb = string.Empty;
                if (YoutubeUrl == "")
                    return "";

                if (YoutubeUrl.IndexOf("=") > 0)
                {
                    youTubeThumb = YoutubeUrl.Split('=')[1];
                }
                else if (YoutubeUrl.IndexOf("/v/") > 0)
                {
                    string strVideoCode = YoutubeUrl.Substring(YoutubeUrl.IndexOf("/v/") + 3);
                    int ind = strVideoCode.IndexOf("?");
                    youTubeThumb = strVideoCode.Substring(0, ind == -1 ? strVideoCode.Length : ind);
                }
                else if (YoutubeUrl.IndexOf('/') < 6)
                {
                    youTubeThumb = YoutubeUrl.Split('/')[3];
                }
                else if (YoutubeUrl.IndexOf('/') > 6)
                {
                    youTubeThumb = YoutubeUrl.Split('/')[1];
                }

                return "http://img.youtube.com/vi/" + youTubeThumb + "/mqdefault.jpg";
            }
        }

        [JsonProperty("site")]
        public string Site { get; internal set; }

        [JsonProperty("size")]
        public int Size { get; internal set; }

        [JsonProperty("type")]
        public string Type { get; internal set; }
    }

    public abstract class PagedResult<T>
    {
        [JsonProperty("id")]
        public int Id { get; internal set; }

        [JsonProperty("results")]
        public IEnumerable<T> Results { get; internal set; }

        [JsonProperty("page")]
        public int PageIndex { get; internal set; }

        [JsonProperty("total_pages")]
        public int PageCount { get; internal set; }

        [JsonProperty("total_results")]
        public int TotalCount { get; internal set; }
    }
}
