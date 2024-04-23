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
        public string ResourceType { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("guid")]
        public string Guid { get; set; }

        [XmlElement("id")]
        public string Id { get; set; }
    }
}
