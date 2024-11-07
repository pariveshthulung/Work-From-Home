using Clean.Application.Feature.Employees.Handlers.Commands;
using Clean.Application.Feature.Employees.Request.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Product.Command;

public class DeleteEmployeeCommandHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock;
    private readonly DeleteEmployeeCommandHandler _handler;

    public DeleteEmployeeCommandHandlerTest()
    {
        _employeeRepoMock = new Mock<IEmployeeRepository>();
        _handler = new DeleteEmployeeCommandHandler(_employeeRepoMock.Object);
    }

    [Fact]
    public async void Handler_should_ReturnFailureResult_whenEmployeeNotFound()
    {
        //Arrange
        var command = new DeleteEmployeeCommand { EmployeeId = Guid.NewGuid() };
        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);
        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(EmployeeErrors.NotFound());
        _employeeRepoMock.Verify(
            x => x.DeleteEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_should_ReturnFailureResult_whenEmployeeAppUserNotFound()
    {
        //Arrange
        var command = new DeleteEmployeeCommand { EmployeeId = Guid.NewGuid() };
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "34342")
        );
        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);
        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(new Error(400, "AppUser.Null", "AppUser doesnot exist."));
        _employeeRepoMock.Verify(
            x => x.DeleteEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Handler_should_ReturnSucessOkResult_whenValidRequestAsync()
    {
        //Arrange
        var guidId = Guid.NewGuid();
        var command = new DeleteEmployeeCommand { EmployeeId = guidId };
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "34342")
        );
        employee.SetGuidId(guidId);
        var appuser = new AppUser();
        employee.SetAppUser(appuser);

        _employeeRepoMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().BeNull();
        result.Success.Should().BeTrue();
        _employeeRepoMock.Verify(
            x => x.DeleteEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
