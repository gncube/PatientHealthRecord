using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.Delete;

/// <summary>
/// Command to delete a clinical observation
/// </summary>
public record DeleteClinicalObservationCommand(int Id) : IRequest<Result>;

/// <summary>
/// Handler for the DeleteClinicalObservationCommand
/// </summary>
public class DeleteClinicalObservationCommandHandler(
    IRepository<ClinicalObservation> repository) : IRequestHandler<DeleteClinicalObservationCommand, Result>
{
    public async Task<Result> Handle(DeleteClinicalObservationCommand request, CancellationToken cancellationToken)
    {
        var existingObservation = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (existingObservation is null)
        {
            return Result.NotFound($"Clinical observation with ID {request.Id} not found.");
        }

        await repository.DeleteAsync(existingObservation, cancellationToken);

        return Result.Success();
    }
}
