namespace DynamicReportApi.Models
{
    public class SystemReport_s
    {
        public int Id { get; set; }
        public string ReportTitle { get; set; }
        public bool IsFunction { get; set; }
        public string StoredProcedureName { get; set; }
        public string InputStructure { get; set; }
        public string OutputStructure { get; set; }
        public bool IsActive { get; set; }

    }
}
