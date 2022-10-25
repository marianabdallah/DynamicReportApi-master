namespace DynamicReportApi.Models
{
    public class ExecuteSystemReportByIdResponse
    {
        public ExecuteSystemReportByIdResponse()
        {
            OutputStructure = new List<OutputObj>();
        }
        public string ReportTitle { get; set; }
        public List<OutputObj> OutputStructure { get; set; }
        public List<dynamic> ResultDataSet { get; set; }
    }
}
