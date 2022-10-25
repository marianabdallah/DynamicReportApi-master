using DynamicReportApi.Enums;

namespace DynamicReportApi.Models
{
    public class ExecuteSystemReportByIdRequest
    {
        public ExecuteSystemReportByIdRequest()
        {
            InputObject = new List<InputExecute>();
        }
        public long ReportId { get; set; }
        public List<InputExecute> InputObject { get; set; }
    }

    public class InputExecute
    {
        public long Order { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string? Value { get; set; }
    }
}
