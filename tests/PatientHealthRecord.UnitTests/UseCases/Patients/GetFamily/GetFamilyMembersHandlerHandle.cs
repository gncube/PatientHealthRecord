using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;
using PatientHealthRecord.UseCases.Patients;
using PatientHealthRecord.UseCases.Patients.GetFamily;
using NSubstitute;
using Shouldly;

namespace PatientHealthRecord.UnitTests.UseCases.Patients.GetFamily;

public class GetFamilyMembersHandlerHandle
{
  private readonly Guid _testFamilyId = Guid.NewGuid();
  private readonly IPatientRepository _repository = Substitute.For<IPatientRepository>();
  private GetFamilyMembersHandler _handler;

  public GetFamilyMembersHandlerHandle()
  {
    _handler = new GetFamilyMembersHandler(_repository);
  }

  private List<Patient> CreateTestFamily()
  {
    var familyId = Guid.NewGuid();
    return new List<Patient>
    {
      new Patient("parent@example.com", "John", "Doe", new DateTime(1970, 1, 1), Gender.Male, "Self", familyId),
      new Patient("spouse@example.com", "Jane", "Doe", new DateTime(1975, 1, 1), Gender.Female, "Spouse", familyId),
      new Patient("child@example.com", "Bob", "Doe", new DateTime(2000, 1, 1), Gender.Male, "Child", familyId)
    };
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidFamilyId()
  {
    var patients = CreateTestFamily();
    var query = new GetFamilyMembersQuery(_testFamilyId);

    _repository.GetFamilyMembersAsync(_testFamilyId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(patients));

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.ShouldNotBeNull();
    result.Value.Count.ShouldBe(3);
  }

  [Fact]
  public async Task ReturnsCorrectPatientSummaryDtos()
  {
    var patients = CreateTestFamily();
    var query = new GetFamilyMembersQuery(_testFamilyId);

    _repository.GetFamilyMembersAsync(_testFamilyId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(patients));

    var result = await _handler.Handle(query, CancellationToken.None);

    var familyMembers = result.Value;
    familyMembers.Count.ShouldBe(3);

    // Check first family member
    var firstMember = familyMembers[0];
    firstMember.PatientId.ShouldBe(patients[0].PatientId.Value);
    firstMember.FullName.ShouldBe("John Doe");
    firstMember.Relationship.ShouldBe("Self");
    firstMember.LastAccessed.ShouldBe(patients[0].CreatedAt);
  }

  [Fact]
  public async Task CallsRepositoryGetFamilyMembersAsync()
  {
    var query = new GetFamilyMembersQuery(_testFamilyId);

    await _handler.Handle(query, CancellationToken.None);

    await _repository.Received(1).GetFamilyMembersAsync(_testFamilyId, Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task ReturnsEmptyListWhenNoFamilyMembers()
  {
    var patients = new List<Patient>();
    var query = new GetFamilyMembersQuery(_testFamilyId);

    _repository.GetFamilyMembersAsync(_testFamilyId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(patients));

    var result = await _handler.Handle(query, CancellationToken.None);

    result.IsSuccess.ShouldBeTrue();
    result.Value.ShouldNotBeNull();
    result.Value.Count.ShouldBe(0);
  }

  [Fact]
  public async Task CalculatesAgeCorrectly()
  {
    var childPatient = new Patient("child@example.com", "Bob", "Doe",
      new DateTime(2010, 1, 1), Gender.Male, "Child", _testFamilyId);
    var patients = new List<Patient> { childPatient };
    var query = new GetFamilyMembersQuery(_testFamilyId);

    _repository.GetFamilyMembersAsync(_testFamilyId, Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(patients));

    var result = await _handler.Handle(query, CancellationToken.None);

    var childMember = result.Value[0];
    var expectedAge = DateTime.UtcNow.Year - 2010;
    childMember.Age.ShouldBe(expectedAge);
  }
}
