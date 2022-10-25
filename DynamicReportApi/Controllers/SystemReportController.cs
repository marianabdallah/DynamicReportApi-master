using Dapper;
using DynamicReportApi.Enums;
using DynamicReportApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace DynamicReportApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemReportController : ControllerBase
    {
        private readonly ILogger<SystemReportController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private long? ClientId { get; set; }
        private long? BranchId { get; set; }
        private long? FloorId { get; set; }
        private long? RoomId { get; set; }
        private long? UserId { get; set; }
        private string? UserName { get; set; }
        private long? LanguageId { get; set; }

        public SystemReportController(ILogger<SystemReportController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        [HttpGet("GetAllSystemReports")]
        public IEnumerable<SystemReport_s> GetAllSystemReports()
        {

            ClientId = 12;
            List<SystemReport_s> result = new();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                result = connection.Query<SystemReport_s>("SELECT * FROM SystemReport_s WHERE IsActive = 1 ").ToList();
                connection.Close();
            }

            return result;
        }

        [HttpGet("GetSystemReportById/{ReportId:long}")]
        public GetSystemReportByIdResponse GetSystemReportById(long ReportId)
        {
            var response = new GetSystemReportByIdResponse();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var systemReport = connection.QueryFirst<SystemReport_s>("SELECT * FROM SystemReport_s WHERE Id =  @Id", new { Id = ReportId });

                response.ReportTitle = systemReport.ReportTitle;

                XmlSerializer inputSerializer = new(typeof(InputStructure));
                using (TextReader reader = new StringReader(systemReport.InputStructure))
                {
                    var result = inputSerializer.Deserialize(reader) as InputStructure;

                    foreach (var input in result.Inputs)
                    {
                        if (input.IsList && input.ListSource != null && input.ListSource.IsAPI)
                        {
                            RestClient restClient = new();
                            RestRequest restRequest = new RestRequest(input.ListSource.Source, Method.Get);
                            var apiResponse = restClient.Execute(restRequest);
                            if (apiResponse.IsSuccessful)
                            {
                                var list = JsonConvert.DeserializeObject<List<Source>>(apiResponse.Content);
                                response.InputStructure.Add(new InputObj
                                {
                                    DataType = input.DataType,
                                    IsList = input.IsList,
                                    Name = input.Name,
                                    ListSource = list,
                                    Order = input.Order,
                                    ShowInput = input.ShowInput
                                });
                            }


                        }
                        else if (input.IsList && input.ListSource != null && input.ListSource.IsAPI == false)
                        {
                            var list = connection.Query<Source>(input.ListSource.Source).ToList();
                            response.InputStructure.Add(new InputObj
                            {
                                DataType = input.DataType,
                                IsList = input.IsList,
                                Name = input.Name,
                                ListSource = list,
                                Order = input.Order,
                                ShowInput = input.ShowInput
                            });
                        }
                        else if (input.DataType.ToLower() == "daterange")
                        {
                            response.InputStructure.Add(new InputObj
                            {
                                DataType = input.DataType,
                                Name = input.Name,
                                Order = input.Order,
                                ShowInput = input.ShowInput,
                                DateRanges = Enum.GetValues(typeof(DateRangeType)).Cast<DateRangeType>().Select(v => v.ToString()).ToList()

                            });

                        }
                        else
                        {
                            response.InputStructure.Add(new InputObj
                            {
                                DataType = input.DataType,
                                IsList = input.IsList,
                                Name = input.Name,
                                Order = input.Order,
                                ShowInput = input.ShowInput,
                                IsEnum = input.IsEnum,
                                EnumValues = input.Enums
                            });

                        }
                    }
                }

                XmlSerializer outputSerializer = new(typeof(OutputStructure));

                using (TextReader reader = new StringReader(systemReport.OutputStructure))
                {
                    var result = outputSerializer.Deserialize(reader) as OutputStructure;

                    foreach (var input in result.Outputs)
                    {
                        response.OutputStructure.Add(new OutputObj
                        {
                            Order = input.Order,
                            DataType = input.DataType,
                            Name = input.Name,
                            ShowInput = input.ShowInput
                        });
                    }
                }

                connection.Close();
            }

            return response;
        }

        [Route("ExecuteSystemReportById")]
        [HttpPost]
        public ExecuteSystemReportByIdResponse ExecuteSystemReportById(ExecuteSystemReportByIdRequest request)
        {
            ExecuteSystemReportByIdResponse response = new ExecuteSystemReportByIdResponse();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SystemReport_s systemReport = connection.QueryFirst<SystemReport_s>("SELECT * FROM SystemReport_s WHERE Id =  @Id", new { Id = request.ReportId });

                response.ReportTitle = systemReport.ReportTitle;

                XmlSerializer serializer = new(typeof(InputStructure));
                using (TextReader reader = new StringReader(systemReport.InputStructure))
                {
                    var result = serializer.Deserialize(reader) as InputStructure;
                    var parameters = new DynamicParameters();

                    foreach (var requestInput in request.InputObject.OrderBy(x => x.Order))
                    {

                        if (requestInput.DataType.ToLower() == "datetime")
                        {

                            parameters.Add(requestInput.Name, Convert.ToDateTime(requestInput.Value), GetDBDataType(requestInput.DataType), ParameterDirection.Input);
                        }
                        else if (requestInput.DataType.ToLower() == "daterange")
                        {
                            var fromAndTo = GetFromAndTo((DateRangeType)Enum.Parse(typeof(DateRangeType), requestInput.Value));

                            var dateRangeInputStructure = result.Inputs.First(x => x.Name == requestInput.Name);

                            parameters.Add(dateRangeInputStructure.From, fromAndTo.From, DbType.Date, ParameterDirection.Input);
                            parameters.Add(dateRangeInputStructure.To, fromAndTo.To, DbType.Date, ParameterDirection.Input);

                        }
                        else if (requestInput.DataType.ToLower() == "sessionparam")
                        {

                            switch (requestInput.Name)
                            {
                                case "ClientId":
                                    parameters.Add(requestInput.Name, ClientId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                case "BranchId":
                                    parameters.Add(requestInput.Name, BranchId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                case "FloorId":
                                    parameters.Add(requestInput.Name, FloorId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                case "UserId":
                                    parameters.Add(requestInput.Name, UserId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                case "RoomId":
                                    parameters.Add(requestInput.Name, RoomId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                case "UserName":
                                    parameters.Add(requestInput.Name, UserName, DbType.String, ParameterDirection.Input);
                                    break;
                                case "LanguageId":
                                    parameters.Add(requestInput.Name, LanguageId, DbType.Int64, ParameterDirection.Input);
                                    break;
                                default:
                                    break;
                            }


                        }
                        else
                        {
                            parameters.Add(requestInput.Name, requestInput.Value, GetDBDataType(requestInput.DataType), ParameterDirection.Input);
                        }

                    }

                    if (!systemReport.IsFunction)
                        response.ResultDataSet = connection.Query(systemReport.StoredProcedureName, parameters, commandType: CommandType.StoredProcedure).ToList();
                    else
                        response.ResultDataSet = connection.Query(PrepareFunctionSQL(systemReport.StoredProcedureName, parameters), parameters, commandType: CommandType.Text).ToList();



                    XmlSerializer outputSerializer = new(typeof(OutputStructure));
                    using TextReader outputReader = new StringReader(systemReport.OutputStructure);
                    var outputStructure = outputSerializer.Deserialize(outputReader) as OutputStructure;

                    foreach (var output in outputStructure.Outputs)
                    {
                        response.OutputStructure.Add(new OutputObj
                        {
                            Name = output.Name,
                            DataType = output.DataType,
                            Order = output.Order,
                            ShowInput = output.ShowInput
                        });
                    }
                }
                connection.Close();
            }

            return response;
        }

        private FromTo GetFromAndTo(DateRangeType dateRangeType)
        {
            FromTo fromTo = new FromTo();
            switch (dateRangeType)
            {

                case DateRangeType.Today:
                    fromTo.From = DateTime.Today;
                    fromTo.To = DateTime.Today;
                    break;
                case DateRangeType.YesterDay:
                    fromTo.From = DateTime.Today.AddDays(-1);
                    fromTo.To = DateTime.Today.AddDays(-1);
                    break;
                case DateRangeType.ThisWeek:
                    int diff = DateTime.Today.DayOfWeek - DayOfWeek.Sunday;
                    fromTo.From = DateTime.Today.AddDays(-diff);
                    fromTo.To = DateTime.Today;

                    break;
                case DateRangeType.LastWeek:
                    var endOfLastWeek = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayOfWeek.Sunday));
                    fromTo.From = endOfLastWeek.AddDays(-7);
                    fromTo.To = endOfLastWeek;
                    break;
                case DateRangeType.ThisMonth:
                    fromTo.From = DateTime.Today.AddDays(-DateTime.Today.Day);
                    fromTo.To = DateTime.Today;
                    break;
                case DateRangeType.LastMonth:
                    var endOfLastMonth = DateTime.Today.AddDays(-DateTime.Today.Day);
                    var startOfLastMonth = endOfLastMonth.AddMonths(-1);

                    fromTo.From = startOfLastMonth;
                    fromTo.To = endOfLastMonth;
                    break;
                case DateRangeType.ThisQuarter:
                    int quarterNumber = (DateTime.Today.Month - 1) / 3 + 1;
                    DateTime firstDayOfQuarter = new DateTime(DateTime.Today.Year, (quarterNumber - 1) * 3 + 1, 1);

                    fromTo.From = firstDayOfQuarter;
                    fromTo.To = DateTime.Today;
                    break;
                case DateRangeType.LastQuarter:
                    var thisQuarterStart = QuarterStart(DateTime.Today);
                    var lastQuarterEnd = thisQuarterStart.AddDays(-1);
                    var lastQuarterStart = QuarterStart(lastQuarterEnd);

                    fromTo.From = lastQuarterStart;
                    fromTo.To = lastQuarterEnd;

                    break;
                case DateRangeType.ThisYear:
                    fromTo.From = new DateTime(DateTime.Today.Year, 1, 1);
                    fromTo.To = DateTime.Today;
                    break;
                case DateRangeType.LastYear:
                    fromTo.From = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    fromTo.To = new DateTime(DateTime.Today.Year - 1, 12, 31);
                    break;
                default:
                    break;
            }

            return fromTo;
        }

        private DateTime QuarterStart(DateTime referenceDate)
        {
            int startingMonth = (referenceDate.Month - 1) / 3;
            startingMonth *= 3;
            startingMonth++;
            return new DateTime(referenceDate.Year, startingMonth, 1);
        }

        private DbType GetDBDataType(string type)
        {
            switch (type.ToLower())
            {
                case "string":
                    return DbType.String;
                case "number":
                    return DbType.Int64;
                case "boolean":
                    return DbType.Boolean;
                case "datetime":
                    return DbType.DateTime;
                default:
                    return DbType.String;
            }
        }

        private string PrepareFunctionSQL(string functionName, DynamicParameters parameters)
        {
            StringBuilder sqlStatement = new StringBuilder($"SELECT {functionName}(");

            foreach (var parameter in parameters.ParameterNames)
            {
                if (parameter != parameters.ParameterNames.Last())
                    sqlStatement.Append($"@{parameter}, ");
                else
                    sqlStatement.Append($"@{parameter});");
            }

            return sqlStatement.ToString();
        }
        public static long GetPropValue(object src, string propName)
        {
            var prop = typeof(SystemReportController).GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Instance);
            return Convert.ToInt64(prop.GetValue(src, null));
        }



    }
}