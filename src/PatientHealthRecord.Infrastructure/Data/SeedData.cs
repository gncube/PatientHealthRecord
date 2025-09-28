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

  // Clinical data seed data
  public static readonly ClinicalObservation Observation1 = new(
    patientId: Patient1.PatientId,
    observationType: "Blood Glucose",
    value: "145",
    unit: "mg/dL",
    recordedAt: DateTime.UtcNow.AddDays(-7),
    recordedBy: "Self",
    category: ObservationCategory.Vital,
    notes: "Fasting blood glucose level"
  );

  public static readonly ClinicalObservation Observation2 = new(
    patientId: Patient2.PatientId,
    observationType: "Blood Pressure",
    value: "120/80",
    unit: "mmHg",
    recordedAt: DateTime.UtcNow.AddDays(-3),
    recordedBy: "Self",
    category: ObservationCategory.Vital,
    notes: "Prenatal check-up"
  );

  public static readonly ClinicalObservation Observation3 = new(
    patientId: Patient3.PatientId,
    observationType: "Weight",
    value: "35",
    unit: "kg",
    recordedAt: DateTime.UtcNow.AddDays(-14),
    recordedBy: "Parent",
    category: ObservationCategory.Vital,
    notes: "Monthly check-up"
  );

  public static readonly Condition Condition1 = new(
    patientId: Patient1.PatientId,
    name: "Type 2 Diabetes Mellitus",
    description: "Chronic condition requiring blood sugar management",
    onsetDate: new DateTime(2020, 6, 15),
    severity: ConditionSeverity.Moderate,
    treatment: "Metformin 500mg twice daily",
    recordedBy: "Dr. Mpho Lekwape"
  );

  public static readonly Condition Condition2 = new(
    patientId: Patient2.PatientId,
    name: "Pregnancy",
    description: "First pregnancy, currently in second trimester",
    onsetDate: new DateTime(2025, 6, 1),
    severity: ConditionSeverity.Mild,
    treatment: "Prenatal vitamins, regular check-ups",
    recordedBy: "Dr. Keabetswe Modise"
  );

  public static readonly Condition Condition3 = new(
    patientId: Patient3.PatientId,
    name: "Lactose Intolerance",
    description: "Inability to digest lactose due to lactase deficiency",
    onsetDate: new DateTime(2018, 5, 10),
    severity: ConditionSeverity.Mild,
    treatment: "Lactase enzyme supplements, lactose-free diet",
    recordedBy: "Dr. Boitumelo Ndlovu"
  );

  public static readonly Medication Medication1 = new(
    patientId: Patient1.PatientId,
    name: "Metformin",
    dosage: "500mg",
    frequency: "Twice daily with meals",
    startDate: new DateTime(2020, 6, 15),
    prescribedBy: "Dr. Mpho Lekwape",
    purpose: "Blood glucose control for Type 2 Diabetes",
    recordedBy: "Dr. Mpho Lekwape"
  );

  public static readonly Medication Medication2 = new(
    patientId: Patient2.PatientId,
    name: "Prenatal Vitamins",
    dosage: "1 tablet",
    frequency: "Once daily",
    startDate: new DateTime(2025, 6, 1),
    prescribedBy: "Dr. Keabetswe Modise",
    purpose: "Nutritional support during pregnancy",
    recordedBy: "Dr. Keabetswe Modise"
  );

  public static readonly Medication Medication3 = new(
    patientId: Patient3.PatientId,
    name: "Lactase Enzyme",
    dosage: "1-2 tablets",
    frequency: "As needed before dairy consumption",
    startDate: new DateTime(2018, 5, 10),
    prescribedBy: "Dr. Boitumelo Ndlovu",
    purpose: "Aid digestion of lactose",
    recordedBy: "Dr. Boitumelo Ndlovu"
  );

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync() ||
        await dbContext.Patients.AnyAsync() ||
        await dbContext.ClinicalObservations.AnyAsync() ||
        await dbContext.Conditions.AnyAsync() ||
        await dbContext.Medications.AnyAsync())
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

    // Save patients first to ensure their IDs are available
    await dbContext.SaveChangesAsync();

    // Add Clinical Data
    dbContext.ClinicalObservations.AddRange([Observation1, Observation2, Observation3]);
    dbContext.Conditions.AddRange([Condition1, Condition2, Condition3]);
    dbContext.Medications.AddRange([Medication1, Medication2, Medication3]);

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
