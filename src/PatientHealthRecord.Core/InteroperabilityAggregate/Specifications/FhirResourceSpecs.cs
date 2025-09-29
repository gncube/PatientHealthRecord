using Ardalis.Specification;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.InteroperabilityAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ValueObjects;

namespace PatientHealthRecord.Core.InteroperabilityAggregate.Specifications;

public class FhirResourcesByPatientSpec : Specification<FhirResource>
{
  public FhirResourcesByPatientSpec(PatientId patientId, string? resourceType = null)
  {
    Query.Where(f => f.PatientId == patientId && f.Status == FhirResourceStatus.Active);

    if (!string.IsNullOrEmpty(resourceType))
    {
      Query.Where(f => f.ResourceType == resourceType);
    }

    Query.OrderByDescending(f => f.LastUpdated);
  }
}

public class FhirResourceByTypeAndIdSpec : Specification<FhirResource>
{
  public FhirResourceByTypeAndIdSpec(string resourceType, string resourceId)
  {
    Query.Where(f => f.ResourceType == resourceType &&
                    f.ResourceId == resourceId &&
                    f.Status == FhirResourceStatus.Active);
  }
}

public class ObservationsByPatientAndDateRangeSpec : Specification<ClinicalObservation>
{
  public ObservationsByPatientAndDateRangeSpec(PatientId patientId, DateTime? fromDate, DateTime? toDate)
  {
    Query.Where(o => o.PatientId == patientId);

    if (fromDate.HasValue)
    {
      Query.Where(o => o.RecordedAt >= fromDate.Value);
    }

    if (toDate.HasValue)
    {
      Query.Where(o => o.RecordedAt <= toDate.Value);
    }

    Query.OrderByDescending(o => o.RecordedAt);
  }
}

public class ConditionsByPatientSpec : Specification<Condition>
{
  public ConditionsByPatientSpec(PatientId patientId)
  {
    Query.Where(c => c.PatientId == patientId)
         .OrderByDescending(c => c.RecordedAt);
  }
}

public class MedicationsByPatientSpec : Specification<Medication>
{
  public MedicationsByPatientSpec(PatientId patientId)
  {
    Query.Where(m => m.PatientId == patientId)
         .OrderByDescending(m => m.StartDate);
  }
}
