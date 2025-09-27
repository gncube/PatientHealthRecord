using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using PatientHealthRecord.UseCases.Patients;
using PatientHealthRecord.UseCases.Patients.Get;
using NSubstitute;
using Shouldly;

namespace PatientHealthRecord.UnitTests.UseCases.Patients.Get;

public class GetPatientHandlerHandle
{
  private readonly Guid _testPatientId = Guid.NewGuid();
  private readonly IPatientRepository _repository = Substitute.For<IPatientRepository>();
  private GetPatientHandler _handler;

  public GetPatientHandlerHandle()
  {
    _handler = new GetPatientHandler(_repository);
  }

  private Patient CreateTestPatient()
  {
    return new Patient("test@example.com", "John", "Doe",
      new DateTime(1990, 1, 1), Gender.Male, "Self", null, "+1234567890");
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidPatientId()
  {
    var patient = CreateTestPatient();
    var query = new GetPatientQuery(_testPatientId);

    _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult<Patient?>(patient));

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.ShouldNotBeNull();
    result.Value.PatientId.ShouldBe(patient.PatientId.Value);
    result.Value.Email.ShouldBe(patient.Email);
    result.Value.FirstName.ShouldBe(patient.FirstName);
    result.Value.LastName.ShouldBe(patient.LastName);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenNonExistentPatientId()
  {
    var query = new GetPatientQuery(_testPatientId);

    _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult<Patient?>(null));

    var result = await _handler.Handle(query, CancellationToken.None);

    result.Status.ShouldBe(Ardalis.Result.ResultStatus.NotFound);
    result.Value.ShouldBeNull();
  }

  [Fact]
  public async Task CallsRepositoryGetByIdAsync()
  {
    var query = new GetPatientQuery(_testPatientId);

    await _handler.Handle(query, CancellationToken.None);

    await _repository.Received(1).GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task ReturnsCorrectPatientDto()
  {
    var patient = CreateTestPatient();
    var query = new GetPatientQuery(_testPatientId);

    _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult<Patient?>(patient));

    var result = await _handler.Handle(query, CancellationToken.None);

    var dto = result.Value;
    dto.PatientId.ShouldBe(patient.PatientId.Value);
    dto.Email.ShouldBe(patient.Email);
    dto.FirstName.ShouldBe(patient.FirstName);
    dto.LastName.ShouldBe(patient.LastName);
    dto.DateOfBirth.ShouldBe(patient.DateOfBirth);
    dto.Gender.ShouldBe(patient.Gender.ToString());
    dto.PhoneNumber.ShouldBe(patient.PhoneNumber);
    dto.Relationship.ShouldBe(patient.Relationship);
    dto.EmergencyContactName.ShouldBe(patient.EmergencyContactName);
    dto.EmergencyContactPhone.ShouldBe(patient.EmergencyContactPhone);
    dto.EmergencyContactRelationship.ShouldBe(patient.EmergencyContactRelationship);
    dto.BloodType.ShouldBe(patient.BloodType);
    dto.Allergies.ShouldBe(patient.Allergies);
    dto.Notes.ShouldBe(patient.Notes);
    dto.ShareWithFamily.ShouldBe(patient.ShareWithFamily);
    dto.RestrictedDataTypes.ShouldBe(patient.RestrictedDataTypes);
    dto.IsActive.ShouldBe(patient.IsActive);
    dto.CreatedAt.ShouldBe(patient.CreatedAt);
    dto.LastAccessedAt.ShouldBe(patient.LastAccessedAt);
  }
}
