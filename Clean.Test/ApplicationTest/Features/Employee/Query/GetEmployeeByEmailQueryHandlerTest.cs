using AutoMapper;
using Clean.Application.Dto.Employee;
using Clean.Application.Feature.Employees.Handlers.Queries;
using Clean.Application.Feature.Employees.Request.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Product.Query;

public class GetEmployeeByEmailQueryHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEmployeeByEmailQueryHandler _handler;

    public GetEmployeeByEmailQueryHandlerTest()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetEmployeeByEmailQueryHandler(
            _employeeRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handler_should_ReturnFailureResult_WhenEmployeeIsNotFoundAsync()
    { //Arrange
        var query = new GetEmployeeByEmailQuery { Email = "parivesh@gmail.com" };

        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.NotFound());
    }

    [Fact]
    public async void Handler_Should_ReturnEmployeeDto_WhenEmployeeIsFound()
    {
        //Arrange
        var guidId = Guid.NewGuid();
        var query = new GetEmployeeByEmailQuery { Email = "parivesh@gmail.com" };
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "1223")
        );
        var employeeDto = new EmployeeDto
        {
            Id = 1,
            Name = "Parivesh",
            Email = "parivesh@gmail.com",
            UserRoleId = 1,
            PhoneNumber = "9876543210",
        };
        employee.SetGuidId(guidId);

        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync("parivesh@gmail.com", It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(employee);
        _mapperMock.Setup(x => x.Map<EmployeeDto>(employee)).Returns(employeeDto);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Errors.Should().BeNull();
        result.Success!.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEquivalentTo(employeeDto);
    }
}
