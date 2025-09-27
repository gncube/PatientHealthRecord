namespace PatientHealthRecord.UseCases.Patients.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListPatientsQueryService
{
    Task<IEnumerable<PatientDto>> ListAsync();
}
