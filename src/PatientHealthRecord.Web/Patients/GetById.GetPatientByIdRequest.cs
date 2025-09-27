namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Request to get a patient by ID
/// </summary>
public class GetPatientByIdRequest
{
    public const string Route = "/Patients/{PatientId:guid}";
    public static string BuildRoute(Guid patientId) => Route.Replace("{PatientId:guid}", patientId.ToString());

    public Guid PatientId { get; set; }
}
