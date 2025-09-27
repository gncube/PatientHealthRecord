using Ardalis.SharedKernel;
using PatientHealthRecord.UseCases.Patients;

namespace PatientHealthRecord.UseCases.Patients.List;

public record ListPatientsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<PatientDto>>>;
