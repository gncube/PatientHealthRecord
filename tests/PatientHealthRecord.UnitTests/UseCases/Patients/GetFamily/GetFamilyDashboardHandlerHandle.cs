using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.UseCases.Patients;
using PatientHealthRecord.UseCases.Patients.GetFamily;
using NSubstitute;
using Shouldly;

namespace PatientHealthRecord.UnitTests.UseCases.Patients.GetFamily;

public class GetFamilyDashboardHandlerHandle
{
    private readonly Guid _testPatientId = Guid.NewGuid();
    private readonly IPatientRepository _repository = Substitute.For<IPatientRepository>();
    private GetFamilyDashboardHandler _handler;

    public GetFamilyDashboardHandlerHandle()
    {
        _handler = new GetFamilyDashboardHandler(_repository);
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
    public async Task ReturnsSuccessGivenValidPatientId()
    {
        var patients = CreateTestFamily();
        var primaryPatient = patients[0]; // Self
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(primaryPatient));
        _repository.GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>())
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
        var primaryPatient = patients[0]; // Self
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(primaryPatient));
        _repository.GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult(patients));

        var result = await _handler.Handle(query, CancellationToken.None);

        var familyDashboard = result.Value;
        familyDashboard.Count.ShouldBe(3);

        // Check first family member (should be Self)
        var selfMember = familyDashboard.First(p => p.Relationship == "Self");
        selfMember.PatientId.ShouldBe(patients[0].PatientId.Value);
        selfMember.FullName.ShouldBe("John Doe");
        selfMember.Relationship.ShouldBe("Self");
        selfMember.LastAccessed.ShouldBe(patients[0].CreatedAt);
    }

    [Fact]
    public async Task CallsRepositoryMethods()
    {
        var primaryPatient = new Patient("test@example.com", "Test", "User", new DateTime(1980, 1, 1), Gender.Male, "Self", Guid.NewGuid());
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(primaryPatient));
        _repository.GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult(new List<Patient>()));

        await _handler.Handle(query, CancellationToken.None);

        await _repository.Received(1).GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>());
        await _repository.Received(1).GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnsNotFoundWhenPatientDoesNotExist()
    {
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(null));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Status.ShouldBe(Ardalis.Result.ResultStatus.NotFound);
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task ReturnsEmptyListWhenNoFamilyMembers()
    {
        var primaryPatient = new Patient("test@example.com", "Test", "User", new DateTime(1980, 1, 1), Gender.Male, "Self", Guid.NewGuid());
        var patients = new List<Patient>();
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(primaryPatient));
        _repository.GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult(patients));

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(0);
    }

    [Fact]
    public async Task CalculatesAgeCorrectly()
    {
        var primaryPatient = new Patient("parent@example.com", "John", "Doe", new DateTime(1970, 1, 1), Gender.Male, "Self", Guid.NewGuid());
        var childPatient = new Patient("child@example.com", "Bob", "Doe",
          new DateTime(2010, 1, 1), Gender.Male, "Child", Guid.NewGuid());
        var patients = new List<Patient> { primaryPatient, childPatient };
        var query = new GetFamilyDashboardQuery(_testPatientId);

        _repository.GetByIdAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult<Patient?>(primaryPatient));
        _repository.GetFamilyDashboardAsync(_testPatientId, Arg.Any<CancellationToken>())
          .Returns(Task.FromResult(patients));

        var result = await _handler.Handle(query, CancellationToken.None);

        var childMember = result.Value.First(p => p.Relationship == "Child");
        var expectedAge = DateTime.UtcNow.Year - 2010;
        childMember.Age.ShouldBe(expectedAge);
    }
}
