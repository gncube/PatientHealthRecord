using PatientHealthRecord.Core.ClinicalDataAggregate;
using PatientHealthRecord.UseCases.ClinicalObservations.List;

namespace PatientHealthRecord.UnitTests.UseCases.ClinicalObservations.List;

public class ListClinicalObservationsQueryHandlerTests
{
    private readonly IListClinicalObservationsQueryService _queryService = Substitute.For<IListClinicalObservationsQueryService>();
    private readonly ListClinicalObservationsQueryHandler _handler;

    public ListClinicalObservationsQueryHandlerTests()
    {
        _handler = new ListClinicalObservationsQueryHandler(_queryService);
    }

    [Fact]
    public async Task ReturnsSuccessResultWithObservations()
    {
        // Arrange
        var patientId = new PatientId(Guid.NewGuid());
        var observations = new List<ClinicalObservation>
        {
            new ClinicalObservation(patientId, "Weight", "70.5", "kg", DateTime.UtcNow, "Self", ObservationCategory.Vital)
        };

        _queryService.ListAsync(Arg.Any<Guid?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(observations);

        var query = new ListClinicalObservationsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
        result.Value[0].ObservationType.ShouldBe("Weight");
    }

    [Fact]
    public async Task PassesCorrectParametersToQueryService()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var skip = 10;
        var take = 20;

        _queryService.ListAsync(Arg.Any<Guid?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(new List<ClinicalObservation>());

        var query = new ListClinicalObservationsQuery(patientId, skip, take);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _queryService.Received(1).ListAsync(
            Arg.Is<Guid?>(p => p == patientId),
            Arg.Is<int?>(s => s == skip),
            Arg.Is<int?>(t => t == take),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandlesNullParametersCorrectly()
    {
        // Arrange
        _queryService.ListAsync(Arg.Any<Guid?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(new List<ClinicalObservation>());

        var query = new ListClinicalObservationsQuery(); // All null parameters

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _queryService.Received(1).ListAsync(
            Arg.Is<Guid?>(p => p == null),
            Arg.Is<int?>(s => s == null),
            Arg.Is<int?>(t => t == null),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnsEmptyListWhenNoObservations()
    {
        // Arrange
        _queryService.ListAsync(Arg.Any<Guid?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(new List<ClinicalObservation>());

        var query = new ListClinicalObservationsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeEmpty();
    }

    [Fact]
    public async Task PassesCancellationTokenCorrectly()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        _queryService.ListAsync(Arg.Any<Guid?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(new List<ClinicalObservation>());

        var query = new ListClinicalObservationsQuery();

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _queryService.Received(1).ListAsync(
            Arg.Any<Guid?>(),
            Arg.Any<int?>(),
            Arg.Any<int?>(),
            Arg.Is<CancellationToken>(ct => ct == cancellationToken));
    }
}
