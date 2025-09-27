using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Create;

/// <summary>
/// Create a new Patient.
/// </summary>
public record CreatePatientCommand(
  string Email,
  string FirstName,
  string LastName,
  DateTime DateOfBirth,
  string Gender,
  string? Relationship,
  Guid? PrimaryContactId,
  string? PhoneNumber
) : Ardalis.SharedKernel.ICommand<Result<Guid>>;
