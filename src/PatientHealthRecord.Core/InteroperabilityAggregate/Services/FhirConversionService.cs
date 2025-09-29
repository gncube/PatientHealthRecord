using Hl7.Fhir.Model;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.ValueObjects;
using PatientHealthRecord.Core.Interfaces;

namespace PatientHealthRecord.Core.InteroperabilityAggregate;

public interface IFhirConversionService
{
    Hl7.Fhir.Model.Patient ConvertToFhirPatient(PatientAggregate.Patient patient);
    Hl7.Fhir.Model.Observation ConvertToFhirObservation(ClinicalObservation observation, string patientReference);
    Hl7.Fhir.Model.Condition ConvertToFhirCondition(ClinicalDataAggregate.Condition condition, string patientReference);
    Hl7.Fhir.Model.MedicationRequest ConvertToFhirMedicationRequest(ClinicalDataAggregate.Medication medication, string patientReference);
    Hl7.Fhir.Model.Bundle CreatePatientBundle(PatientAggregate.Patient patient,
        List<ClinicalObservation> observations,
        List<ClinicalDataAggregate.Condition> conditions,
        List<ClinicalDataAggregate.Medication> medications);
}

public class FhirConversionService : IFhirConversionService
{
    public Hl7.Fhir.Model.Patient ConvertToFhirPatient(PatientAggregate.Patient patient)
    {
        var fhirPatient = new Hl7.Fhir.Model.Patient
        {
            Id = patient.PatientId.Value.ToString(),
            Meta = new Meta
            {
                LastUpdated = patient.LastAccessedAt ?? patient.CreatedAt,
                Source = "Family Health Record App"
            },
            Active = patient.IsActive
        };

        // Add names
        fhirPatient.Name.Add(new HumanName
        {
            Use = HumanName.NameUse.Official,
            Family = patient.LastName,
            Given = new[] { patient.FirstName }
        });

        // Add gender
        fhirPatient.Gender = patient.Gender switch
        {
            ValueObjects.Gender.Male => AdministrativeGender.Male,
            ValueObjects.Gender.Female => AdministrativeGender.Female,
            ValueObjects.Gender.Other => AdministrativeGender.Other,
            _ => AdministrativeGender.Unknown
        };

        // Add birth date
        fhirPatient.BirthDate = patient.DateOfBirth.ToString("yyyy-MM-dd");

        // Add contact information
        if (!string.IsNullOrEmpty(patient.PhoneNumber))
        {
            fhirPatient.Telecom.Add(new ContactPoint
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = patient.PhoneNumber,
                Use = ContactPoint.ContactPointUse.Mobile
            });
        }

        fhirPatient.Telecom.Add(new ContactPoint
        {
            System = ContactPoint.ContactPointSystem.Email,
            Value = patient.Email,
            Use = ContactPoint.ContactPointUse.Home
        });

        // Add emergency contact if available
        if (!string.IsNullOrEmpty(patient.EmergencyContactName))
        {
            fhirPatient.Contact.Add(new Hl7.Fhir.Model.Patient.ContactComponent
            {
                Name = new HumanName
                {
                    Text = patient.EmergencyContactName
                },
                Relationship = new List<CodeableConcept>
                {
                    new CodeableConcept("http://terminology.hl7.org/CodeSystem/v2-0131", "EP", "Emergency contact person")
                },
                Telecom = !string.IsNullOrEmpty(patient.EmergencyContactPhone)
                    ? new List<ContactPoint>
                    {
                        new ContactPoint
                        {
                            System = ContactPoint.ContactPointSystem.Phone,
                            Value = patient.EmergencyContactPhone
                        }
                    }
                    : new List<ContactPoint>()
            });
        }

