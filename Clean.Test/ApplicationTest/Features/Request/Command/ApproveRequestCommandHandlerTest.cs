using AutoMapper;
using Clean.Application.Dto.Approval;
using Clean.Application.Feature.Requests.Handlers.Commands;
using Clean.Application.Feature.Requests.Requests.Commands;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clean.Test.ApplicationTest.Features.Request.Command;

public class ApproveRequestCommandHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICurrentUserService> _currentUserMock;
    private readonly ApproveRequestCommandHandler _handler;

    public ApproveRequestCommandHandlerTest()
    {
        _employeeRepositoryMock = new();
        _mapperMock = new Mock<IMapper>();
        _currentUserMock = new Mock<ICurrentUserService>();
        _handler = new ApproveRequestCommandHandler(
            _mapperMock.Object,
            _employeeRepositoryMock.Object,
            _currentUserMock.Object
        );
    }

    [Fact]
    public async void Handler_Should_ReturnValidationFailureResult_WhenInvalidParameter()
    {
        //Arrange
        var approveRequestDto = new ApproveRequestDto
        {
            RequestId = Guid.Empty,
            EmployeeId = Guid.Empty,
            ApprovalStatusId = 3
        };
        var command = new ApproveRequestCommand { ApproveRequestDto = approveRequestDto };

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
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
    public async void Handler_Should_ReturnFailureResult_WhenEmployeeIsNotFound()
    {
        //Arrange
        var requestId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var approveRequestDto = new ApproveRequestDto
        {
            RequestId = requestId,
            EmployeeId = employeeId,
            ApprovalStatusId = 3
        };
        var command = new ApproveRequestCommand { ApproveRequestDto = approveRequestDto };

        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.NotFound());
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
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
    public async void Handler_Should_ReturnFailureResult_WhenRequestIsNotFound()
    {
        //Arrange
        var requestId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var approveRequestDto = new ApproveRequestDto
        {
            RequestId = requestId,
            EmployeeId = employeeId,
            ApprovalStatusId = 3
        };
        var command = new ApproveRequestCommand { ApproveRequestDto = approveRequestDto };
        var employee = GeneralEmployee.Create(
            "parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "3453454")
        );
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(RequestErrors.NotFound());
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
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
    public async void Handler_Should_ReturnFailureResultUnauthorize_WhenEmployeeIsNotFound()
    {
        //Arrange
        var requestId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var approveRequestDto = new ApproveRequestDto
        {
            RequestId = requestId,
            EmployeeId = employeeId,
            ApprovalStatusId = 3
        };
        var command = new ApproveRequestCommand { ApproveRequestDto = approveRequestDto };
        var employee = GeneralEmployee.Create(
            "parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "3453454")
        );
        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            "sick"
        );
        request.SetGuidId(requestId);
        employee.SubmitRequest(request);
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);
        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((Employee?)null);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(EmployeeErrors.Unauthorize());
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
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
    public async void Handler_Should_ReturnSuccessResultOk_WhenValidRequest()
    {
        //Arrange
        var requestId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var approveRequestDto = new ApproveRequestDto
        {
            RequestId = requestId,
            EmployeeId = employeeId,
            ApprovalStatusId = 3
        };
        var command = new ApproveRequestCommand { ApproveRequestDto = approveRequestDto };
        var employee = GeneralEmployee.Create(
            "parivesh",
            "parivesh@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "3453454")
        );
        var currentEmployee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("Charali", "Jhapa", "3453454")
        );
        currentEmployee.SetId(2);
        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            "sick"
        );
        request.SetGuidId(requestId);
        request.SetId(10);
        employee.SubmitRequest(request);
        _employeeRepositoryMock
            .Setup(x => x.GetEmployeeByGuidIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);
        _employeeRepositoryMock
            .Setup(x =>
                x.GetEmployeeByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(currentEmployee);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.Success.Should().BeTrue();
        _employeeRepositoryMock.Verify(
            x => x.GetEmployeeByGuidIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        _employeeRepositoryMock.Verify(
            x =>
                x.UpdateEmployeeAsync(
                    It.IsAny<Employee>(),
                    It.IsAny<Employee>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
        result.Data.Should().Be(10);
    }
}
