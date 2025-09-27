namespace PatientHealthRecord.UnitTests.Core.PatientAggregate;

public class PatientConstructor
{
    private readonly string _testEmail = "test@example.com";
    private readonly string _testFirstName = "John";
    private readonly string _testLastName = "Doe";
    private readonly DateTime _testDateOfBirth = new DateTime(1990, 1, 1);
    private readonly Gender _testGender = Gender.Male;
    private readonly string _testRelationship = "Self";
    private readonly Guid _testPrimaryContactId = Guid.NewGuid();
    private readonly string _testPhoneNumber = "+1234567890";
    private Patient? _testPatient;

    private Patient CreatePatient()
    {
        return new Patient(_testEmail, _testFirstName, _testLastName,
          _testDateOfBirth, _testGender, _testRelationship, _testPrimaryContactId, _testPhoneNumber);
    }

    [Fact]
    public void InitializesEmail()
    {
        _testPatient = CreatePatient();

        _testPatient.Email.ShouldBe(_testEmail);
    }

    [Fact]
    public void InitializesFirstName()
    {
        _testPatient = CreatePatient();

        _testPatient.FirstName.ShouldBe(_testFirstName);
    }

    [Fact]
    public void InitializesLastName()
    {
        _testPatient = CreatePatient();

        _testPatient.LastName.ShouldBe(_testLastName);
    }

    [Fact]
    public void InitializesDateOfBirth()
    {
        _testPatient = CreatePatient();

        _testPatient.DateOfBirth.ShouldBe(_testDateOfBirth);
    }

    [Fact]
    public void InitializesGender()
    {
        _testPatient = CreatePatient();

        _testPatient.Gender.ShouldBe(_testGender);
    }

    [Fact]
    public void InitializesRelationship()
    {
        _testPatient = CreatePatient();

        _testPatient.Relationship.ShouldBe(_testRelationship);
    }

    [Fact]
    public void InitializesPrimaryContactId()
    {
        _testPatient = CreatePatient();

        _testPatient.PrimaryContactId.ShouldBe(_testPrimaryContactId);
    }

    [Fact]
    public void InitializesPhoneNumber()
    {
        _testPatient = CreatePatient();

        _testPatient.PhoneNumber.ShouldBe(_testPhoneNumber);
    }

    [Fact]
    public void InitializesIsActiveToTrue()
    {
        _testPatient = CreatePatient();

        _testPatient.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void InitializesCreatedAt()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        _testPatient = CreatePatient();
        var after = DateTime.UtcNow.AddSeconds(1);

        _testPatient.CreatedAt.ShouldBeGreaterThanOrEqualTo(before);
        _testPatient.CreatedAt.ShouldBeLessThanOrEqualTo(after);
    }

    [Fact]
    public void GeneratesPatientId()
    {
        _testPatient = CreatePatient();

        _testPatient.PatientId.ShouldNotBeNull();
        _testPatient.PatientId.Value.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void ThrowsExceptionWhenEmailIsNull()
    {
        var exception = Should.Throw<ArgumentException>(() =>
          new Patient(null!, _testFirstName, _testLastName, _testDateOfBirth, _testGender));

        exception.Message.ShouldContain("email");
    }

    [Fact]
    public void ThrowsExceptionWhenEmailIsEmpty()
    {
        var exception = Should.Throw<ArgumentException>(() =>
          new Patient("", _testFirstName, _testLastName, _testDateOfBirth, _testGender));

        exception.Message.ShouldContain("email");
    }

    [Fact]
    public void ThrowsExceptionWhenFirstNameIsNull()
    {
        var exception = Should.Throw<ArgumentException>(() =>
          new Patient(_testEmail, null!, _testLastName, _testDateOfBirth, _testGender));

        exception.Message.ShouldContain("firstName");
    }

    [Fact]
    public void ThrowsExceptionWhenLastNameIsNull()
    {
        var exception = Should.Throw<ArgumentException>(() =>
          new Patient(_testEmail, _testFirstName, null!, _testDateOfBirth, _testGender));

        exception.Message.ShouldContain("lastName");
    }

    [Fact]
    public void ThrowsExceptionWhenDateOfBirthIsTooOld()
    {
        var tooOldDate = DateTime.UtcNow.AddYears(-151);

        var exception = Should.Throw<ArgumentOutOfRangeException>(() =>
          new Patient(_testEmail, _testFirstName, _testLastName, tooOldDate, _testGender));

        exception.Message.ShouldContain("dateOfBirth");
    }

    [Fact]
    public void ThrowsExceptionWhenDateOfBirthIsInFuture()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);

        var exception = Should.Throw<ArgumentOutOfRangeException>(() =>
          new Patient(_testEmail, _testFirstName, _testLastName, futureDate, _testGender));

        exception.Message.ShouldContain("dateOfBirth");
    }
}
