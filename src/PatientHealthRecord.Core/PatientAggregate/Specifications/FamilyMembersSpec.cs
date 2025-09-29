using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

/// <summary>
/// Specification for finding all family members related to a patient.
/// Includes the patient themselves, their children (who have them as primary contact),
/// and their parent (if they have a primary contact that matches).
/// </summary>
public class FamilyMembersSpec : Specification<Patient>
{
    public FamilyMembersSpec(Guid familyId)
    {
        Query.Where(p =>
            p.PatientId.Value == familyId ||                     // The patient themselves
            p.PrimaryContactId == familyId ||                    // Children who have this patient as primary contact
            (p.IsActive &&
             p.PatientId.Value != familyId &&                   // Exclude the patient to avoid duplicates
             (p.PrimaryContactId.HasValue ?
              p.PrimaryContactId.Value == familyId : false)))   // Additional safety check
             .Where(p => p.IsActive)
             .OrderBy(p => p.Relationship)
             .ThenBy(p => p.FirstName);
    }
}
