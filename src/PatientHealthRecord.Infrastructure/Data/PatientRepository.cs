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
        // Get the primary patient to find their primary contact
        var primaryPatient = await GetByIdAsync(familyId, cancellationToken);
        if (primaryPatient == null)
        {
            return new List<Patient>();
        }

        // Find all family members:
        // 1. The patient themselves
        // 2. All patients who have this patient as their primary contact (children)
        // 3. The primary contact of this patient (parent)
        var familyMemberIds = new List<Guid> { familyId };

        // Add children (patients who have this patient as primary contact)
        var children = await dbContext.Patients
            .Where(p => p.PrimaryContactId == familyId)
            .Select(p => p.PatientId.Value)
            .ToListAsync(cancellationToken);
        familyMemberIds.AddRange(children);

        // Add parent (if this patient has a primary contact)
        if (primaryPatient.PrimaryContactId.HasValue)
        {
            familyMemberIds.Add(primaryPatient.PrimaryContactId.Value);
        }

        return await dbContext.Patients
            .Where(p => familyMemberIds.Contains(p.PatientId.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Patient>> GetFamilyDashboardAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        // Get the primary patient to find their primary contact
        var primaryPatient = await GetByIdAsync(familyId, cancellationToken);
        if (primaryPatient == null)
        {
            return new List<Patient>();
        }

        // Find all family members for dashboard:
        // 1. The patient themselves
        // 2. All patients who have this patient as their primary contact (children)
        // 3. The primary contact of this patient (parent)
        var familyMemberIds = new List<Guid> { familyId };

        // Add children (patients who have this patient as primary contact)
        var children = await dbContext.Patients
            .Where(p => p.PrimaryContactId == familyId)
            .Select(p => p.PatientId.Value)
            .ToListAsync(cancellationToken);
        familyMemberIds.AddRange(children);

        // Add parent (if this patient has a primary contact)
        if (primaryPatient.PrimaryContactId.HasValue)
        {
            familyMemberIds.Add(primaryPatient.PrimaryContactId.Value);
        }

        return await dbContext.Patients
            .Where(p => familyMemberIds.Contains(p.PatientId.Value))
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
