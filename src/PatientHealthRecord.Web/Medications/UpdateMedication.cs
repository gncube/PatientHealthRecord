using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.Update;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Endpoint for updating a medication
/// </summary>
public class UpdateMedication : Endpoint<UpdateMedicationRequest, UpdateMedicationResponse>
{
    private readonly IMediator _mediator;

    public UpdateMedication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Put("/Medications/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update a medication";
            s.Description = "Updates an existing medication by its ID";
            s.ExampleRequest = new UpdateMedicationRequest
            {
                Id = 1,
                Name = "Lisinopril",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow,
                PrescribedBy = "Dr. Smith",
                Purpose = "Blood pressure control",
                RecordedBy = "Dr. Smith",
                IsVisibleToFamily = true
            };
        });
    }

    public override async Task HandleAsync(UpdateMedicationRequest req, CancellationToken ct)
    {
        var command = new UpdateMedicationCommand(
            req.Id,
            req.PatientId,
            req.Name,
            req.Dosage,
            req.Frequency,
            req.Instructions,
            req.StartDate,
            req.PrescribedBy,
            req.Purpose,
            req.RecordedBy,
            req.IsVisibleToFamily);

        var result = await _mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            var response = new UpdateMedicationResponse
            {
                Message = "Medication updated successfully"
            };
            await SendAsync(response, cancellation: ct);
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
/// Request DTO for updating a medication
/// </summary>
public class UpdateMedicationRequest
{
    public int Id { get; set; }
    public Guid PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Instructions { get; set; }
    public DateTime StartDate { get; set; }
    public string? PrescribedBy { get; set; }
    public string? Purpose { get; set; }
    public string RecordedBy { get; set; } = string.Empty;
    public bool IsVisibleToFamily { get; set; } = true;
}

/// <summary>
/// Response DTO for updating a medication
/// </summary>
public class UpdateMedicationResponse
{
    public string Message { get; set; } = string.Empty;
}
