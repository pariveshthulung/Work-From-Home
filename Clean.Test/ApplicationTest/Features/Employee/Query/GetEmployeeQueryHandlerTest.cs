using System;
using System.Net.Mail;
using AutoMapper;
using Clean.Application.Feature.Employees.Handlers.Queries;
using Clean.Application.Feature.Employees.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Product.Query;

public class GetEmployeeQueryHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEmployeeQueryHandler _handler;

    public GetEmployeeQueryHandlerTest()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetEmployeeQueryHandler(_employeeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenEmployeeIsNotFound()
    {
        //Arrange
        var guidId = Guid.NewGuid();
        var query = new GetEmployeeQuery { GuidId = guidId };

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
    }

    [Fact]
    public async void Handler_Should_ReturnEmployeeDto_WhenEmployeeIsFound()
    {
        //Arrange
        var guidId = Guid.NewGuid();
        var query = new GetEmployeeQuery { GuidId = guidId };
        var employee = GeneralEmployee.Create(
            "Parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "1223")
        );
        employee.SetGuidId(guidId);

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Errors.Should().BeNull();
        result.Success!.Should().BeTrue();
    }
}
