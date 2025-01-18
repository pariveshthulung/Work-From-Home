using Clean.Domain.Entities;
using WorkFromHome.Domain.Exceptions;
using Xunit;

namespace Clean.Test.DomainTests;

public class RequestTest
{
    [Fact]
    public void Create_Should_ReturnException_When_ToDate_Is_Less_Then_FromDate()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<DateTimeException>(
            () =>
                GeneralRequest.Create(
                    1,
                    2,
                    DateTime.UtcNow.AddDays(2),
                    DateTime.UtcNow,
                    "felling unwell"
                )
        );
    }

    [Fact]
    public void Create_Should_ReturnException_When_FromDate_Is_Greater_Then_ToDate()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<DateTimeException>(
            () =>
                GeneralRequest.Create(
                    1,
                    2,
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow,
                    "felling unwell"
                )
        );
    }

    [Fact]
    public void Create_Should_ReturnException_When_Description_Is_WhiteSpaceOrEmpty()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<ArgumentException>(
            () => GeneralRequest.Create(1, 2, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "")
        );
    }

    [Fact]
    public void Create_Should_Sucess_When_All_Value_Valid()
    {
        //Arrange
        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            "felling unwell"
        );
        //Act

        //Assert
        Assert.IsType<GeneralRequest>(request);
        Assert.Equal(1, request.RequestedBy);
        Assert.Equal(2, request.RequestedTo);
        Assert.Equal("felling unwell", request.Description);
    }

    [Fact]
    public void Submit_Should_Create_Approval_When_Success()
    {
        //Arrange
        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            "felling unwell"
        );
        //Act
        request.Submit(1);

        //Assert
        Assert.IsType<GeneralRequest>(request);
        Assert.Equal(1, request.Approval!.RequestId);
    }
}
