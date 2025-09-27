namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class ChildPatientsSpec : Specification<Patient>
{
    public ChildPatientsSpec()
    {
        var childThresholdDate = DateTime.UtcNow.AddYears(-18);

        Query
            .Where(patient => patient.DateOfBirth > childThresholdDate);
    }
}
