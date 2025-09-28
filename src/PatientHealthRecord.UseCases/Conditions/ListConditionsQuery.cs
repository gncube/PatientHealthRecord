using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Conditions;

public record ListConditionsQuery(
    Guid? PatientId = null,
    string? Status = null,
    string? Severity = null,
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<ListConditionsResult>;

public record ListConditionsResult(
    IEnumerable<Condition> Conditions,
    int TotalCount,
    int PageNumber,
    int PageSize);
