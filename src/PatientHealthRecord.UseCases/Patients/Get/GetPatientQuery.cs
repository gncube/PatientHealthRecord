using System;
using Ardalis.SharedKernel;

namespace PatientHealthRecord.UseCases.Patients.Get;

public record GetPatientQuery(Guid PatientId) : IQuery<Result<PatientDto>>;
