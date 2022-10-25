using System.Xml.Serialization;

namespace DynamicReportApi.Models
{
    [XmlRoot("InputStructure")]
    public class InputStructure
    {
        [XmlElement("Input")]
        public List<Input> Inputs { get; set; }

        public InputStructure()
        {
            Inputs = new List<Input>();
        }
    }
}
