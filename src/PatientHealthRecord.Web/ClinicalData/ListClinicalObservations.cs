using PatientHealthRecord.UseCases.ClinicalObservations.List;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// Request for listing clinical observations
/// </summary>
public class ClinicalObservationsRequest
{
  /// <summary>
  /// Patient ID to filter observations (optional)
  /// </summary>
  public Guid? PatientId { get; set; }

  /// <summary>
  /// Number of records to skip
  /// </summary>
  public int? Skip { get; set; }

  /// <summary>
  /// Maximum number of records to return
  /// </summary>
  public int? Take { get; set; }
}

/// <summary>
/// List all Clinical Observations
/// </summary>
public class ListClinicalObservations(IMediator _mediator) : Endpoint<ClinicalObservationsRequest, ClinicalObservationListResponse>
{
  public override void Configure()
  {
    Get("/ClinicalObservations");
    AllowAnonymous();
  }

  public override async Task HandleAsync(ClinicalObservationsRequest request, CancellationToken cancellationToken)
  {
    var query = new ListClinicalObservationsQuery(request.PatientId, request.Skip, request.Take);
    var result = await _mediator.Send(query, cancellationToken);

    if (result.IsSuccess)
    {
      Response = new ClinicalObservationListResponse
      {
        ClinicalObservations = result.Value.Select(co => new ClinicalObservationRecord(
          co.Id,
          co.PatientId.Value, // Convert PatientId value object to Guid
          co.ObservationType,
          co.Value,
          co.Unit,
          co.RecordedAt.ToUniversalTime(), // Convert DateTime to DateTimeOffset
          co.RecordedBy,
          co.Category.ToString(), // Convert enum to string
          co.Notes,
          co.IsVisibleToFamily)).ToList()
      };
    }
  }
}
