// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using PatientHealthRecord.Infrastructure.Data;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=../src/PatientHealthRecord.Web/database.sqlite")
    .Options;

using var context = new AppDbContext(options, null!);

Console.WriteLine("Checking database contents...");

var clinicalObservations = await context.ClinicalObservations.CountAsync();
var conditions = await context.Conditions.CountAsync();
var medications = await context.Medications.CountAsync();
var patients = await context.Patients.CountAsync();
var contributors = await context.Contributors.CountAsync();

Console.WriteLine($"Contributors: {contributors}");
Console.WriteLine($"Patients: {patients}");
Console.WriteLine($"Clinical Observations: {clinicalObservations}");
Console.WriteLine($"Conditions: {conditions}");
Console.WriteLine($"Medications: {medications}");

if (clinicalObservations == 0 || conditions == 0 || medications == 0)
{
    Console.WriteLine("Clinical data tables are empty! Running seed data...");

    // Run the seeding
    await PatientHealthRecord.Infrastructure.Data.SeedData.InitializeAsync(context);

    // Check again
    clinicalObservations = await context.ClinicalObservations.CountAsync();
    conditions = await context.Conditions.CountAsync();
    medications = await context.Medications.CountAsync();

    Console.WriteLine("After seeding:");
    Console.WriteLine($"Clinical Observations: {clinicalObservations}");
    Console.WriteLine($"Conditions: {conditions}");
    Console.WriteLine($"Medications: {medications}");
}
else
{
    Console.WriteLine("Clinical data tables already have data.");
}
