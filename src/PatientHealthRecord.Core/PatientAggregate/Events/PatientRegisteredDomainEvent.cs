using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.Entities.PatientAggregate.Events;

public sealed class PatientRegisteredDomainEvent : DomainEventBase
{
  public PatientRegisteredDomainEvent(Patient patient)
  {
    Patient = patient;
  }

  public Patient Patient { get; set; }
}
