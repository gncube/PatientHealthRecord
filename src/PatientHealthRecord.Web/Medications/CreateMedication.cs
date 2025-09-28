using FastEndpoints;
using MediatR;
using PatientHealthRecord.UseCases.Medications.Create;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Endpoint for creating a new medication
/// </summary>
public class CreateMedication : Endpoint<CreateMedicationRequest, CreateMedicationResponse>
{
    private readonly IMediator _mediator;

    public CreateMedication(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/Medications");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a new medication";
            s.Description = "Creates a new medication for a patient";
            s.ExampleRequest = new CreateMedicationRequest
            {
                PatientId = Guid.NewGuid(),
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

    public override async Task HandleAsync(CreateMedicationRequest req, CancellationToken ct)
    {
        var command = new CreateMedicationCommand(
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
            var response = new CreateMedicationResponse
            {
                Id = result.Value,
                Message = "Medication created successfully"
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
/// Request DTO for creating a medication
/// </summary>
public class CreateMedicationRequest
{
    public Guid PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Instructions { get; set; }
    public DateTime? StartDate { get; set; }
    public string? PrescribedBy { get; set; }
    public string? Purpose { get; set; }
    public string RecordedBy { get; set; } = string.Empty;
    public bool IsVisibleToFamily { get; set; } = true;
}

/// <summary>
/// Response DTO for creating a medication
/// </summary>
public class CreateMedicationResponse
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
}
