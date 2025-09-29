using PatientHealthRecord.Core.InteroperabilityAggregate;
using PatientHealthRecord.Core.PatientAggregate;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using Hl7.Fhir.Model;
using System.Globalization;

// Type aliases to resolve naming conflicts
using DomainPatient = PatientHealthRecord.Core.PatientAggregate.Patient;
using DomainCondition = PatientHealthRecord.Core.ClinicalDataAggregate.Condition;
using DomainMedication = PatientHealthRecord.Core.ClinicalDataAggregate.Medication;

namespace PatientHealthRecord.Infrastructure.Services;

/// <summary>
/// Service for converting domain objects to FHIR R4 resources
/// </summary>
public class FhirConversionService : IFhirConversionService
{
    /// <inheritdoc />
    public Bundle CreatePatientBundle(
        DomainPatient patient,
        IEnumerable<ClinicalObservation> observations,
        IEnumerable<DomainCondition> conditions,
        IEnumerable<DomainMedication> medications)
    {
        var bundle = new Bundle
        {
            Id = Guid.NewGuid().ToString(),
            Type = Bundle.BundleType.Document,
            Timestamp = DateTimeOffset.UtcNow,
            Meta = new Meta
            {
                LastUpdated = DateTimeOffset.UtcNow
            }
        };

        // Add patient resource
        var patientResource = CreatePatientResource(patient);
        bundle.Entry.Add(new Bundle.EntryComponent
        {
            FullUrl = $"urn:uuid:{patientResource.Id}",
            Resource = patientResource
        });

        // Add observations
        foreach (var observation in observations)
        {
            var observationResource = CreateObservationResource(observation, patient);
            bundle.Entry.Add(new Bundle.EntryComponent
            {
                FullUrl = $"urn:uuid:{observationResource.Id}",
                Resource = observationResource
            });
        }

        // Add conditions
        foreach (var condition in conditions)
        {
            var conditionResource = CreateConditionResource(condition, patient);
            bundle.Entry.Add(new Bundle.EntryComponent
            {
                FullUrl = $"urn:uuid:{conditionResource.Id}",
                Resource = conditionResource
            });
        }

        // Add medications
        foreach (var medication in medications)
        {
            var medicationResource = CreateMedicationRequestResource(medication, patient);
            bundle.Entry.Add(new Bundle.EntryComponent
            {
                FullUrl = $"urn:uuid:{medicationResource.Id}",
                Resource = medicationResource
            });
        }

        return bundle;
    }

    private Hl7.Fhir.Model.Patient CreatePatientResource(DomainPatient patient)
    {
        var fhirPatient = new Hl7.Fhir.Model.Patient
        {
            Id = patient.PatientId.Value.ToString(),
            Meta = new Meta
            {
                LastUpdated = DateTimeOffset.UtcNow
            }
        };

        // Name
        fhirPatient.Name = new List<HumanName>
        {
            new HumanName
            {
                Family = patient.LastName,
                Given = new List<string> { patient.FirstName },
                Use = HumanName.NameUse.Official
            }
        };

        // Birth date
        fhirPatient.BirthDate = patient.DateOfBirth.ToString("yyyy-MM-dd");

        // Gender (simplified mapping)
        if (patient.FirstName.EndsWith("a", StringComparison.OrdinalIgnoreCase) ||
            patient.FirstName.EndsWith("e", StringComparison.OrdinalIgnoreCase))
        {
            fhirPatient.Gender = AdministrativeGender.Female;
        }
        else
        {
            fhirPatient.Gender = AdministrativeGender.Male;
        }

        // Telecom
        if (!string.IsNullOrEmpty(patient.PhoneNumber))
        {
            fhirPatient.Telecom = new List<ContactPoint>
            {
                new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = patient.PhoneNumber,
                    Use = ContactPoint.ContactPointUse.Home
                }
            };
        }

