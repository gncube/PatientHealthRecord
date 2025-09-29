using PatientHealthRecord.Core.InteroperabilityAggregate;

namespace PatientHealthRecord.Core.InteroperabilityAggregate.Events;

public sealed class FhirResourceCreatedDomainEvent : DomainEventBase
{
  public FhirResourceCreatedDomainEvent(FhirResource fhirResource)
  {
    FhirResource = fhirResource;
  }

  public FhirResource FhirResource { get; }
}

public sealed class FhirResourceUpdatedDomainEvent : DomainEventBase
{
  public FhirResourceUpdatedDomainEvent(FhirResource fhirResource)
  {
    FhirResource = fhirResource;
  }

  public FhirResource FhirResource { get; }
}
