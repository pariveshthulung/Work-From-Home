using AutoMapper;
using Clean.Application.Feature.Employees.Handlers.Queries;
using Clean.Application.Persistence.Contract;
using Moq;

namespace Clean.Test.ApplicationTest.Features.Product.Query;

public class GetEmployeeListQueryHandlerTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEmployeeListQueryHandler _handler;

    public GetEmployeeListQueryHandlerTest()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetEmployeeListQueryHandler(
            _employeeRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    // [Fact]
    // public async void Handler_Should_ReturnSuccessEmployeeList_WhenEmployeesExist()
    // {
    //     //Arrange
    //     var query = new GetEmployeeListQuery();
    //     var employee = GeneralEmployee.Create(
    //         "Parivesh",
    //         "parivesh@gmail.com",
    //         "9876543210",
    //         1,
    //         Address.Create("Charali", "Jhapa", "23343")
    //     );

    //     _employeeRepositoryMock
    //         .Setup(x =>
    //             x.GetAllEmployeeAsync(
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>(),
    //                 It.IsAny<int>(),
    //                 It.IsAny<int>(),
    //                 It.IsAny<CancellationToken>()
    //             )
    //         )
    //         .ReturnsAsync([employee]);
    //     //Act
    //     var result = await _handler.Handle(query, default);
    //     //Assert
    //     result.Success.Should().BeTrue();
    // }

    // [Fact]
    // public async void Handler_Should_ReturnSuccessWithNoData_WhenEmployeesDoesNotExist()
    // {
    //     //Arrange
    //     var query = new GetEmployeeListQuery();
    //     _employeeRepositoryMock
    //         .Setup(x =>
    //             x.GetAllEmployeeAsync(
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>(),
    //                 It.IsAny<string>(),
    //                 It.IsAny<int>(),
    //                 It.IsAny<int>(),
    //                 It.IsAny<CancellationToken>()
    //             )
    //         )
    //         .ReturnsAsync([]);
    //     //Act
    //     var result = await _handler.Handle(query, default);
    //     //Assert
    //     result.Success.Should().BeTrue();
    // }
}
