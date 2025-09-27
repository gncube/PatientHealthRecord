namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Request to get family members for a patient
/// </summary>
public class GetFamilyMembersRequest
{
    public const string Route = "/Patients/{FamilyId:guid}/family";
    public static string BuildRoute(Guid familyId) => Route.Replace("{FamilyId:guid}", familyId.ToString());

    public Guid FamilyId { get; set; }
}
