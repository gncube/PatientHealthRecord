using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Core.Specifications;

public class FamilyMembersSpec : Specification<Patient>
{
    public FamilyMembersSpec(Guid? primaryContactId)
    {
        if (primaryContactId.HasValue)
        {
            Query.Where(p => p.PrimaryContactId == primaryContactId.Value ||
                            p.PatientId.Value == primaryContactId.Value);
        }
        else
        {
            // If no primary contact specified, get all patients (for admin view)
            Query.Where(p => p.IsActive);
        }

        Query.Where(p => p.IsActive)
             .OrderBy(p => p.Relationship)
             .ThenBy(p => p.FirstName);
    }
}
