using System;
using CommandLine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq;

namespace RESTful_WebServices
{

    public class Post
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return $"PostId:{Id} - UserId:{UserId} - Title: {Title} \n{Body}\n";
        }
    }

    public class Comment
    {
        
        public string PostId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public override string ToString()
        {
            return $"{ PostId}, {Id}, {Name}, {Email} \n{Body}\n";
        }
    }
    public class Album
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public override string ToString()
        {
            return $"{ UserId}, {Id}, {Title}";
        }
    }
    public class Photo
    {
        public string AlbumId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public override string ToString()
        {
            return $"{ AlbumId}, {Id}, {Title}, {Url}, {ThumbnailUrl}";
        }
    }

    class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Enter a UserId. 1 to 10");
            string user = Console.ReadLine();

            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"/posts?userId={user}";
            HttpResponseMessage response = await client.GetAsync(url);
            var resp = await response.Content.ReadAsStringAsync();
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(resp);
            var userPostsIds = posts.Select(x => x.Id).ToList();

            var url1 = $"/comments";
            HttpResponseMessage response1 = await client.GetAsync(url1);
            var resp1 = await response1.Content.ReadAsStringAsync();
            List<Comment> comments = JsonConvert.DeserializeObject<List<Comment>>(resp1);

            var url2 = $"/albums";
            HttpResponseMessage response2 = await client.GetAsync(url2);
            var resp2 = await response2.Content.ReadAsStringAsync();
            List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(resp2);
            var userAlbumIds = albums.Where(x => x.UserId == user).Select(x => x.Id).ToList();

            var url3 = $"/photos";
            HttpResponseMessage response3 = await client.GetAsync(url3);
            var resp3 = await response3.Content.ReadAsStringAsync();
            List<Photo> photos = JsonConvert.DeserializeObject<List<Photo>>(resp3);


            Console.WriteLine("Posts : ");
            posts.ForEach(Console.WriteLine);

            Console.WriteLine("Comments : ");
            foreach (string post in userPostsIds)
            {
                comments.Where(x => x.PostId == post).ToList().ForEach(i => Console.WriteLine(i));
            }

            Console.WriteLine("Photos : ");
            foreach (string album in userAlbumIds)
            {
                photos.Where(x => x.AlbumId == album).ToList().ForEach(i => Console.WriteLine(i));
            }
        }
    }
}
