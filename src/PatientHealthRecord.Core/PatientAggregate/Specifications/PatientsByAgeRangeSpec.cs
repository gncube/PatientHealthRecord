namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientsByAgeRangeSpec : Specification<Patient>
{
    public PatientsByAgeRangeSpec(int minAge, int maxAge)
    {
        var minDate = DateTime.UtcNow.AddYears(-maxAge - 1);
        var maxDate = DateTime.UtcNow.AddYears(-minAge);

        Query
            .Where(patient => patient.DateOfBirth >= minDate && patient.DateOfBirth <= maxDate);
    }
}
