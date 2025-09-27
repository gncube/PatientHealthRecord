using PatientHealthRecord.Infrastructure.Data;
using PatientHealthRecord.Web.Patients;

namespace PatientHealthRecord.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class PatientGetFamilyDashboard(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReturnsFamilyDashboardForPatient1()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyDashboardResponse>(
          GetFamilyDashboardRequest.BuildRoute(SeedData.Patient1.PatientId.Value));

        result.DashboardData.Count.ShouldBe(2); // Patient1 (self) and Patient3 (child)
        result.DashboardData.ShouldContain(p => p.Id == SeedData.Patient1.PatientId.Value);
        result.DashboardData.ShouldContain(p => p.Id == SeedData.Patient3.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsFamilyDashboardForPatient3()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyDashboardResponse>(
          GetFamilyDashboardRequest.BuildRoute(SeedData.Patient3.PatientId.Value));

        result.DashboardData.Count.ShouldBe(2); // Patient1 (parent) and Patient3 (self)
        result.DashboardData.ShouldContain(p => p.Id == SeedData.Patient1.PatientId.Value);
        result.DashboardData.ShouldContain(p => p.Id == SeedData.Patient3.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsOnlySelfDashboardForPatient2()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyDashboardResponse>(
          GetFamilyDashboardRequest.BuildRoute(SeedData.Patient2.PatientId.Value));

        result.DashboardData.Count.ShouldBe(1); // Only Patient2 (self)
        result.DashboardData.ShouldContain(p => p.Id == SeedData.Patient2.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsCorrectDashboardData()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyDashboardResponse>(
          GetFamilyDashboardRequest.BuildRoute(SeedData.Patient1.PatientId.Value));

        var patient1 = result.DashboardData.First(p => p.Id == SeedData.Patient1.PatientId.Value);
        patient1.FirstName.ShouldBe(SeedData.Patient1.FirstName);
        patient1.LastName.ShouldBe(SeedData.Patient1.LastName);
        patient1.Email.ShouldBe(SeedData.Patient1.Email);
        patient1.Relationship.ShouldBe(SeedData.Patient1.Relationship);
        patient1.Age.ShouldBe(SeedData.Patient1.Age);

        var patient3 = result.DashboardData.First(p => p.Id == SeedData.Patient3.PatientId.Value);
        patient3.FirstName.ShouldBe(SeedData.Patient3.FirstName);
        patient3.LastName.ShouldBe(SeedData.Patient3.LastName);
        patient3.Email.ShouldBe(SeedData.Patient3.Email);
        patient3.Relationship.ShouldBe(SeedData.Patient3.Relationship);
        patient3.Age.ShouldBe(SeedData.Patient3.Age);
    }

    [Fact]
    public async Task ReturnsNotFoundForNonExistentFamily()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.GetAsync(GetFamilyDashboardRequest.BuildRoute(nonExistentId));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
