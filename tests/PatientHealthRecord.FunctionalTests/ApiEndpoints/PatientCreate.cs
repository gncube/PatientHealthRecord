using PatientHealthRecord.Web.Patients;

namespace PatientHealthRecord.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class PatientCreate(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreatesPatientWithValidData()
    {
        var request = new CreatePatientRequest
        {
            Email = "new.patient@example.com",
            FirstName = "New",
            LastName = "Patient",
            DateOfBirth = new DateTime(1995, 5, 15),
            Gender = "Female",
            PhoneNumber = "+267 74 567 890",
            Relationship = "Self"
        };

        var result = await _client.PostAndDeserializeAsync<CreatePatientResponse>("/Patients", request);

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(Guid.Empty);
        result.Email.ShouldBe("new.patient@example.com");
        result.FirstName.ShouldBe("New");
        result.LastName.ShouldBe("Patient");
    }

    [Fact]
    public async Task ReturnsBadRequestWithInvalidData()
    {
        var request = new CreatePatientRequest
        {
            // Missing required fields
            Email = null,
            FirstName = null,
            LastName = null,
            DateOfBirth = default,
            Gender = null
        };

        var response = await _client.PostAsync("/Patients", request.ToJsonContent());
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ReturnsBadRequestWithInvalidEmail()
    {
        var request = new CreatePatientRequest
        {
            Email = "invalid-email",
            FirstName = "Test",
            LastName = "User",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Male"
        };

        var response = await _client.PostAsync("/Patients", request.ToJsonContent());
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
