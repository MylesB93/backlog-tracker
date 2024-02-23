using System.Xml.Serialization;

namespace BacklogTracker.Models
{
    [XmlRoot("response")]
    public class Response
    {
        [XmlArray("results")]
        [XmlArrayItem("game")]
        public List<Game> Games { get; set; }
    }
}
