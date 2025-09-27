using PatientHealthRecord.Infrastructure.Data;
using PatientHealthRecord.Web.Patients;

namespace PatientHealthRecord.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class PatientList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReturnsThreeSeedPatients()
    {
        var result = await _client.GetAndDeserializeAsync<PatientListResponse>("/Patients");

        result.Patients.Count.ShouldBe(3);
        result.Patients.ShouldContain(p => p.Email == SeedData.Patient1.Email);
        result.Patients.ShouldContain(p => p.Email == SeedData.Patient2.Email);
        result.Patients.ShouldContain(p => p.Email == SeedData.Patient3.Email);
    }

    [Fact]
    public async Task ReturnsPatientsWithCorrectMedicalInfo()
    {
        var result = await _client.GetAndDeserializeAsync<PatientListResponse>("/Patients");

        var patient1 = result.Patients.First(p => p.Email == SeedData.Patient1.Email);
        patient1.BloodType.ShouldBe("O+");
        patient1.Allergies.ShouldContain("Penicillin");
        patient1.Allergies.ShouldContain("Shellfish");
        patient1.Notes.ShouldContain("Type 2 Diabetes");

        var patient2 = result.Patients.First(p => p.Email == SeedData.Patient2.Email);
        patient2.BloodType.ShouldBe("A-");
        patient2.Allergies.ShouldContain("Latex");
        patient2.Notes.ShouldContain("Pregnant");

        var patient3 = result.Patients.First(p => p.Email == SeedData.Patient3.Email);
        patient3.BloodType.ShouldBe("B+");
        patient3.Allergies.ShouldContain("Dairy products");
        patient3.Notes.ShouldContain("Lactose intolerant");
    }
}
