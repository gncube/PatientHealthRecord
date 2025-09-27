using Ardalis.Result;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.UseCases.Patients.Create;

public record CreatePatientCommand(
  string Email,
  string FirstName,
  string LastName,
  DateTime DateOfBirth,
  Gender Gender,
  string Relationship = "Self",
  Guid? PrimaryContactId = null,
  string? PhoneNumber = null,
  string? EmergencyContactName = null,
  string? EmergencyContactPhone = null,
  string? BloodType = null,
  List<string>? Allergies = null
) : Ardalis.SharedKernel.ICommand<Result<PatientDto>>;
