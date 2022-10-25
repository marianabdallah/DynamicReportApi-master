using System.ComponentModel;
using System.Xml.Serialization;

namespace DynamicReportApi.Models
{
    public class Input
    {
        [XmlElement]
        public long Order { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string DataType { get; set; }

        [XmlElement]
        public bool IsList { get; set; }

        [XmlElement]
        public bool ShowInput { get; set; }

        [XmlElement]
        public ListSource ListSource { get; set; }

        [XmlElement]
        public bool IsEnum { get; set; }

        [XmlElement("EnumValues")]
        public EnumValues Enums { get; set; }

        [XmlElement]
        public string From { get; set; }

        [XmlElement]
        public string To { get; set; }

    }

    public class ListSource
    {
        [XmlAttribute]
        public bool IsAPI { get; set; }

        [XmlText]
        public string Source { get; set; }
    }

    public class EnumValues
    {
        [XmlElement("Name")]
        public List<NameObj> Names { get; set; }
    }

    public class NameObj
    {
        [XmlText]
        public string Name { get; set; }
        [XmlAttribute]
        public int Value { get; set; }
    }
}
