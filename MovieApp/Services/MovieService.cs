using MovieApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Services
{
    public class MovieService
    {
        private readonly HttpClient httpClient;
        private JsonSerializerSettings serializeSettings;
        private readonly string apiKey;
        public MovieService(string apiKey)
        {            
            this.apiKey = apiKey;
            //2023-02-12T13:47:18.985Z
            serializeSettings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddThh:mm:ss.000Z", DateTimeZoneHandling = DateTimeZoneHandling.Utc };

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri("http://api.themoviedb.org/3/");
        }

        public async Task<IEnumerable<Genre>> GetGenres(DataInfoType type, string language = "en-US")
        {
            var sb = new StringBuilder("genre");

            switch(type)
            {
                case DataInfoType.Movie: 
                    sb.AppendLine("/movie/list");
                    break;
                case DataInfoType.Television:
                    sb.AppendLine("/tv/list");
                    break;
                case DataInfoType.Combined:
                    sb.AppendLine("/list"); 
                    break;
            }

            var parameters = new Dictionary<string, object>
            {
                { "language", language }
            };

            var genres = await GetAsync<Genres>(sb.Replace("\n","").ToString(), parameters);
            return genres.Results;
        }

        public async Task<Movies> GetMovies(int genreId, int page, string language = "en-US")
        {
            var parameters = new Dictionary<string, object>
            {
                { "language", language },
                { "page", page }
            };

            var movies = await GetAsync<Movies>($"genre/{genreId}/movies", parameters);
            var genres = await GetGenres(DataInfoType.Movie);
            var genresV2 = await GetGenres(DataInfoType.Television);
            List<Genre> finalGenres = new List<Genre>();
            finalGenres.AddRange(genres);
            finalGenres.AddRange(genresV2);
            foreach (var movie in movies.Results)
            {
                foreach (var id in movie.GenreIds)
                {
                    var x = finalGenres.FirstOrDefault(x => x.Id == id);
                    movie.Genres.Add(x);
                }
            }
            return movies;
        }

        public Task<Review> GetReview(string reviewId)
            => GetAsync<Review>($"review/{reviewId}", null);

        public Task<Reviews> GetReviews(int id, int page, string language ="en-US")
        {
            var parameters = new Dictionary<string, object> { { "page", page }, { "language", language } };
            return GetAsync<Reviews>($"movie/{id}/reviews", parameters);
        }

        public Task<MediaCredits> GetCredits(int id)
            => GetAsync<MediaCredits>($"movie/{id}/credits", null);

        public async Task<IEnumerable<Video>> GetVideos(int id, string language = "en-US")
        {
            var parameters = new Dictionary<string, object> { { "language", language } };
            var data = await GetAsync<Videos>($"movie/{id}/videos", parameters);
            return data.Results;
        }


        #region Others
        private async Task<T> GetAsync<T>(string cmd, IDictionary<string, object> parameters)
        {
            
            var httpResponseMessage = await httpClient.GetAsync(CreateRequestUri(cmd, parameters), HttpCompletionOption.ResponseHeadersRead);
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            //Debug.WriteLine($"Response: {response}");
            return JsonConvert.DeserializeObject<T>(response, serializeSettings);
        }

        private string CreateRequestUri(string cmd, IDictionary<string, object> parameters)
        {
            var sb = new StringBuilder($"{cmd}?api_key={apiKey}&");

            if (parameters != null)
            {
                sb.Append(string.Join("&", parameters.Where(s => s.Value != null)
                    .Select(s => string.Concat(s.Key, "=", ConvertParameterValue(s.Value)))));
            }

            Debug.WriteLine("Url: " + sb);
            return sb.ToString();
        }

        private string ConvertParameterValue(object value)
        {
            Type t = value.GetType();
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (t == typeof(DateTime))
                return ((DateTime)value).ToString("yyyy-MM-dd");
            else if (t == typeof(Decimal))
                return ((Decimal)value).ToString(CultureInfo.InvariantCulture);
            else
                return Uri.EscapeDataString(value.ToString());
        }
        #endregion
    }
}
