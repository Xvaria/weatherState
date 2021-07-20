using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace weatherState
{
    public class City
    {
        public string city { get; set; }
    }
    public class Comment
    {
        public static string comment = "Bogotá";
    }
    public class WeatherStatus
    {
        public string name { get; set; }
        public string country { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public List<CityNews> citynews { get; set; }
    }

    public class CityNews
    {
        public string name { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public DateTime publishedAt { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public string content { get; set; }
    }

    // weather
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Root
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    // News
    public class Source
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Article
    {
        public Source source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime publishedAt { get; set; }
        public string content { get; set; }
    }

    public class News
    {
        public string status { get; set; }
        public int totalResults { get; set; }
        public List<Article> articles { get; set; }
    }
    public class Requests
    {
        public static WeatherStatus create()
        {
            Root weather;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($@"http://api.openweathermap.org/data/2.5/weather?q={Comment.comment}&APPID=4322b65473997aefc011cb7ba01c4eaf");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                weather = JsonConvert.DeserializeObject<Root>(json);
            }
            WeatherStatus res = new WeatherStatus();
            res.name = weather.name;
            res.country = weather.sys.country;
            res.status = weather.weather[0].main;
            res.description = weather.weather[0].description;
            res.temp = (Math.Truncate((weather.main.temp - 273.15) * 100) / 100);
            res.feels_like = (Math.Truncate((weather.main.feels_like - 273.15) * 100) / 100);
            res.pressure = weather.main.pressure;
            res.humidity = weather.main.humidity;

            News news;
            byte[] bytes = Encoding.Default.GetBytes(Comment.comment);
            string newcity = Encoding.UTF32.GetString(bytes);
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create($@"https://newsapi.org/v2/everything?q={newcity}1&pageSize=2&apiKey=6b113ff4e33a479797dff06834feb41a");
            using (HttpWebResponse response = (HttpWebResponse)request1.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                news = JsonConvert.DeserializeObject<News>(json);
            }

            List<CityNews> cities = new List<CityNews>();
            for (int i = 0; i < news.articles.Count; i++)
            {
                CityNews res1 = new CityNews();
                res1.name = news.articles[i].source.name;
                res1.author = news.articles[i].author;
                res1.title = news.articles[i].title;
                res1.publishedAt = news.articles[i].publishedAt;
                res1.description = news.articles[i].description;
                res1.url = news.articles[i].url;
                res1.urlToImage = news.articles[i].urlToImage;
                res1.content = news.articles[i].content;
                cities.Add(res1);
            }
            res.citynews = cities;
            return res;
        }
        public static void show()
        {

        }
    }
}
