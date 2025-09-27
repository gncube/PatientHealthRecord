using PatientHealthRecord.UseCases.Patients.List;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// List all Patients
/// </summary>
/// <remarks>
/// List all patients - returns a PatientListResponse containing the Patients.
/// </remarks>
public class List(IMediator _mediator) : EndpointWithoutRequest<PatientListResponse>
{
  public override void Configure()
  {
    Get("/Patients");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListPatientsQuery(null, null), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new PatientListResponse
      {
        Patients = result.Value.Select(p => new PatientRecord(
          p.PatientId,
          p.Email,
          p.FirstName,
          p.LastName,
          p.DateOfBirth,
          p.Gender,
          p.PhoneNumber,
          p.Relationship,
          p.EmergencyContactName,
          p.EmergencyContactPhone,
          p.EmergencyContactRelationship,
          p.BloodType,
          p.Allergies,
          p.Notes,
          p.ShareWithFamily,
          p.RestrictedDataTypes,
          p.IsActive,
          p.CreatedAt,
          p.LastAccessedAt)).ToList()
      };
    }
  }
}
