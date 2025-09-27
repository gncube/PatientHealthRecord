namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Response for creating a new patient
/// </summary>
public class CreatePatientResponse(Guid id, string email, string firstName, string lastName)
{
    public Guid Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
}
