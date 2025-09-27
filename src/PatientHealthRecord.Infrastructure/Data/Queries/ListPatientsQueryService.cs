using PatientHealthRecord.UseCases.Patients;
using PatientHealthRecord.UseCases.Patients.List;

namespace PatientHealthRecord.Infrastructure.Data.Queries;

public class ListPatientsQueryService(AppDbContext _db) : IListPatientsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries -
  // this is just an example

  public async Task<IEnumerable<PatientDto>> ListAsync()
  {
    var result = await _db.Patients
      .Select(p => new PatientDto(
        p.PatientId.Value,
        p.Email,
        p.FirstName,
        p.LastName,
        p.DateOfBirth,
        p.Gender.ToString(),
        p.PhoneNumber,
        p.Relationship,
        p.EmergencyContactName,
        p.EmergencyContactPhone,
        p.EmergencyContactRelationship,
        p.BloodType,
        p.Allergies,
        p.Notes,
        p.ShareWithFamily,
        p.RestrictedDataTypes,
        p.IsActive,
        p.CreatedAt,
        p.LastAccessedAt))
      .ToListAsync();

    Console.WriteLine($"<<<<<<<Listed {result.Count} patients");

    return result;
  }
}
