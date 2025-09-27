using PatientHealthRecord.Core.ContributorAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.Infrastructure.Data;

public static class SeedData
{
  public static readonly Contributor Contributor1 = new("Ardalis");
  public static readonly Contributor Contributor2 = new("Snowfrog");

  // Patient seed data with Botswana names
  public static readonly Patient Patient1 = new(
    email: "thabo.molefe@example.com",
    firstName: "Thabo",
    lastName: "Molefe",
    dateOfBirth: new DateTime(1985, 3, 15),
    gender: Gender.Male,
    relationship: "Self",
    phoneNumber: "+267 71 234 567"
  );

  public static readonly Patient Patient2 = new(
    email: "neo.kgositsile@example.com",
    firstName: "Neo",
    lastName: "Kgositsile",
    dateOfBirth: new DateTime(1992, 8, 22),
    gender: Gender.Female,
    relationship: "Self",
    phoneNumber: "+267 72 345 678"
  );

  public static readonly Patient Patient3 = new(
    email: "kagiso.segwabe@example.com",
    firstName: "Kagiso",
    lastName: "Segwabe",
    dateOfBirth: new DateTime(2010, 11, 5), // Child patient
    gender: Gender.Male,
    relationship: "Child",
    phoneNumber: null // Child may not have own phone
  );

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync() || await dbContext.Patients.AnyAsync())
      return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    // Add Contributors
    dbContext.Contributors.AddRange([Contributor1, Contributor2]);

    // Add Patients with medical information
    ConfigurePatientMedicalInfo();
    dbContext.Patients.AddRange([Patient1, Patient2, Patient3]);

    await dbContext.SaveChangesAsync();
  }

  private static void ConfigurePatientMedicalInfo()
  {
    // Configure Patient1 (Thabo Molefe) - Adult with medical history
    Patient1.UpdateEmergencyContact(
      "Mpho Molefe",
      "+267 71 987 654",
      "Spouse"
    );

    Patient1.UpdateMedicalInfo(
      bloodType: "O+",
      allergies: ["Penicillin", "Shellfish"],
      notes: "Type 2 Diabetes - managed with Metformin. Regular check-ups required."
    );

    // Configure Patient2 (Neo Kgositsile) - Adult female
    Patient2.UpdateEmergencyContact(
      "Keabetswe Kgositsile",
      "+267 72 876 543",
      "Sister"
    );

    Patient2.UpdateMedicalInfo(
      bloodType: "A-",
      allergies: ["Latex"],
      notes: "Pregnant - Due date: March 2026. Regular prenatal care scheduled."
    );

    // Configure Patient3 (Kagiso Segwabe) - Child patient
    Patient3.UpdateEmergencyContact(
      "Boitumelo Segwabe",
      "+267 73 456 789",
      "Mother"
    );

    Patient3.UpdateMedicalInfo(
      bloodType: "B+",
      allergies: ["Dairy products"],
      notes: "Lactose intolerant. Up-to-date with childhood vaccinations."
    );

    // Set family relationship for child
    Patient3.UpdatePrivacySettings(
      shareWithFamily: true,
      restrictedDataTypes: null // Full access for parents
    );
  }
}
