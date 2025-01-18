using AutoMapper;
using Clean.Application.Dto.Address;
using Clean.Application.Dto.Employee;
using Clean.Application.Dto.Employee.Validation;
using Clean.Application.Feature.Employees.Handlers.Commands;
using Clean.Application.Feature.Employees.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Product.Command;

public class UpdateEmployeeCommandHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<ICurrentUserService> _userServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateEmployeeCommandHandler _handler;

    public UpdateEmployeeCommandHandlerTest()
    {
        _employeeRepositoryMock = new();
        _userServiceMock = new();
        _mapperMock = new();
        _handler = new UpdateEmployeeCommandHandler(
            _employeeRepositoryMock.Object,
            _mapperMock.Object,
            _userServiceMock.Object
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailurResult_WhenUserInputNotValid()
    {
        // Arrange
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            Id = Guid.NewGuid(),
            Name = "Parivesh",
            Email = "pariveshthulung@gmail.com",
            UserRoleId = 1,
            PhoneNumber = "98123678",
            Address = new AddressDto
            {
                Street = "Charali",
                City = "Charali",
                PostalCode = "43231"
            }
        };
        var validator = new UpdateEmployeeDtoValitator();

        //Act
        var validatorResult = await validator.ValidateAsync(updateEmployeeDto, default);

        // Assert
        validatorResult.IsValid.Should().BeFalse();
        validatorResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async void Handler_Should_ReturnFailurResultNotFound_WhenEmployeeIsNotFound()
    {
        // Arrange
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            Id = Guid.NewGuid(),
            Name = "Parivesh",
            Email = "pariveshthulung@gmail.com",
            UserRoleId = 1,
            PhoneNumber = "9812367823",
            Address = new AddressDto
            {
                Street = "Charali",
                City = "Charali",
                PostalCode = "43231"
            }
        };
        var command = new UpdateEmployeeCommand { UpdateEmployeeDto = updateEmployeeDto };

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.NotFound());
        _employeeRepositoryMock.Verify(
            x =>
                x.UpdateEmployeeAsync(
                    It.IsAny<Employee>(),
                    It.IsAny<Employee>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }

    [Fact]
    public async Task Handler_Should_UpdateEmployeeAndReturnOkResult_WhenSucessAsync()
    {
        // Arrange
        var address = Address.Create("Charali", "Jhapa", "12312");
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9864532107",
            1,
            address
        );
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            Id = Guid.NewGuid(),
            Name = "Updated",
            Email = "parivesh@gmail.com",
            UserRoleId = 1,
            PhoneNumber = "9812367823",
            Address = new AddressDto
            {
                Street = "Charali",
                City = "Charali",
                PostalCode = "43231"
            }
        };
        var command = new UpdateEmployeeCommand { UpdateEmployeeDto = updateEmployeeDto };

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);
        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeTrue();
        employee.UpdatedBy.Should().Be(employee.Id);
        employee.Name.Should().Be(updateEmployeeDto.Name);
        _employeeRepositoryMock.Verify(
            x =>
                x.UpdateEmployeeAsync(
                    It.IsAny<Employee>(),
                    It.IsAny<Employee>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }
}
