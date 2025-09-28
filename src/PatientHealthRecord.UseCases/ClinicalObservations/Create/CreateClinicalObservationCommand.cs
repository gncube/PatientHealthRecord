using Ardalis.Result;
using MediatR;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.ClinicalObservations.Create;

/// <summary>
/// Command to create a new clinical observation
/// </summary>
public record CreateClinicalObservationCommand(
    Guid PatientId,
    string ObservationType,
    string Value,
    string? Unit,
    DateTime RecordedAt,
    string RecordedBy,
    string Category,
    string? Notes,
    bool IsVisibleToFamily = true) : IRequest<Result<int>>;

/// <summary>
/// Handler for the CreateClinicalObservationCommand
/// </summary>
public class CreateClinicalObservationCommandHandler(
    IRepository<ClinicalObservation> repository) : IRequestHandler<CreateClinicalObservationCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateClinicalObservationCommand request, CancellationToken cancellationToken)
    {
        // Validate that the category is valid
        if (!Enum.TryParse<ObservationCategory>(request.Category, true, out var category))
        {
            return Result.Error($"Invalid category: {request.Category}. Valid categories are: {string.Join(", ", Enum.GetNames<ObservationCategory>())}");
        }

        var patientId = new PatientId(request.PatientId);

        var observation = new ClinicalObservation(
            patientId,
            request.ObservationType,
            request.Value,
            request.Unit,
            request.RecordedAt,
            request.RecordedBy,
            category,
            request.Notes,
            request.IsVisibleToFamily);

        var createdObservation = await repository.AddAsync(observation, cancellationToken);

        return Result.Success(createdObservation.Id);
    }
}
