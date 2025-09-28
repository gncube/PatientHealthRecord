using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.GetById;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Endpoint for getting a medication by ID
/// </summary>
public class GetMedication : Endpoint<GetMedicationRequest, GetMedicationResponse>
{
    private readonly IMediator _mediator;

    public GetMedication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Get("/Medications/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get a medication by ID";
            s.Description = "Retrieves a specific medication by its ID";
        });
    }

    public override async Task HandleAsync(GetMedicationRequest req, CancellationToken ct)
    {
        var query = new GetMedicationByIdQuery(req.Id);
        var result = await _mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            var medication = result.Value;
            var response = new GetMedicationResponse
            {
                Id = medication.Id,
                PatientId = medication.PatientId.Value,
                Name = medication.Name,
                Dosage = medication.Dosage,
                Frequency = medication.Frequency,
                Instructions = medication.Instructions,
                StartDate = medication.StartDate,
                EndDate = medication.EndDate,
                Status = medication.Status.ToString(),
                PrescribedBy = medication.PrescribedBy,
                Purpose = medication.Purpose,
                SideEffects = medication.SideEffects,
                RecordedBy = medication.RecordedBy,
                IsVisibleToFamily = medication.IsVisibleToFamily
            };
            await SendAsync(response, cancellation: ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}

/// <summary>
/// Request DTO for getting a medication by ID
/// </summary>
public class GetMedicationRequest
{
    public int Id { get; set; }
}

/// <summary>
/// Response DTO for getting a medication by ID
/// </summary>
public class GetMedicationResponse
{
    public int Id { get; set; }
    public Guid PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Instructions { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PrescribedBy { get; set; }
    public string? Purpose { get; set; }
    public string? SideEffects { get; set; }
    public string RecordedBy { get; set; } = string.Empty;
    public bool IsVisibleToFamily { get; set; }
}
