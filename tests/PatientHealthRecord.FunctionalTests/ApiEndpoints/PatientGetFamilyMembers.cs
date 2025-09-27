using PatientHealthRecord.Infrastructure.Data;
using PatientHealthRecord.Web.Patients;

namespace PatientHealthRecord.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class PatientGetFamilyMembers(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReturnsFamilyMembersForPatient1()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyMembersResponse>(
          GetFamilyMembersRequest.BuildRoute(SeedData.Patient1.PatientId.Value));

        result.FamilyMembers.Count.ShouldBe(2); // Patient1 (self) and Patient3 (child)
        result.FamilyMembers.ShouldContain(p => p.PatientId == SeedData.Patient1.PatientId.Value);
        result.FamilyMembers.ShouldContain(p => p.PatientId == SeedData.Patient3.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsFamilyMembersForPatient3()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyMembersResponse>(
          GetFamilyMembersRequest.BuildRoute(SeedData.Patient3.PatientId.Value));

        result.FamilyMembers.Count.ShouldBe(2); // Patient1 (parent) and Patient3 (self)
        result.FamilyMembers.ShouldContain(p => p.PatientId == SeedData.Patient1.PatientId.Value);
        result.FamilyMembers.ShouldContain(p => p.PatientId == SeedData.Patient3.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsOnlySelfForPatient2()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyMembersResponse>(
          GetFamilyMembersRequest.BuildRoute(SeedData.Patient2.PatientId.Value));

        result.FamilyMembers.Count.ShouldBe(1); // Only Patient2 (self)
        result.FamilyMembers.ShouldContain(p => p.PatientId == SeedData.Patient2.PatientId.Value);
    }

    [Fact]
    public async Task ReturnsCorrectFamilyMemberDetails()
    {
        var result = await _client.GetAndDeserializeAsync<GetFamilyMembersResponse>(
          GetFamilyMembersRequest.BuildRoute(SeedData.Patient1.PatientId.Value));

        var patient1 = result.FamilyMembers.First(p => p.PatientId == SeedData.Patient1.PatientId.Value);
        patient1.FullName.ShouldBe($"{SeedData.Patient1.FirstName} {SeedData.Patient1.LastName}");
        patient1.Relationship.ShouldBe(SeedData.Patient1.Relationship);
        patient1.Age.ShouldBe(SeedData.Patient1.Age);

        var patient3 = result.FamilyMembers.First(p => p.PatientId == SeedData.Patient3.PatientId.Value);
        patient3.FullName.ShouldBe($"{SeedData.Patient3.FirstName} {SeedData.Patient3.LastName}");
        patient3.Relationship.ShouldBe(SeedData.Patient3.Relationship);
        patient3.Age.ShouldBe(SeedData.Patient3.Age);
    }
}
