using System.Xml.Serialization;

namespace DynamicReportApi.Models
{
    [XmlRoot("OutputStructure")]
    public class OutputStructure
    {
        [XmlElement("OutputCoulmn")]
        public List<OutputColumn> Outputs { get; set; }
        public OutputStructure()
        {
            Outputs = new List<OutputColumn>();
        }
    }
}
