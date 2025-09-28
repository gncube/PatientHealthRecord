using PatientHealthRecord.Core.ClinicalDataAggregate;
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
    phoneNumber: null, // Child may not have own phone
    primaryContactId: Patient1.PatientId.Value // Child belongs to Patient1's family
  );

  // Clinical data seed data - created dynamically after patients are saved
  // (Removed static readonly fields to avoid PatientId mismatch issues)

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    // Seed each type of data independently if the table is empty
    if (!await dbContext.Contributors.AnyAsync())
    {
      dbContext.Contributors.AddRange([Contributor1, Contributor2]);
      await dbContext.SaveChangesAsync();
    }

    if (!await dbContext.Patients.AnyAsync())
    {
      ConfigurePatientMedicalInfo();
      dbContext.Patients.AddRange([Patient1, Patient2, Patient3]);
      await dbContext.SaveChangesAsync();
    }

    // Get the actual PatientId values from the database
    var patient1 = await dbContext.Patients.FirstOrDefaultAsync(p => p.Email == Patient1.Email);
    var patient2 = await dbContext.Patients.FirstOrDefaultAsync(p => p.Email == Patient2.Email);
    var patient3 = await dbContext.Patients.FirstOrDefaultAsync(p => p.Email == Patient3.Email);

    if (patient1 != null && patient2 != null && patient3 != null)
    {
      if (!await dbContext.ClinicalObservations.AnyAsync())
      {
        var observations = new[]
        {
          new ClinicalObservation(
            patientId: patient1.PatientId,
            observationType: "Blood Glucose",
            value: "145",
            unit: "mg/dL",
            recordedAt: DateTime.UtcNow.AddDays(-7),
            recordedBy: "Self",
            category: ObservationCategory.Vital,
            notes: "Fasting blood glucose level"
          ),
          new ClinicalObservation(
            patientId: patient2.PatientId,
            observationType: "Blood Pressure",
            value: "120/80",
            unit: "mmHg",
            recordedAt: DateTime.UtcNow.AddDays(-3),
            recordedBy: "Self",
            category: ObservationCategory.Vital,
            notes: "Prenatal check-up"
          ),
          new ClinicalObservation(
            patientId: patient3.PatientId,
            observationType: "Weight",
            value: "35",
            unit: "kg",
            recordedAt: DateTime.UtcNow.AddDays(-14),
            recordedBy: "Parent",
            category: ObservationCategory.Vital,
            notes: "Monthly check-up"
          )
        };
        dbContext.ClinicalObservations.AddRange(observations);
        await dbContext.SaveChangesAsync();
      }

      if (!await dbContext.Conditions.AnyAsync())
      {
        var conditions = new[]
        {
          new Condition(
            patientId: patient1.PatientId,
            name: "Type 2 Diabetes Mellitus",
            description: "Chronic condition requiring blood sugar management",
            onsetDate: new DateTime(2020, 6, 15),
            severity: ConditionSeverity.Moderate,
            treatment: "Metformin 500mg twice daily",
            recordedBy: "Dr. Mpho Lekwape"
          ),
          new Condition(
            patientId: patient2.PatientId,
            name: "Pregnancy",
            description: "First pregnancy, currently in second trimester",
            onsetDate: new DateTime(2025, 6, 1),
            severity: ConditionSeverity.Mild,
            treatment: "Prenatal vitamins, regular check-ups",
            recordedBy: "Dr. Keabetswe Modise"
          ),
          new Condition(
            patientId: patient3.PatientId,
            name: "Lactose Intolerance",
            description: "Inability to digest lactose due to lactase deficiency",
            onsetDate: new DateTime(2018, 5, 10),
            severity: ConditionSeverity.Mild,
            treatment: "Lactase enzyme supplements, lactose-free diet",
            recordedBy: "Dr. Boitumelo Ndlovu"
          )
        };
        dbContext.Conditions.AddRange(conditions);
        await dbContext.SaveChangesAsync();
      }

      if (!await dbContext.Medications.AnyAsync())
      {
        var medications = new[]
        {
          new Medication(
            patientId: patient1.PatientId,
            name: "Metformin",
            dosage: "500mg",
            frequency: "Twice daily with meals",
            startDate: new DateTime(2020, 6, 15),
            prescribedBy: "Dr. Mpho Lekwape",
            purpose: "Blood glucose control for Type 2 Diabetes",
            recordedBy: "Dr. Mpho Lekwape"
          ),
          new Medication(
            patientId: patient2.PatientId,
            name: "Prenatal Vitamins",
            dosage: "1 tablet",
            frequency: "Once daily",
            startDate: new DateTime(2025, 6, 1),
            prescribedBy: "Dr. Keabetswe Modise",
            purpose: "Nutritional support during pregnancy",
            recordedBy: "Dr. Keabetswe Modise"
          ),
          new Medication(
            patientId: patient3.PatientId,
            name: "Lactase Enzyme",
            dosage: "1-2 tablets",
            frequency: "As needed before dairy consumption",
            startDate: new DateTime(2018, 5, 10),
            prescribedBy: "Dr. Boitumelo Ndlovu",
            purpose: "Aid digestion of lactose",
            recordedBy: "Dr. Boitumelo Ndlovu"
          )
        };
        dbContext.Medications.AddRange(medications);
        await dbContext.SaveChangesAsync();
      }
    }
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
