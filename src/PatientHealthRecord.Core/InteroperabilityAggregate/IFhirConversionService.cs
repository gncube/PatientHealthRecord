using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using Hl7.Fhir.Model;

namespace PatientHealthRecord.Core.InteroperabilityAggregate;

public interface IFhirConversionService
{
    /// <summary>
    /// Creates a FHIR Bundle containing patient data and related clinical resources
    /// </summary>
    /// <param name="patient">The patient to include in the bundle</param>
    /// <param name="observations">Clinical observations to include</param>
    /// <param name="conditions">Medical conditions to include</param>
    /// <param name="medications">Medications to include</param>
    /// <returns>A FHIR Bundle with the patient and clinical data</returns>
    Bundle CreatePatientBundle(
        PatientAggregate.Patient patient,
        IEnumerable<ClinicalObservation> observations,
        IEnumerable<ClinicalDataAggregate.Condition> conditions,
        IEnumerable<ClinicalDataAggregate.Medication> medications);
}
