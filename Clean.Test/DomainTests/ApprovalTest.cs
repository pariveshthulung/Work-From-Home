using Clean.Domain.Entities;
using Xunit;

namespace Clean.Test.DomainTests;

public class ApprovalTest
{
    [Fact]
    public void Create_Should_Success_When_All_Valid_Value()
    {
        //Arrange
        //Act
        var approval = Approval.Create(1, 2);
        //Assert
        Assert.Equal(1, approval.RequestId);
        Assert.Equal(2, approval.ApprovalStatusId);
    }

    [Fact]
    public void SetStatus_Should_Change_Status()
    {
        //Arrange
        var approval = Approval.Create(1, 2);
        //Act
        approval.SetStatus(4);
        //Assert
        Assert.Equal(4, approval.ApprovalStatusId);
    }

    [Fact]
    public void SetApproverId_Should_Set_ApproverId()
    {
        //Arrange
        var approval = Approval.Create(1, 2);
        //Act
        approval.SetApproverId(2);
        //Assert
        Assert.Equal(2, approval.ApproverId);
    }

    [Fact]
    public void SetRequestId_Should_Set_RequestId()
    {
        //Arrange
        var approval = Approval.Create(1, 2);
        //Act
        approval.SetRequestId(4);
        //Assert
        Assert.Equal(4, approval.RequestId);
    }
}
