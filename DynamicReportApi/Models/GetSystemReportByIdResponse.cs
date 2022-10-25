namespace DynamicReportApi.Models
{
    public class GetSystemReportByIdResponse
    {
        public string ReportTitle { get; set; }
        public List<InputObj> InputStructure { get; set; }
        public List<OutputObj> OutputStructure { get; set; }

        public GetSystemReportByIdResponse()
        {
            InputStructure = new List<InputObj>();
            OutputStructure = new List<OutputObj>();
        }
    }
}
