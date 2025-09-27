namespace PatientHealthRecord.UseCases.Patients;

public record PatientDto(
  Guid PatientId,
  string Email,
  string FirstName,
  string LastName,
  DateTime DateOfBirth,
  string Gender,
  string? PhoneNumber,
  string? Relationship,
  string? EmergencyContactName,
  string? EmergencyContactPhone,
  string? EmergencyContactRelationship,
  string? BloodType,
  List<string>? Allergies,
  string? Notes,
  bool ShareWithFamily,
  List<string>? RestrictedDataTypes,
  bool IsActive,
  DateTime CreatedAt,
  DateTime? LastAccessedAt
)
{
  public string FullName => $"{FirstName} {LastName}";
  public int Age => DateTime.UtcNow.Year - DateOfBirth.Year -
    (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
  public bool IsChild => Age < 18;
};

public record PatientSummaryDto(
  Guid PatientId,
  string FullName,
  int Age,
  string Relationship,
  DateTime LastAccessed
);