        return fhirPatient;
    }

    public Hl7.Fhir.Model.Observation ConvertToFhirObservation(ClinicalObservation observation, string patientReference)
    {
        var fhirObservation = new Hl7.Fhir.Model.Observation
        {
            Id = observation.Id.ToString(),
            Status = Hl7.Fhir.Model.ObservationStatus.Final,
            Subject = new Hl7.Fhir.Model.ResourceReference(patientReference),
            Effective = new Hl7.Fhir.Model.FhirDateTime(observation.RecordedAt),
            Meta = new Hl7.Fhir.Model.Meta
            {
                Source = "Family Health Record App"
            }
        };

        // Set observation code based on type
        fhirObservation.Code = observation.ObservationType.ToLower() switch
        {
            "weight" => new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "29463-7", "Body Weight"),
            "height" => new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "8302-2", "Body Height"),
            "blood pressure" => new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "85354-9", "Blood pressure panel"),
            "temperature" => new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "8310-5", "Body Temperature"),
            _ => new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "72133-2", "Observation")
        };

        // Set value
        if (observation.ObservationType.ToLower() == "blood pressure" && observation.Value.Contains("/"))
        {
            // Handle blood pressure as component observation
            var values = observation.Value.Split('/');
            if (values.Length == 2 && int.TryParse(values[0], out var systolic) && int.TryParse(values[1], out var diastolic))
            {
                fhirObservation.Component = new List<Hl7.Fhir.Model.Observation.ComponentComponent>
                {
                    new Hl7.Fhir.Model.Observation.ComponentComponent
                    {
                        Code = new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "8480-6", "Systolic blood pressure"),
                        Value = new Hl7.Fhir.Model.Quantity(systolic, "mm[Hg]")
                    },
                    new Hl7.Fhir.Model.Observation.ComponentComponent
                    {
                        Code = new Hl7.Fhir.Model.CodeableConcept("http://loinc.org", "8462-4", "Diastolic blood pressure"),
                        Value = new Hl7.Fhir.Model.Quantity(diastolic, "mm[Hg]")
                    }
                };
            }
        }
        else if (decimal.TryParse(observation.Value, out var numericValue))
        {
            var unit = string.IsNullOrEmpty(observation.Unit) ? "1" : observation.Unit;
            fhirObservation.Value = new Hl7.Fhir.Model.Quantity(numericValue, unit);
        }
        else
        {
            fhirObservation.Value = new Hl7.Fhir.Model.FhirString(observation.Value);
        }

        // Add performer
        if (!string.IsNullOrEmpty(observation.RecordedBy))
        {
            fhirObservation.Performer.Add(new Hl7.Fhir.Model.ResourceReference
            {
                Display = observation.RecordedBy
            });
        }

        // Add note if present
        if (!string.IsNullOrEmpty(observation.Notes))
        {
            fhirObservation.Note.Add(new Hl7.Fhir.Model.Annotation
            {
                Text = observation.Notes
            });
        }

        return fhirObservation;
    }

    public Hl7.Fhir.Model.Condition ConvertToFhirCondition(ClinicalDataAggregate.Condition condition, string patientReference)
    {
        var fhirCondition = new Hl7.Fhir.Model.Condition
        {
            Id = condition.Id.ToString(),
            Subject = new Hl7.Fhir.Model.ResourceReference(patientReference),
            RecordedDate = condition.RecordedAt.ToString("yyyy-MM-dd"),
            Meta = new Hl7.Fhir.Model.Meta
            {
                Source = "Family Health Record App"
            }
        };

        // Set clinical status
        fhirCondition.ClinicalStatus = condition.Status switch
        {
            ConditionStatus.Active => new Hl7.Fhir.Model.CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-clinical", "active", "Active"),
            ConditionStatus.Resolved => new Hl7.Fhir.Model.CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-clinical", "resolved", "Resolved"),
            ConditionStatus.Inactive => new Hl7.Fhir.Model.CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-clinical", "inactive", "Inactive"),
            _ => new Hl7.Fhir.Model.CodeableConcept("http://terminology.hl7.org/CodeSystem/condition-clinical", "active", "Active")
        };

        // Set verification status (family-reported conditions are typically unconfirmed)
        fhirCondition.VerificationStatus = new Hl7.Fhir.Model.CodeableConcept(
            "http://terminology.hl7.org/CodeSystem/condition-ver-status", "unconfirmed", "Unconfirmed");

        // Set condition code (using SNOMED CT or text)
        fhirCondition.Code = new Hl7.Fhir.Model.CodeableConcept
        {
            Text = condition.Name
        };

        // Set severity if available
        if (condition.Severity != ConditionSeverity.Mild)
        {
            fhirCondition.Severity = condition.Severity switch
            {
                ConditionSeverity.Mild => new Hl7.Fhir.Model.CodeableConcept("http://snomed.info/sct", "255604002", "Mild"),
                ConditionSeverity.Moderate => new Hl7.Fhir.Model.CodeableConcept("http://snomed.info/sct", "6736007", "Moderate"),
                ConditionSeverity.Severe => new Hl7.Fhir.Model.CodeableConcept("http://snomed.info/sct", "24484000", "Severe"),
                _ => new Hl7.Fhir.Model.CodeableConcept("http://snomed.info/sct", "255604002", "Mild")
            };
        }

        // Set onset date if available
        if (condition.OnsetDate.HasValue)
        {
            fhirCondition.Onset = new Hl7.Fhir.Model.FhirDateTime(condition.OnsetDate.Value);
        }

        // Add notes
        if (!string.IsNullOrEmpty(condition.Description) || !string.IsNullOrEmpty(condition.Treatment))
        {
            var noteText = !string.IsNullOrEmpty(condition.Description) ? condition.Description : "";
            if (!string.IsNullOrEmpty(condition.Treatment))
            {
                noteText += (string.IsNullOrEmpty(noteText) ? "" : "\n") + $"Treatment: {condition.Treatment}";
            }

            fhirCondition.Note.Add(new Hl7.Fhir.Model.Annotation { Text = noteText });
        }

        return fhirCondition;
    }

    public Hl7.Fhir.Model.MedicationRequest ConvertToFhirMedicationRequest(ClinicalDataAggregate.Medication medication, string patientReference)
    {
        var fhirMedRequest = new Hl7.Fhir.Model.MedicationRequest
        {
            Id = medication.Id.ToString(),
            Status = medication.Status switch
            {
                MedicationStatus.Active => Hl7.Fhir.Model.MedicationRequest.MedicationrequestStatus.Active,
                MedicationStatus.Stopped => Hl7.Fhir.Model.MedicationRequest.MedicationrequestStatus.Stopped,
                MedicationStatus.Completed => Hl7.Fhir.Model.MedicationRequest.MedicationrequestStatus.Completed,
                MedicationStatus.OnHold => Hl7.Fhir.Model.MedicationRequest.MedicationrequestStatus.OnHold,
                _ => Hl7.Fhir.Model.MedicationRequest.MedicationrequestStatus.Active
            },
            Intent = Hl7.Fhir.Model.MedicationRequest.MedicationRequestIntent.Order,
            Subject = new Hl7.Fhir.Model.ResourceReference(patientReference),
            AuthoredOn = medication.StartDate.ToString("yyyy-MM-dd"),
            Meta = new Hl7.Fhir.Model.Meta
            {
                Source = "Family Health Record App"
            }
        };

        // Set medication
        fhirMedRequest.Medication = new Hl7.Fhir.Model.CodeableConcept
        {
            Text = medication.Name
        };

        // Set dosage instruction
        if (!string.IsNullOrEmpty(medication.Dosage) || !string.IsNullOrEmpty(medication.Frequency))
        {
            var dosageInstruction = new Hl7.Fhir.Model.Dosage();

            if (!string.IsNullOrEmpty(medication.Dosage))
            {
                dosageInstruction.Text = medication.Dosage;
            }

            if (!string.IsNullOrEmpty(medication.Frequency))
            {
                dosageInstruction.Text = string.IsNullOrEmpty(dosageInstruction.Text)
                    ? medication.Frequency
                    : $"{dosageInstruction.Text}, {medication.Frequency}";
            }

            fhirMedRequest.DosageInstruction.Add(dosageInstruction);
        }

        // Add requester if available
        if (!string.IsNullOrEmpty(medication.PrescribedBy))
        {
            fhirMedRequest.Requester = new Hl7.Fhir.Model.ResourceReference
            {
                Display = medication.PrescribedBy
            };
        }

        // Add reason if available
        if (!string.IsNullOrEmpty(medication.Purpose))
        {
            fhirMedRequest.ReasonCode.Add(new Hl7.Fhir.Model.CodeableConcept
            {
                Text = medication.Purpose
            });
        }

        // Add notes
        var notes = new List<string>();
        if (!string.IsNullOrEmpty(medication.Instructions)) notes.Add(medication.Instructions);
        if (!string.IsNullOrEmpty(medication.SideEffects)) notes.Add($"Side effects: {medication.SideEffects}");

        if (notes.Any())
        {
            fhirMedRequest.Note.Add(new Hl7.Fhir.Model.Annotation
            {
                Text = string.Join("\n", notes)
            });
        }

        return fhirMedRequest;
    }

    public Hl7.Fhir.Model.Bundle CreatePatientBundle(PatientAggregate.Patient patient,
        List<ClinicalObservation> observations,
        List<ClinicalDataAggregate.Condition> conditions,
        List<ClinicalDataAggregate.Medication> medications)
    {
        var bundle = new Hl7.Fhir.Model.Bundle
        {
            Id = Guid.NewGuid().ToString(),
            Type = Hl7.Fhir.Model.Bundle.BundleType.Collection,
            Timestamp = DateTimeOffset.UtcNow,
            Meta = new Hl7.Fhir.Model.Meta
            {
                Source = "Family Health Record App"
            }
        };

        var patientReference = $"Patient/{patient.PatientId.Value}";

        // Add patient resource
        var fhirPatient = ConvertToFhirPatient(patient);
        bundle.Entry.Add(new Hl7.Fhir.Model.Bundle.EntryComponent
        {
            Resource = fhirPatient,
            FullUrl = patientReference
        });

        // Add observations
        foreach (var observation in observations)
        {
            var fhirObservation = ConvertToFhirObservation(observation, patientReference);
            bundle.Entry.Add(new Hl7.Fhir.Model.Bundle.EntryComponent
            {
                Resource = fhirObservation,
                FullUrl = $"Observation/{observation.Id}"
            });
        }

        // Add conditions
        foreach (var condition in conditions)
        {
            var fhirCondition = ConvertToFhirCondition(condition, patientReference);
            bundle.Entry.Add(new Hl7.Fhir.Model.Bundle.EntryComponent
            {
                Resource = fhirCondition,
                FullUrl = $"Condition/{condition.Id}"
            });
        }

        // Add medications
        foreach (var medication in medications)
        {
            var fhirMedRequest = ConvertToFhirMedicationRequest(medication, patientReference);
            bundle.Entry.Add(new Hl7.Fhir.Model.Bundle.EntryComponent
            {
                Resource = fhirMedRequest,
                FullUrl = $"MedicationRequest/{medication.Id}"
            });
        }

        return bundle;
    }
}
