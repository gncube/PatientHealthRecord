using PatientHealthRecord.UseCases.Patients.GetFamily;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Get family members for a patient.
/// </summary>
/// <remarks>
/// Takes a family ID and returns all family members.
/// </remarks>
public class GetFamilyMembers(IMediator _mediator)
  : Endpoint<GetFamilyMembersRequest, GetFamilyMembersResponse>
{
    public override void Configure()
    {
        Get(GetFamilyMembersRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetFamilyMembersRequest request,
      CancellationToken cancellationToken)
    {
        var query = new GetFamilyMembersQuery(request.FamilyId);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            Response = new GetFamilyMembersResponse(result.Value);
        }
    }
}
