using Ardalis.GuardClauses;

namespace PatientHealthRecord.Core.ValueObjects;

public record ClinicalCode
{
  public string System { get; }
  public string Code { get; }
  public string Display { get; }
  public string? Version { get; }

  public ClinicalCode(string system, string code, string display, string? version = null)
  {
    System = Guard.Against.NullOrEmpty(system, nameof(system));
    Code = Guard.Against.NullOrEmpty(code, nameof(code));
    Display = Guard.Against.NullOrEmpty(display, nameof(display));
    Version = version;
  }

  // Common clinical code systems
  public static ClinicalCode Loinc(string code, string display) =>
    new("http://loinc.org", code, display);

  public static ClinicalCode Snomed(string code, string display) =>
    new("http://snomed.info/sct", code, display);

  public static ClinicalCode Icd10(string code, string display) =>
    new("http://hl7.org/fhir/sid/icd-10-cm", code, display);
}
