using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Dto.Request.Validation;
using Clean.Application.Feature.Requests.Handlers.Commands;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Request.Command;

public class SubmitRequestCommandHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICurrentUserService> _currentUserMock;
    private readonly SubmitRequestCommandHandler _handler;

    public SubmitRequestCommandHandlerTest()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _mapperMock = new Mock<IMapper>();
        _currentUserMock = new();

        _handler = new SubmitRequestCommandHandler(
            _mapperMock.Object,
            _employeeRepositoryMock.Object,
            _currentUserMock.Object
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureValidationError_WhenUserInputIsInValid()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            // RequestedBy = 1,
            RequestedTo = 2,
            FromDate = DateTime.UtcNow.AddDays(5),
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 1,
        };

        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        //Act
        var result = await _handler.Handle(command, default);
        //Assert
        result.Errors.Should().NotBeEmpty();
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenRequestedToEmployeeIsNotFound()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            RequestedTo = 2,
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 1,
        };

        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((Employee?)null);
        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.NotFound(""));

        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenEmployeeIsUnAuthorize()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            RequestedToEmail = "pariveshthulung@gmail.com",
            RequestedTo = 2,
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 1,
        };
        var requestedToEmployee = GeneralEmployee.Create(
            "Parivesh",
            "pariveshthulung@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );

        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByEmailAsync(requestDto.RequestedToEmail, default))
            .ReturnsAsync(requestedToEmployee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.Unauthorize());
        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenEmployeeRequestToOwnEmail()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            // RequestedBy = 1,
            RequestedTo = 2,
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 1,
        };
        var currentUser = GeneralEmployee.Create(
            "Parivesh",
            "pariveshthulung@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        currentUser.SetId(2);
        var requestedToEmployee = GeneralEmployee.Create(
            "Parivesh",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(requestedToEmployee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(new Error(400, "Request.Submit", "Can't submit to own email"));
        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnFailureResultError_WhenEmployeeHasPreviousRequestPending()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            RequestedToEmail = "hari@gmail.com",
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 1,
        };
        var currentUser = GeneralEmployee.Create(
            "Parivesh",
            "pariveshthulung@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        currentUser.SetId(1);
        var requestedToEmployee = GeneralEmployee.Create(
            "Hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        var request = GeneralRequest.Create(
            requestBy: 1,
            requestTo: 2,
            requestedTypeId: 1,
            fromDate: DateTime.UtcNow.AddDays(1),
            toDate: DateTime.UtcNow.AddDays(2)
        );
        currentUser.SubmitRequest(request);

        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        _currentUserMock.Setup(x => x.UserEmail).Returns("pariveshthulung@gmail.com");

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByEmailAsync(requestDto.RequestedToEmail, default))
            .ReturnsAsync(requestedToEmployee);
        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(
                    _currentUserMock.Object.UserEmail,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(currentUser);
        // _employeeRepositoryMock.Setup(x=>x.)

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Errors.Should().NotBeEmpty();
        result.Success.Should().BeFalse();
        result
            .Errors.Should()
            .Contain(
                new Error(
                    400,
                    "Request.Submit",
                    "Your previous request is pending so can't summit new request."
                )
            );
        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async void Handler_Should_ReturnSuccessResult_WhenRequestIsValid()
    {
        //Arrange
        var requestDto = new CreateRequestDto
        {
            RequestedToEmail = "hari@gmail.com",
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(2),
            RequestedTypeId = 3,
        };
        var currentUser = GeneralEmployee.Create(
            "Parivesh",
            "pariveshthulung@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        currentUser.SetId(1);
        var requestedToEmployee = GeneralEmployee.Create(
            "Hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "4532")
        );
        var request = GeneralRequest.Create(
            requestBy: 1,
            requestTo: 2,
            requestedTypeId: 1,
            fromDate: DateTime.UtcNow.AddDays(1),
            toDate: DateTime.UtcNow.AddDays(2)
        );
        currentUser.SubmitRequest(request);

        var command = new SubmitRequestCommand { CreateRequestDto = requestDto };

        _currentUserMock.Setup(x => x.UserEmail).Returns("pariveshthulung@gmail.com");

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByEmailAsync(requestDto.RequestedToEmail, default))
            .ReturnsAsync(requestedToEmployee);
        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(
                    _currentUserMock.Object.UserEmail,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(currentUser);
        _mapperMock.Setup(x => x.Map<GeneralRequest>(requestDto)).Returns(request);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeTrue();
        result.Errors.Should().BeNull();

        _employeeRepositoryMock.Verify(
            x => x.UpdateEmployeeAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
