using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Patients;
using MediatR;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Get;

/// <summary>
/// Get a Patient by ID.
/// </summary>
public record GetPatientQuery(Guid PatientId) : Ardalis.SharedKernel.IQuery<Result<PatientDto>>;
