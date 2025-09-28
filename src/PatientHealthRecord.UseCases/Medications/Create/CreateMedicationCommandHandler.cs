using Ardalis.Result;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.Core.PatientAggregate;

namespace PatientHealthRecord.UseCases.Medications.Create;

public class CreateMedicationCommandHandler(IRepository<Medication> _repository, IPatientRepository _patientRepository) : ICommandHandler<CreateMedicationCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
    {
        // Validate that patient exists
        var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
        if (patient == null)
        {
            return Result.NotFound($"Patient with ID {request.PatientId} not found");
        }

        var medication = new Medication(
            patientId: new PatientId(request.PatientId),
            name: request.Name,
            dosage: request.Dosage,
            frequency: request.Frequency,
            instructions: request.Instructions,
            startDate: request.StartDate,
            prescribedBy: request.PrescribedBy,
            purpose: request.Purpose,
            recordedBy: request.RecordedBy,
            isVisibleToFamily: request.IsVisibleToFamily);

        var createdMedication = await _repository.AddAsync(medication, cancellationToken);
        return Result.Success(createdMedication.Id);
    }
}
