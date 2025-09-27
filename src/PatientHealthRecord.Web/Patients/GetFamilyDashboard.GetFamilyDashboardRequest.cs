namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Request to get family dashboard for a patient
/// </summary>
public class GetFamilyDashboardRequest
{
    public const string Route = "/Patients/{FamilyId:guid}/dashboard";
    public static string BuildRoute(Guid familyId) => Route.Replace("{FamilyId:guid}", familyId.ToString());

    public Guid FamilyId { get; set; }
}
