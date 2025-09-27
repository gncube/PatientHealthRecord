using PatientHealthRecord.UseCases.Patients.Create;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Create a new Patient
/// </summary>
/// <remarks>
/// Creates a new Patient with the provided information.
/// </remarks>
public class Create(IMediator _mediator)
  : Endpoint<CreatePatientRequest, CreatePatientResponse>
{
    public override void Configure()
    {
        Post(CreatePatientRequest.Route);
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a new Patient.";
            s.Description = "Create a new Patient. All required fields must be provided.";
            s.ExampleRequest = new CreatePatientRequest
            {
                Email = "patient@example.com",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                PhoneNumber = "+1234567890"
            };
        });
    }

    public override async Task HandleAsync(
      CreatePatientRequest request,
      CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreatePatientCommand(
          request.Email!,
          request.FirstName!,
          request.LastName!,
          request.DateOfBirth,
          request.Gender!,
          request.Relationship,
          request.PrimaryContactId,
          request.PhoneNumber
        ), cancellationToken);

        if (result.IsSuccess)
        {
            Response = new CreatePatientResponse(
              result.Value,
              request.Email!,
              request.FirstName!,
              request.LastName!
            );
            return;
        }

        // Handle failure cases
        await SendErrorsAsync(400, cancellationToken);
    }
}
