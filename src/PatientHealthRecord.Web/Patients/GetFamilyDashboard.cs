using PatientHealthRecord.UseCases.Patients.GetFamily;

namespace PatientHealthRecord.Web.Patients;

/// <summary>
/// Get family dashboard data for a patient.
/// </summary>
/// <remarks>
/// Takes a family ID and returns dashboard data for all family members.
/// </remarks>
public class GetFamilyDashboard(IMediator _mediator)
  : Endpoint<GetFamilyDashboardRequest, GetFamilyDashboardResponse>
{
  public override void Configure()
  {
    Get(GetFamilyDashboardRequest.Route);
    AllowAnonymous();
  }

  public override async Task HandleAsync(GetFamilyDashboardRequest request,
    CancellationToken cancellationToken)
  {
    var query = new GetFamilyDashboardQuery(request.FamilyId);

    var result = await _mediator.Send(query, cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      Response = new GetFamilyDashboardResponse(result.Value);
    }
  }
}
