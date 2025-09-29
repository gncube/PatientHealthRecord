using PatientHealthRecord.UseCases.Interoperability;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.Web.Interoperability;

/// <summary>
/// Export patient data in FHIR format
/// </summary>
/// <remarks>
/// Exports comprehensive patient health data as a FHIR Bundle, including patient demographics,
/// clinical observations, conditions, and medications. The data can be exported in JSON or XML format.
/// </remarks>
public class ExportPatientData(IMediator _mediator)
  : Endpoint<ExportPatientDataRequest, ExportPatientDataResponse>
{
    public override void Configure()
    {
        Get(ExportPatientDataRequest.Route);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Export patient data in FHIR format.";
            s.Description = "Exports comprehensive patient health data as a FHIR Bundle, including demographics, observations, conditions, and medications.";
            s.ExampleRequest = new ExportPatientDataRequest
            {
                PatientId = Guid.NewGuid(),
                Format = "Json",
                IncludeObservations = true,
                IncludeConditions = true,
                IncludeMedications = true,
                FromDate = DateTime.UtcNow.AddMonths(-6),
                ToDate = DateTime.UtcNow
            };
        });
    }

    public override async Task HandleAsync(
      ExportPatientDataRequest request,
      CancellationToken cancellationToken)
    {
        // Parse the format enum
        if (!Enum.TryParse<FhirExportFormat>(request.Format, true, out var format))
        {
            format = FhirExportFormat.Json; // Default to JSON
        }

        var command = new ExportPatientDataCommand(
            new PatientId(request.PatientId),
            format,
            request.IncludeObservations,
            request.IncludeConditions,
            request.IncludeMedications,
            request.FromDate,
            request.ToDate
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            Response = new ExportPatientDataResponse(result.Value);
            return;
        }

        // Handle different failure types
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        // Handle other errors
        await SendErrorsAsync(400, cancellationToken);
    }
}
