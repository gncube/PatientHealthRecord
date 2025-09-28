using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.Medications.GetById;

/// <summary>
/// Query to get a medication by ID
/// </summary>
public record GetMedicationByIdQuery(int Id) : IRequest<Result<Medication>>;

/// <summary>
/// Handler for the GetMedicationByIdQuery
/// </summary>
public class GetMedicationByIdQueryHandler(
    IRepository<Medication> repository) : IRequestHandler<GetMedicationByIdQuery, Result<Medication>>
{
    public async Task<Result<Medication>> Handle(GetMedicationByIdQuery request, CancellationToken cancellationToken)
    {
        var medication = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (medication is null)
        {
            return Result.NotFound($"Medication with ID {request.Id} not found.");
        }

        return Result.Success(medication);
    }
}
