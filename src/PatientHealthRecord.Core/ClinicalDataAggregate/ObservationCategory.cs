namespace PatientHealthRecord.Core.ClinicalDataAggregate;

public enum ObservationCategory
{
    Vital = 1,        // Weight, Height, Blood Pressure, Temperature
    Symptom = 2,      // Pain, Fatigue, Mood
    Medication = 3,   // Medication taken, Side effects
    Exercise = 4,     // Physical activity, Steps
    General = 5       // Other observations
}
