using Ardalis.Specification;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.Core.ClinicalDataAggregate.Specifications;

public class ListMedicationsSpecification : Specification<Medication>
{
  public ListMedicationsSpecification(Guid? patientId = null, MedicationStatus? status = null, string? searchTerm = null)
  {
    if (patientId.HasValue)
    {
      Query.Where(m => m.PatientId.Value == patientId.Value);
    }

    if (status.HasValue)
    {
      Query.Where(m => m.Status == status.Value);
    }

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
      Query.Where(m => m.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                     (m.Purpose != null && m.Purpose.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
    }

    Query.OrderByDescending(m => m.StartDate);
  }
}
