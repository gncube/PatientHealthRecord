using PatientHealthRecord.Core.Entities.PatientAggregate.Events;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.Core.PatientAggregate;

public class Patient : EntityBase, IAggregateRoot
{
  public PatientId PatientId { get; private set; }
  public string Email { get; private set; } = string.Empty;
  public string FirstName { get; private set; } = string.Empty;
  public string LastName { get; private set; } = string.Empty;
  public DateTime DateOfBirth { get; private set; }
  public Gender Gender { get; private set; }
  public string? PhoneNumber { get; private set; }
  public string? Relationship { get; private set; } // Self, Spouse, Child, Parent, etc.
  public Guid? PrimaryContactId { get; private set; } // Links to the family member who manages this record
  public bool IsActive { get; private set; } = true;
  public DateTime CreatedAt { get; private set; }
  public DateTime? LastAccessedAt { get; private set; }

  // Emergency Contact Information
  public string? EmergencyContactName { get; private set; }
  public string? EmergencyContactPhone { get; private set; }
  public string? EmergencyContactRelationship { get; private set; }

  // Medical Information
  public string? BloodType { get; private set; }
  public List<string> Allergies { get; private set; } = new();
  public string? Notes { get; private set; }

  // Privacy settings for family sharing
  public bool ShareWithFamily { get; private set; } = true;
  public List<string> RestrictedDataTypes { get; private set; } = new();

  // Private constructor for EF Core
  private Patient()
  {
    PatientId = new PatientId(Guid.NewGuid());
  }

  public Patient(string email, string firstName, string lastName,
    DateTime dateOfBirth, Gender gender, string relationship = "Self",
    Guid? primaryContactId = null, string? phoneNumber = null)
  {
    PatientId = new PatientId(Guid.NewGuid());
    Email = Guard.Against.NullOrEmpty(email, nameof(email));
    FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
    LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
    DateOfBirth = Guard.Against.OutOfRange(dateOfBirth, nameof(dateOfBirth),
      DateTime.UtcNow.AddYears(-150), DateTime.UtcNow);
    Gender = gender;
    Relationship = relationship;
    PrimaryContactId = primaryContactId;
    PhoneNumber = phoneNumber;
    CreatedAt = DateTime.UtcNow;
    IsActive = true;

    var domainEvent = new PatientRegisteredDomainEvent(this);
    RegisterDomainEvent(domainEvent);
  }

  public void UpdatePersonalInfo(string firstName, string lastName, string? phoneNumber = null)
  {
    FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
    LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
    PhoneNumber = phoneNumber;
  }

  public void UpdateEmergencyContact(string name, string phone, string relationship)
  {
    EmergencyContactName = Guard.Against.NullOrEmpty(name, nameof(name));
    EmergencyContactPhone = Guard.Against.NullOrEmpty(phone, nameof(phone));
    EmergencyContactRelationship = Guard.Against.NullOrEmpty(relationship, nameof(relationship));
  }

  public void UpdateMedicalInfo(string? bloodType, List<string>? allergies, string? notes)
  {
    BloodType = bloodType;
    if (allergies != null) Allergies = allergies;
    Notes = notes;
  }

  public void UpdatePrivacySettings(bool shareWithFamily, List<string>? restrictedDataTypes = null)
  {
    ShareWithFamily = shareWithFamily;
    if (restrictedDataTypes != null) RestrictedDataTypes = restrictedDataTypes;
  }

  public void UpdateLastAccessed()
  {
    LastAccessedAt = DateTime.UtcNow;
  }

  public void Deactivate()
  {
    IsActive = false;
  }

  public string FullName => $"{FirstName} {LastName}";

  public int Age => DateTime.UtcNow.Year - DateOfBirth.Year -
    (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

  public bool IsChild => Age < 18;
  public bool IsAdult => Age >= 18;

  // Family relationship helpers
  public bool CanBeAccessedBy(Guid userId)
  {
    return PrimaryContactId == userId || PatientId.Value == userId;
  }
}
