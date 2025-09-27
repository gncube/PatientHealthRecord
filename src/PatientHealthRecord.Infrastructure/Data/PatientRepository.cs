using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Infrastructure.Data;

public class PatientRepository(AppDbContext dbContext) : IPatientRepository
{
    public async Task<Patient?> GetByIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Patients
            .FirstOrDefaultAsync(p => p.PatientId.Value == patientId, cancellationToken);
    }

    public async Task<List<Patient>> GetFamilyMembersAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Patients
            .Where(p => p.PrimaryContactId == familyId || p.PatientId.Value == familyId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Patient>> GetFamilyDashboardAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Patients
            .Where(p => p.PrimaryContactId == familyId || p.PatientId.Value == familyId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        dbContext.Patients.Update(patient);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        dbContext.Patients.Remove(patient);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