        return fhirPatient;
    }

    private Observation CreateObservationResource(ClinicalObservation observation, DomainPatient patient)
    {
        var fhirObservation = new Observation
        {
            Id = observation.Id.ToString(),
            Status = ObservationStatus.Final,
            Meta = new Meta
            {
                LastUpdated = observation.RecordedAt
            }
        };

        // Subject (patient reference)
        fhirObservation.Subject = new ResourceReference($"urn:uuid:{patient.PatientId.Value}");

        // Code (observation type)
        fhirObservation.Code = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = GetObservationCode(observation.ObservationType),
                    Display = observation.ObservationType
                }
            },
            Text = observation.ObservationType
        };

        // Value
        if (decimal.TryParse(observation.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericValue))
        {
            fhirObservation.Value = new Quantity
            {
                Value = numericValue,
                Unit = observation.Unit ?? "unit",
                System = "http://unitsofmeasure.org",
                Code = observation.Unit ?? "unit"
            };
        }
        else
        {
            fhirObservation.Value = new FhirString(observation.Value);
        }

        // Effective date/time
        fhirObservation.Effective = new FhirDateTime(observation.RecordedAt);

        // Performer
        if (!string.IsNullOrEmpty(observation.RecordedBy))
        {
            fhirObservation.Performer = new List<ResourceReference>
            {
                new ResourceReference
                {
                    Display = observation.RecordedBy
                }
            };
        }

        // Category
        fhirObservation.Category = new List<CodeableConcept>
        {
            new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding
                    {
                        System = "http://terminology.hl7.org/CodeSystem/observation-category",
                        Code = GetObservationCategoryCode(observation.Category),
                        Display = observation.Category.ToString()
                    }
                }
            }
        };

        // Notes
        if (!string.IsNullOrEmpty(observation.Notes))
        {
            fhirObservation.Note = new List<Annotation>
            {
                new Annotation
                {
                    Text = observation.Notes,
                    Time = observation.RecordedAt.ToString("O")
                }
            };
        }

        return fhirObservation;
    }

    private Hl7.Fhir.Model.Condition CreateConditionResource(DomainCondition condition, DomainPatient patient)
    {
        var fhirCondition = new Hl7.Fhir.Model.Condition
        {
            Id = condition.Id.ToString(),
            Meta = new Meta
            {
                LastUpdated = condition.RecordedAt
            }
        };

        // Subject (patient reference)
        fhirCondition.Subject = new ResourceReference($"urn:uuid:{patient.PatientId.Value}");

        // Code
        fhirCondition.Code = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = GetConditionCode(condition.Name),
                    Display = condition.Name
                }
            },
            Text = condition.Name
        };

        // Clinical status
        switch (condition.Status)
        {
            case ConditionStatus.Active:
                fhirCondition.ClinicalStatus = new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://terminology.hl7.org/CodeSystem/condition-clinical",
                            Code = "active",
                            Display = "Active"
                        }
                    }
                };
                break;
            case ConditionStatus.Resolved:
                fhirCondition.ClinicalStatus = new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://terminology.hl7.org/CodeSystem/condition-clinical",
                            Code = "resolved",
                            Display = "Resolved"
                        }
                    }
                };
                break;
            default:
                fhirCondition.ClinicalStatus = new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://terminology.hl7.org/CodeSystem/condition-clinical",
                            Code = "unknown",
                            Display = "Unknown"
                        }
                    }
                };
                break;
        }

        // Verification status
        fhirCondition.VerificationStatus = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://terminology.hl7.org/CodeSystem/condition-ver-status",
                    Code = "confirmed",
                    Display = "Confirmed"
                }
            }
        };

        // Severity
        fhirCondition.Severity = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = GetSeverityCode(condition.Severity),
                    Display = condition.Severity.ToString()
                }
            }
        };

        // Onset date/time
        if (condition.OnsetDate.HasValue)
        {
            fhirCondition.Onset = new FhirDateTime(condition.OnsetDate.Value);
        }

        // Abatement date/time
        if (condition.ResolvedDate.HasValue)
        {
            fhirCondition.Abatement = new FhirDateTime(condition.ResolvedDate.Value);
        }

        // Recorded date
        fhirCondition.RecordedDate = condition.RecordedAt.ToString("O");

        // Recorder
        if (!string.IsNullOrEmpty(condition.RecordedBy))
        {
            fhirCondition.Recorder = new ResourceReference
            {
                Display = condition.RecordedBy
            };
        }

        // Notes
        if (!string.IsNullOrEmpty(condition.Description))
        {
            fhirCondition.Note = new List<Annotation>
            {
                new Annotation
                {
                    Text = condition.Description,
                    Time = condition.RecordedAt.ToString("O")
                }
            };
        }

        return fhirCondition;
    }

    private MedicationRequest CreateMedicationRequestResource(DomainMedication medication, DomainPatient patient)
    {
        var medicationRequest = new MedicationRequest
        {
            Id = medication.Id.ToString(),
            Meta = new Meta
            {
                LastUpdated = medication.StartDate
            },
            Status = GetMedicationStatus(medication.Status),
            Intent = MedicationRequest.MedicationRequestIntent.Order
        };

        // Subject (patient reference)
        medicationRequest.Subject = new ResourceReference($"urn:uuid:{patient.PatientId.Value}");

        // Medication
        medicationRequest.Medication = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = GetMedicationCode(medication.Name),
                    Display = medication.Name
                }
            },
            Text = medication.Name
        };

        // Dosage instruction
        if (!string.IsNullOrEmpty(medication.Dosage) || !string.IsNullOrEmpty(medication.Frequency))
        {
            var dosage = new Dosage
            {
                Text = $"{medication.Dosage ?? "As directed"} {medication.Frequency ?? ""}".Trim()
            };

            if (!string.IsNullOrEmpty(medication.Instructions))
            {
                dosage.PatientInstruction = medication.Instructions;
            }

            medicationRequest.DosageInstruction = new List<Dosage> { dosage };
        }

        // Timing
        if (medication.StartDate != default)
        {
            medicationRequest.DispenseRequest = new MedicationRequest.DispenseRequestComponent
            {
                ValidityPeriod = new Period
                {
                    Start = medication.StartDate.ToString("O")
                }
            };

            if (medication.EndDate.HasValue)
            {
                medicationRequest.DispenseRequest.ValidityPeriod.End = medication.EndDate.Value.ToString("O");
            }
        }

        // Requester
        if (!string.IsNullOrEmpty(medication.PrescribedBy))
        {
            medicationRequest.Requester = new ResourceReference
            {
                Display = medication.PrescribedBy
            };
        }

        // Authored on
        medicationRequest.AuthoredOn = medication.StartDate.ToString("O");

        // Recorder
        if (!string.IsNullOrEmpty(medication.RecordedBy))
        {
            medicationRequest.Recorder = new ResourceReference
            {
                Display = medication.RecordedBy
            };
        }

        // Notes
        var notes = new List<Annotation>();

        if (!string.IsNullOrEmpty(medication.Purpose))
        {
            notes.Add(new Annotation
            {
                Text = $"Purpose: {medication.Purpose}",
                Time = medication.StartDate.ToString("O")
            });
        }

        if (!string.IsNullOrEmpty(medication.SideEffects))
        {
            notes.Add(new Annotation
            {
                Text = $"Side effects: {medication.SideEffects}",
                Time = medication.StartDate.ToString("O")
            });
        }

        if (notes.Any())
        {
            medicationRequest.Note = notes;
        }

        return medicationRequest;
    }

    // Helper methods for code mappings (simplified - in production, use proper terminology services)
    private string GetObservationCode(string observationType) => observationType.ToLower() switch
    {
        "blood pressure" => "75367002",
        "heart rate" => "364075005",
        "temperature" => "8310-5",
        "weight" => "29463-7",
        "height" => "8302-2",
        _ => "404684003" // Clinical finding
    };

    private string GetObservationCategoryCode(ObservationCategory category) => category switch
    {
        ObservationCategory.Vital => "vital-signs",
        ObservationCategory.Symptom => "exam",
        ObservationCategory.Medication => "exam",
        ObservationCategory.Exercise => "activity",
        ObservationCategory.General => "exam",
        _ => "exam"
    };

    private string GetConditionCode(string conditionName) => conditionName.ToLower() switch
    {
        "diabetes" => "73211009",
        "hypertension" => "38341003",
        "asthma" => "195967001",
        _ => "404684003" // Clinical finding
    };

    private string GetSeverityCode(ConditionSeverity severity) => severity switch
    {
        ConditionSeverity.Mild => "255604002",
        ConditionSeverity.Moderate => "6736007",
        ConditionSeverity.Severe => "24484000",
        _ => "255604002" // Mild
    };

    private MedicationRequest.MedicationrequestStatus GetMedicationStatus(MedicationStatus status) => status switch
    {
        MedicationStatus.Active => MedicationRequest.MedicationrequestStatus.Active,
        MedicationStatus.Completed => MedicationRequest.MedicationrequestStatus.Completed,
        MedicationStatus.Stopped => MedicationRequest.MedicationrequestStatus.Stopped,
        MedicationStatus.OnHold => MedicationRequest.MedicationrequestStatus.OnHold,
        _ => MedicationRequest.MedicationrequestStatus.Unknown
    };

    private string GetMedicationCode(string medicationName) => medicationName.ToLower() switch
    {
        "aspirin" => "387458008",
        "ibuprofen" => "387207008",
        "paracetamol" => "387517004",
        _ => "373873005" // Pharmaceutical / biologic product
    };
}
