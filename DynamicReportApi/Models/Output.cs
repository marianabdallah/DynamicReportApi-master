using System.Xml.Serialization;

namespace DynamicReportApi.Models
{
    public class OutputColumn
    {
        [XmlElement]
        public long Order { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string DataType { get; set; }

        [XmlElement]
        public bool ShowInput { get; set; }
    }
}
