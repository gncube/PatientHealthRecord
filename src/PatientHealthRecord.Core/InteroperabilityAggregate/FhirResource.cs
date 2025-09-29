using PatientHealthRecord.Core.InteroperabilityAggregate.Events;
using PatientHealthRecord.Core.PatientAggregate;
using Ardalis.SharedKernel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace PatientHealthRecord.Core.InteroperabilityAggregate;

public class FhirResource : EntityBase, IAggregateRoot
{
  public string ResourceType { get; private set; } = string.Empty;
  public string ResourceId { get; private set; } = string.Empty;
  public string VersionId { get; private set; } = string.Empty;
  public PatientId PatientId { get; private set; }
  public string Content { get; private set; } = string.Empty; // JSON serialized FHIR resource
  public FhirResourceStatus Status { get; private set; }
  public DateTime LastUpdated { get; private set; }
  public string? Source { get; private set; } // Source system or "Family App"
  public Dictionary<string, string> Metadata { get; private set; } = new();

  // Private constructor for EF Core
  private FhirResource()
  {
    PatientId = new PatientId(Guid.Empty);
  }

  public FhirResource(string resourceType, string resourceId, PatientId patientId,
      string content, string? source = "Family App")
  {
    ResourceType = Guard.Against.NullOrEmpty(resourceType, nameof(resourceType));
    ResourceId = Guard.Against.NullOrEmpty(resourceId, nameof(resourceId));
    PatientId = Guard.Against.Null(patientId, nameof(patientId));
    Content = Guard.Against.NullOrEmpty(content, nameof(content));
    Source = source ?? "Family App";
    Status = FhirResourceStatus.Active;
    VersionId = "1";
    LastUpdated = DateTime.UtcNow;

    RegisterDomainEvent(new FhirResourceCreatedDomainEvent(this));
  }

  public void UpdateContent(string newContent)
  {
    Guard.Against.NullOrEmpty(newContent, nameof(newContent));
    Content = newContent;
    VersionId = (int.Parse(VersionId) + 1).ToString();
    LastUpdated = DateTime.UtcNow;
    RegisterDomainEvent(new FhirResourceUpdatedDomainEvent(this));
  }

  public void MarkAsDeleted()
  {
    Status = FhirResourceStatus.Deleted;
    LastUpdated = DateTime.UtcNow;
  }

  public T? DeserializeAs<T>() where T : Resource
  {
    try
    {
      var parser = new FhirJsonParser();
      return parser.Parse<T>(Content);
    }
    catch
    {
      return default;
    }
  }

  public Resource? DeserializeAsResource()
  {
    try
    {
      var parser = new FhirJsonParser();
      return parser.Parse<Resource>(Content);
    }
    catch
    {
      return default;
    }
  }

  public static FhirResource FromResource<T>(T resource, PatientId patientId, string? source = null) where T : Resource
  {
    var serializer = new FhirJsonSerializer();
    var content = serializer.SerializeToString(resource);

    return new FhirResource(
        resource.TypeName,
        resource.Id ?? Guid.NewGuid().ToString(),
        patientId,
        content,
        source
    );
  }

  // Helper methods for common FHIR resource types
  public bool IsPatientResource => ResourceType == "Patient";
  public bool IsObservationResource => ResourceType == "Observation";
  public bool IsConditionResource => ResourceType == "Condition";
  public bool IsMedicationRequestResource => ResourceType == "MedicationRequest";
  public bool IsProcedureResource => ResourceType == "Procedure";
  public bool IsImmunizationResource => ResourceType == "Immunization";
  public bool IsAllergyIntoleranceResource => ResourceType == "AllergyIntolerance";
  public bool IsDocumentReferenceResource => ResourceType == "DocumentReference";
}

public enum FhirResourceStatus
{
  Active = 1,
  Inactive = 2,
  Deleted = 3,
  Error = 4
}
