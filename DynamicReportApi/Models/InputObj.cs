using System.Xml.Serialization;

namespace DynamicReportApi.Models
{
    public class InputObj
    {
        public long Order { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsList { get; set; }
        public bool ShowInput { get; set; }
        public List<Source> ListSource { get; set; }
        public bool IsEnum { get; set; }
        public EnumValues EnumValues { get; set; }
        public string Value { get; set; }
        public List<string> DateRanges { get; set; }

        public InputObj()
        {
            ListSource = new List<Source>();
            DateRanges = new List<string>();
        }

    }

    public class Source
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
