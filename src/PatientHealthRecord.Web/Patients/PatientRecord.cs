namespace PatientHealthRecord.Web.Patients;

public record PatientRecord(
    Guid Id,
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
    DateTime? LastAccessedAt);
