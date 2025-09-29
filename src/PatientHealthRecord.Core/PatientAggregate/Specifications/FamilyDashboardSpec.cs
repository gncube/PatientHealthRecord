using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

/// <summary>
/// Specification for finding all family members for dashboard display.
/// This includes the patient and all their family members with optimized ordering for dashboard.
/// </summary>
public class FamilyDashboardSpec : Specification<Patient>
{
    public FamilyDashboardSpec(Guid familyId)
    {
        Query.Where(p =>
            p.PatientId.Value == familyId ||                     // The patient themselves
            p.PrimaryContactId == familyId)                      // Children who have this patient as primary contact
             .Where(p => p.IsActive)
             .OrderBy(p => p.Relationship == "Self" ? 0 : 1)    // Self first
             .ThenBy(p => p.Age)                                 // Then by age
             .ThenBy(p => p.FirstName);                          // Then by first name
    }
}
