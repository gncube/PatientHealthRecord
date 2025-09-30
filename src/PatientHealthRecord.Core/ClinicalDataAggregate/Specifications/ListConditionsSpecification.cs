using Ardalis.Specification;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.Core.ClinicalDataAggregate.Specifications;

public class ListConditionsSpecification : Specification<Condition>
{
    public ListConditionsSpecification(Guid? patientId = null, ConditionStatus? status = null, ConditionSeverity? severity = null, string? searchTerm = null)
    {
        if (patientId.HasValue)
        {
            Query.Where(c => c.PatientId.Value == patientId.Value);
        }

        if (status.HasValue)
        {
            Query.Where(c => c.Status == status.Value);
        }

        if (severity.HasValue)
        {
            Query.Where(c => c.Severity == severity.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        Query.OrderByDescending(c => c.OnsetDate);
    }
}
