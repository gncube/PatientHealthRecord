using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using PatientHealthRecord.UseCases.Patients.Create;
using Ardalis.SharedKernel;
using NSubstitute;
using Shouldly;

namespace PatientHealthRecord.UnitTests.UseCases.Patients.Create;

public class CreatePatientHandlerHandle
{
  private readonly string _testEmail = "test@example.com";
  private readonly string _testFirstName = "John";
  private readonly string _testLastName = "Doe";
  private readonly DateTime _testDateOfBirth = new DateTime(1990, 1, 1);
  private readonly string _testGender = "Male";
  private readonly string _testRelationship = "Self";
  private readonly Guid _testPrimaryContactId = Guid.NewGuid();
  private readonly string _testPhoneNumber = "+1234567890";
  private readonly IRepository<Patient> _repository = Substitute.For<IRepository<Patient>>();
  private CreatePatientHandler _handler;

  public CreatePatientHandlerHandle()
  {
    _handler = new CreatePatientHandler(_repository);
  }

  private Patient CreatePatient()
  {
    return new Patient(_testEmail, _testFirstName, _testLastName,
      _testDateOfBirth, Gender.Male, _testRelationship, _testPrimaryContactId, _testPhoneNumber);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidCommand()
  {
    var command = new CreatePatientCommand(_testEmail, _testFirstName, _testLastName,
      _testDateOfBirth, _testGender, _testRelationship, _testPrimaryContactId, _testPhoneNumber);

    var expectedPatient = CreatePatient();
    _repository.AddAsync(Arg.Any<Patient>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(expectedPatient));

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.ShouldBe(expectedPatient.PatientId.Value);
  }

  [Fact]
  public async Task CallsRepositoryAddAsync()
  {
    var command = new CreatePatientCommand(_testEmail, _testFirstName, _testLastName,
      _testDateOfBirth, _testGender, _testRelationship, _testPrimaryContactId, _testPhoneNumber);

    _repository.AddAsync(Arg.Any<Patient>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreatePatient()));

    await _handler.Handle(command, CancellationToken.None);

    await _repository.Received(1).AddAsync(Arg.Any<Patient>(), Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task CreatesPatientWithCorrectProperties()
  {
    var command = new CreatePatientCommand(_testEmail, _testFirstName, _testLastName,
      _testDateOfBirth, _testGender, _testRelationship, _testPrimaryContactId, _testPhoneNumber);

    Patient? capturedPatient = null;
    _repository.AddAsync(Arg.Do<Patient>(p => capturedPatient = p), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreatePatient()));

    await _handler.Handle(command, CancellationToken.None);

    capturedPatient.ShouldNotBeNull();
    capturedPatient.Email.ShouldBe(_testEmail);
    capturedPatient.FirstName.ShouldBe(_testFirstName);
    capturedPatient.LastName.ShouldBe(_testLastName);
    capturedPatient.DateOfBirth.ShouldBe(_testDateOfBirth);
    capturedPatient.Gender.ShouldBe(Gender.Male);
    capturedPatient.Relationship.ShouldBe(_testRelationship);
    capturedPatient.PrimaryContactId.ShouldBe(_testPrimaryContactId);
    capturedPatient.PhoneNumber.ShouldBe(_testPhoneNumber);
  }

  [Fact]
  public async Task UsesDefaultRelationshipWhenNull()
  {
    var command = new CreatePatientCommand(_testEmail, _testFirstName, _testLastName,
      _testDateOfBirth, _testGender, null, _testPrimaryContactId, _testPhoneNumber);

    Patient? capturedPatient = null;
    _repository.AddAsync(Arg.Do<Patient>(p => capturedPatient = p), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreatePatient()));

    await _handler.Handle(command, CancellationToken.None);

    capturedPatient.ShouldNotBeNull();
    capturedPatient.Relationship.ShouldBe("Self");
  }
}
