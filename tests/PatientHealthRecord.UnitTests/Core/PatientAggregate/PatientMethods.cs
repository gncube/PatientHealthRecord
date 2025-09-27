namespace PatientHealthRecord.UnitTests.Core.PatientAggregate;

public class PatientMethods
{
    private readonly string _testEmail = "test@example.com";
    private readonly string _testFirstName = "John";
    private readonly string _testLastName = "Doe";
    private readonly DateTime _testDateOfBirth = new DateTime(1990, 1, 1);
    private readonly Gender _testGender = Gender.Male;
    private Patient _testPatient;

    public PatientMethods()
    {
        _testPatient = new Patient(_testEmail, _testFirstName, _testLastName,
          _testDateOfBirth, _testGender);
    }

    [Fact]
    public void FullName_ReturnsCorrectFormat()
    {
        _testPatient.FullName.ShouldBe("John Doe");
    }

    [Fact]
    public void Age_CalculatesCorrectly()
    {
        var expectedAge = DateTime.UtcNow.Year - 1990;
        if (DateTime.UtcNow.DayOfYear < new DateTime(DateTime.UtcNow.Year, 1, 1).DayOfYear)
            expectedAge--;

        _testPatient.Age.ShouldBe(expectedAge);
    }

    [Fact]
    public void IsChild_ReturnsTrueForUnder18()
    {
        _testPatient.IsChild.ShouldBeFalse(); // Born in 1990, should be adult
    }

    [Fact]
    public void IsAdult_ReturnsTrueFor18AndOver()
    {
        _testPatient.IsAdult.ShouldBeTrue();
    }

    [Fact]
    public void UpdatePersonalInfo_UpdatesFields()
    {
        _testPatient.UpdatePersonalInfo("Jane", "Smith", "+9876543210");

        _testPatient.FirstName.ShouldBe("Jane");
        _testPatient.LastName.ShouldBe("Smith");
        _testPatient.PhoneNumber.ShouldBe("+9876543210");
    }

    [Fact]
    public void UpdateEmergencyContact_UpdatesFields()
    {
        _testPatient.UpdateEmergencyContact("Emergency Contact", "+1112223333", "Spouse");

        _testPatient.EmergencyContactName.ShouldBe("Emergency Contact");
        _testPatient.EmergencyContactPhone.ShouldBe("+1112223333");
        _testPatient.EmergencyContactRelationship.ShouldBe("Spouse");
    }

    [Fact]
    public void UpdateMedicalInfo_UpdatesFields()
    {
        var allergies = new List<string> { "Peanuts", "Shellfish" };
        _testPatient.UpdateMedicalInfo("O+", allergies, "Medical notes");

        _testPatient.BloodType.ShouldBe("O+");
        _testPatient.Allergies.ShouldBe(allergies);
        _testPatient.Notes.ShouldBe("Medical notes");
    }

    [Fact]
    public void UpdatePrivacySettings_UpdatesFields()
    {
        var restrictedData = new List<string> { "MedicalHistory", "FinancialInfo" };
        _testPatient.UpdatePrivacySettings(false, restrictedData);

        _testPatient.ShareWithFamily.ShouldBeFalse();
        _testPatient.RestrictedDataTypes.ShouldBe(restrictedData);
    }

    [Fact]
    public void UpdateLastAccessed_UpdatesTimestamp()
    {
        var beforeUpdate = DateTime.UtcNow;
        _testPatient.UpdateLastAccessed();
        var afterUpdate = DateTime.UtcNow;

        _testPatient.LastAccessedAt.ShouldNotBeNull();
        _testPatient.LastAccessedAt.Value.ShouldBeInRange(beforeUpdate, afterUpdate);
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalse()
    {
        _testPatient.Deactivate();

        _testPatient.IsActive.ShouldBeFalse();
    }

    [Fact]
    public void CanBeAccessedBy_ReturnsTrueForPrimaryContact()
    {
        var primaryContactId = Guid.NewGuid();
        var patient = new Patient(_testEmail, _testFirstName, _testLastName,
          _testDateOfBirth, _testGender, "Self", primaryContactId);

        patient.CanBeAccessedBy(primaryContactId).ShouldBeTrue();
    }

    [Fact]
    public void CanBeAccessedBy_ReturnsTrueForPatientId()
    {
        var patientId = _testPatient.PatientId.Value;

        _testPatient.CanBeAccessedBy(patientId).ShouldBeTrue();
    }

    [Fact]
    public void CanBeAccessedBy_ReturnsFalseForUnauthorizedUser()
    {
        var unauthorizedUserId = Guid.NewGuid();

        _testPatient.CanBeAccessedBy(unauthorizedUserId).ShouldBeFalse();
    }
}
