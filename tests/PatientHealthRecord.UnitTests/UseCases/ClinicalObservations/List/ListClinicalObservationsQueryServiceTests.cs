using Microsoft.EntityFrameworkCore;
using NSubstitute;
using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.Core.Interfaces;
using PatientHealthRecord.UseCases.ClinicalObservations.List;

namespace PatientHealthRecord.UnitTests.UseCases.ClinicalObservations.List;

public class ListClinicalObservationsQueryServiceTests
{
    private readonly PatientHealthRecord.Infrastructure.Data.AppDbContext _dbContext;
    private readonly PatientHealthRecord.Infrastructure.Data.Queries.ListClinicalObservationsQueryService _service;

    public ListClinicalObservationsQueryServiceTests()
    {
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        var options = new DbContextOptionsBuilder<PatientHealthRecord.Infrastructure.Data.AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new PatientHealthRecord.Infrastructure.Data.AppDbContext(options, dispatcher);
        _service = new PatientHealthRecord.Infrastructure.Data.Queries.ListClinicalObservationsQueryService(_dbContext);
    }

    [Fact]
    public async Task ReturnsAllObservationsWhenNoFilters()
    {
        // Arrange
        var patientId1 = new PatientId(Guid.NewGuid());
        var patientId2 = new PatientId(Guid.NewGuid());

        var observation1 = new ClinicalObservation(
            patientId1, "Weight", "70.5", "kg", DateTime.UtcNow, "Self",
            ObservationCategory.Vital, "Morning weight");

        var observation2 = new ClinicalObservation(
            patientId2, "Blood Pressure", "120/80", "mmHg", DateTime.UtcNow, "Self",
            ObservationCategory.Vital, "Evening reading");

        _dbContext.ClinicalObservations.AddRange(observation1, observation2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.ListAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
    }

    [Fact]
    public async Task FiltersByPatientIdWhenProvided()
    {
        // Arrange
        var patientId1 = new PatientId(Guid.NewGuid());
        var patientId2 = new PatientId(Guid.NewGuid());

        var observation1 = new ClinicalObservation(
            patientId1, "Weight", "70.5", "kg", DateTime.UtcNow, "Self",
            ObservationCategory.Vital);

        var observation2 = new ClinicalObservation(
            patientId2, "Blood Pressure", "120/80", "mmHg", DateTime.UtcNow, "Self",
            ObservationCategory.Vital);

        _dbContext.ClinicalObservations.AddRange(observation1, observation2);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.ListAsync(patientId1.Value);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().PatientId.ShouldBe(patientId1);
    }

    [Fact]
    public async Task AppliesPaginationCorrectly()
    {
        // Arrange
        var patientId = new PatientId(Guid.NewGuid());

        for (int i = 0; i < 5; i++)
        {
            var observation = new ClinicalObservation(
                patientId, $"Observation {i}", $"Value {i}", "unit",
                DateTime.UtcNow.AddMinutes(i), "Self", ObservationCategory.General);

            _dbContext.ClinicalObservations.Add(observation);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.ListAsync(skip: 2, take: 2);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
    }

    [Fact]
    public async Task CombinesPatientIdFilterAndPagination()
    {
        // Arrange
        var patientId1 = new PatientId(Guid.NewGuid());
        var patientId2 = new PatientId(Guid.NewGuid());

        // Add 3 observations for patient 1
        for (int i = 0; i < 3; i++)
        {
            var observation = new ClinicalObservation(
                patientId1, $"Observation {i}", $"Value {i}", "unit",
                DateTime.UtcNow.AddMinutes(i), "Self", ObservationCategory.General);
            _dbContext.ClinicalObservations.Add(observation);
        }

        // Add 2 observations for patient 2
        for (int i = 0; i < 2; i++)
        {
            var observation = new ClinicalObservation(
                patientId2, $"Observation {i}", $"Value {i}", "unit",
                DateTime.UtcNow.AddMinutes(i), "Self", ObservationCategory.General);
            _dbContext.ClinicalObservations.Add(observation);
        }

        await _dbContext.SaveChangesAsync();

        // Act - Get page 2 (skip 1, take 1) for patient 1
        var result = await _service.ListAsync(patientId1.Value, skip: 1, take: 1);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().PatientId.ShouldBe(patientId1);
        result.First().ObservationType.ShouldBe("Observation 1");
    }

    [Fact]
    public async Task ReturnsEmptyListWhenNoObservationsExist()
    {
        // Act
        var result = await _service.ListAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task ReturnsEmptyListWhenPatientHasNoObservations()
    {
        // Arrange
        var patientId = new PatientId(Guid.NewGuid());

        // Act
        var result = await _service.ListAsync(patientId.Value);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}
