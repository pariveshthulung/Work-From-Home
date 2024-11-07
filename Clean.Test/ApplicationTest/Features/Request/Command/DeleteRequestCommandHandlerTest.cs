using Clean.Application.Feature.Requests.Handlers.Commands;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Request.Command;

public class DeleteRequestCommandHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly DeleteRequestCommandHandler _handler;

    public DeleteRequestCommandHandlerTest()
    {
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _handler = new DeleteRequestCommandHandler(_employeeRepoMock.Object);
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenEmployeeIsNotFound()
    {
        //Arrange
        var command = new DeleteRequestCommand
        {
            RequestId = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid()
        };
        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.NotFound());
        _employeeRepoMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenRequestIsNotFound()
    {
        //Arrange
        var command = new DeleteRequestCommand
        {
            RequestId = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid()
        };
        var guidId = Guid.NewGuid();
        var address = Address.Create("Charali", "Jhapa", "12312");
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9864532107",
            1,
            address
        );
        employee.SetGuidId(guidId);

        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(RequestErrors.NotFound());
        _employeeRepoMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnSucessOkResult_WhenValidRequest()
    {
        //Arrange
        var requestId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        var command = new DeleteRequestCommand { RequestId = requestId, EmployeeId = employeeId };
        var address = Address.Create("Charali", "Jhapa", "12312");
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9864532107",
            1,
            address
        );
        employee.SetGuidId(employeeId);
        employee.SetId(1);

        var request = GeneralRequest.Create(1, 2, 1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));
        request.SetGuiId(requestId);

        employee.SubmitRequest(request);

        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeTrue();
        _employeeRepoMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
