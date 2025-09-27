using PatientHealthRecord.Infrastructure.Data;
using PatientHealthRecord.Web.Patients;

namespace PatientHealthRecord.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class PatientGetById(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReturnsSeedPatientGivenId()
    {
        var result = await _client.GetAndDeserializeAsync<PatientRecord>(GetPatientByIdRequest.BuildRoute(SeedData.Patient1.PatientId.Value));

        result.Id.ShouldBe(SeedData.Patient1.PatientId.Value);
        result.Email.ShouldBe(SeedData.Patient1.Email);
        result.FirstName.ShouldBe(SeedData.Patient1.FirstName);
        result.LastName.ShouldBe(SeedData.Patient1.LastName);
        result.Gender.ShouldBe(SeedData.Patient1.Gender.ToString());
        result.PhoneNumber.ShouldBe(SeedData.Patient1.PhoneNumber);
        result.Relationship.ShouldBe(SeedData.Patient1.Relationship);
        result.IsActive.ShouldBe(true);
    }

    [Fact]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        string route = GetPatientByIdRequest.BuildRoute(Guid.NewGuid());
        _ = await _client.GetAndEnsureNotFoundAsync(route);
    }
}
