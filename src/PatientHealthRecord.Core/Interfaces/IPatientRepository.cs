using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<List<Patient>> GetFamilyMembersAsync(Guid familyId, CancellationToken cancellationToken = default);
    Task<List<Patient>> GetFamilyDashboardAsync(Guid familyId, CancellationToken cancellationToken = default);
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default);
}
