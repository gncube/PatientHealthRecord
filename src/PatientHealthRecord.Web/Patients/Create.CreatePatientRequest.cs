using System.ComponentModel.DataAnnotations;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Request to create a new patient
/// </summary>
public class CreatePatientRequest
{
    public const string Route = "/Patients";

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string? Gender { get; set; }

    public string? Relationship { get; set; }

    public Guid? PrimaryContactId { get; set; }

    public string? PhoneNumber { get; set; }
}
