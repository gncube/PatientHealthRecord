namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientSearchSpec : Specification<Patient>
{
    public PatientSearchSpec(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLowerInvariant();

        Query
            .Where(patient =>
                patient.FirstName.ToLower().Contains(lowerSearchTerm) ||
                patient.LastName.ToLower().Contains(lowerSearchTerm) ||
                patient.Email.ToLower().Contains(lowerSearchTerm));
    }
}
