using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.ClinicalObservations.Create;

namespace PatientHealthRecord.Web.ClinicalData;

/// <summary>
/// Endpoint for creating a new clinical observation
/// </summary>
public class CreateClinicalObservation : Endpoint<CreateClinicalObservationRequest, CreateClinicalObservationResponse>
{
  private readonly IMediator _mediator;

  public CreateClinicalObservation(IMediator mediator)
  {
    _mediator = mediator;
  }

  public override void Configure()
  {
    Post("/ClinicalObservations");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Create a new clinical observation";
      s.Description = "Creates a new clinical observation for a patient";
      s.ExampleRequest = new CreateClinicalObservationRequest
      {
        PatientId = Guid.NewGuid(),
        ObservationType = "Weight",
        Value = "70.5",
        Unit = "kg",
        RecordedAt = DateTime.UtcNow,
        RecordedBy = "Dr. Smith",
        Category = "Vital",
        Notes = "Morning weight",
        IsVisibleToFamily = true
      };
    });
  }

  public override async Task HandleAsync(CreateClinicalObservationRequest req, CancellationToken ct)
  {
    var command = new CreateClinicalObservationCommand(
        req.PatientId,
        req.ObservationType,
        req.Value,
        req.Unit,
        req.RecordedAt,
        req.RecordedBy,
        req.Category,
        req.Notes,
        req.IsVisibleToFamily);

    var result = await _mediator.Send(command, ct);

    if (result.IsSuccess)
    {
      var response = new CreateClinicalObservationResponse
      {
        Id = result.Value,
        Message = "Clinical observation created successfully"
      };
      await SendAsync(response, StatusCodes.Status201Created, ct);
    }
    else
    {
      foreach (var error in result.Errors)
      {
        AddError(error);
      }
      await SendErrorsAsync(cancellation: ct);
    }
  }
}

/// <summary>
/// Request DTO for creating a clinical observation
/// </summary>
public class CreateClinicalObservationRequest
{
  public Guid PatientId { get; set; }
  public string ObservationType { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public string? Unit { get; set; }
  public DateTime RecordedAt { get; set; }
  public string RecordedBy { get; set; } = string.Empty;
  public string Category { get; set; } = string.Empty;
  public string? Notes { get; set; }
  public bool IsVisibleToFamily { get; set; } = true;
}

/// <summary>
/// Response DTO for creating a clinical observation
/// </summary>
public class CreateClinicalObservationResponse
{
  public int Id { get; set; }
  public string Message { get; set; } = string.Empty;
}
