using PatientHealthRecord.UseCases.Patients.Get;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Get a Patient by GUID ID.
/// </summary>
/// <remarks>
/// Takes a GUID ID and returns a matching Patient record.
/// </remarks>
public class GetById(IMediator _mediator)
  : Endpoint<GetPatientByIdRequest, PatientRecord>
{
    public override void Configure()
    {
        Get(GetPatientByIdRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPatientByIdRequest request,
      CancellationToken cancellationToken)
    {
        var query = new GetPatientQuery(request.PatientId);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            var patient = result.Value;
            Response = new PatientRecord(
              patient.PatientId,
              patient.Email,
              patient.FirstName,
              patient.LastName,
              patient.DateOfBirth,
              patient.Gender,
              patient.PhoneNumber,
              patient.Relationship,
              patient.EmergencyContactName,
              patient.EmergencyContactPhone,
              patient.EmergencyContactRelationship,
              patient.BloodType,
              patient.Allergies,
              patient.Notes,
              patient.ShareWithFamily,
              patient.RestrictedDataTypes,
              patient.IsActive,
              patient.CreatedAt,
              patient.LastAccessedAt
            );
        }
    }
}
