using PatientHealthRecord.UseCases.Patients;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Response for getting family dashboard
/// </summary>
public class GetFamilyDashboardResponse(List<PatientSummaryDto> dashboardData)
{
    public List<PatientSummaryDto> DashboardData { get; set; } = dashboardData;
}
