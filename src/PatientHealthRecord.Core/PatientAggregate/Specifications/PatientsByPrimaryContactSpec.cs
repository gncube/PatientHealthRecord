namespace PatientHealthRecord.Core.PatientAggregate.Specifications;

public class PatientsByPrimaryContactSpec : Specification<Patient>
{
    public PatientsByPrimaryContactSpec(Guid primaryContactId) =>
      Query
          .Where(patient => patient.PrimaryContactId == primaryContactId);
}
