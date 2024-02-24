using System.Xml.Serialization;

namespace BacklogTracker.Models
{
    public class Game
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("site_detail_url")]
        public string SiteDetailUrl { get; set; }

        [XmlElement("resource_type")]
        public string ResourceType { get; set; } // TODO: Change this to enum?

        [XmlElement("description")]
        public string Description { get; set; }
    }
}
