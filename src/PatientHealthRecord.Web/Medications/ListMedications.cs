using FastEndpoints;
using PatientHealthRecord.UseCases.Medications;

namespace PatientHealthRecord.Web.Medications;

/// <summary>
/// Request for listing medications
/// </summary>
public class MedicationsRequest
{
    /// <summary>
    /// Patient ID to filter medications (optional)
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
/// List all Medications
/// </summary>
public class ListMedications(IMediator _mediator) : Endpoint<MedicationsRequest, MedicationListResponse>
{
    public override void Configure()
    {
        Get("/medications");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MedicationsRequest request, CancellationToken cancellationToken)
    {
        var query = new ListMedicationsQuery(request.PatientId, request.Skip, request.Take);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            var medicationRecords = result.Value.Select(m => new MedicationRecord(
                Id: m.Id,
                PatientId: m.PatientId.Value,
                Name: m.Name,
                Dosage: m.Dosage,
                Frequency: m.Frequency,
                Instructions: m.Instructions,
                StartDate: m.StartDate,
                EndDate: m.EndDate,
                Status: m.Status.ToString(),
                PrescribedBy: m.PrescribedBy,
                Purpose: m.Purpose,
                SideEffects: m.SideEffects,
                RecordedBy: m.RecordedBy,
                IsVisibleToFamily: m.IsVisibleToFamily)).ToList();

            Response = new MedicationListResponse(medicationRecords);
        }
    }
}
