using BacklogTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace BacklogTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IOptions<GiantBombConfiguration> _giantBombConfiguration;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public Response GamesResponse { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IOptions<GiantBombConfiguration> giantBombConfiguration)
        {
            _logger = logger;
            _giantBombConfiguration = giantBombConfiguration;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var query = Request.Form["query"];

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://www.giantbomb.com/api/search/");
            client.DefaultRequestHeaders.Add("User-Agent", "Backlog Tracker app");
            // Get data response
            var response = client.GetAsync($"?api_key={_giantBombConfiguration.Value.GiantBombAPIKey}&query={query}&resources=game&field_list=name,site_detail_url,description").Result;

            XmlSerializer xs = new XmlSerializer(typeof(Response));

            using (StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
            {
                GamesResponse = (Response)xs.Deserialize(reader);
                _logger.LogInformation("Deserialization complete!");
            }

            _logger.LogInformation("Request complete!");
        }
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
