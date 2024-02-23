using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Xml.Linq;
using System.Xml.Serialization;
using BacklogTracker.Data;

namespace BacklogTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var query = Request.Form["query"];

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.giantbomb.com/api/search/");
            client.DefaultRequestHeaders.Add("User-Agent", "My Awesome App");
            // Get data response
            var response = client.GetAsync($"?api_key=971b75abd389a994ecfcc70ae5ebab3e2abe2a35&query={query}&resources=game&field_list=name,api_detail_url").Result;

            XmlSerializer xs = new XmlSerializer(typeof(Response));

            using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
            {
                var results = (Response)xs.Deserialize(reader);
                _logger.LogInformation("Deserialization complete!");
            }

            _logger.LogInformation("Request complete!");
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


    }
    public class Serializer
    {
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}
