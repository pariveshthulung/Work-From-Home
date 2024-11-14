using Clean.Domain.Entities;
using Clean.Domain.Exceptions;
using Xunit;

namespace Clean.Test.DomainTests;

public class EmployeeTest
{
    [Theory]
    [InlineData("hari", "harigmail.com")]
    public void Constructor_Should_ThrowException_When_Email_Is_Invalid(string name, string email)
    {
        //Arrange

        //Act

        //Assert
        Assert.Throws<InvalidEmailException>(
            () =>
                GeneralEmployee.Create(
                    name,
                    email,
                    "9876543210",
                    1,
                    Address.Create("ktm", "ktm", "1234")
                )
        );
    }

    [Theory]
    [InlineData("")]
    public void Constructor_Should_ThrowException_When_Name_Is_WhiteSpace(string name)
    {
        //Arrange

        //Act

        //Assert
        Assert.Throws<ArgumentException>(
            () =>
                GeneralEmployee.Create(
                    name,
                    "hari@gmai.com",
                    "9876543210",
                    1,
                    Address.Create("ktm", "ktm", "1234")
                )
        );
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_PhoneNumber_Is_Invalid()
    {
        //Arrange

        //Act

        //Assert
        Assert.Throws<InvalidPhoneNoException>(
            () =>
                GeneralEmployee.Create(
                    "hari",
                    "hari@gmai.com",
                    "2343",
                    1,
                    Address.Create("ktm", "ktm", "1234")
                )
        );
    }

    [Fact]
    public void Constructor_Should_Success_When_All_Value_Is_Valid()
    {
        //Arrange

        //Act
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );
        //Assert
        Assert.IsType<GeneralEmployee>(employee);
        Assert.Equal("hari", employee.Name);
        Assert.Equal("hari@gmail.com", employee.Email);
    }

    [Fact]
    public void Update_Should_ThrowException_When_Invalid_Email()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );

        //Act

        //Assert

        Assert.Throws<InvalidEmailException>(
            () => employee.Update("hari", "email", "9876543219", 1)
        );
    }

    [Fact]
    public void Update_Should_ThrowException_When_Empty_Name()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );

        //Act

        //Assert

        Assert.Throws<ArgumentException>(
            () => employee.Update("", "email@yahoo.com", "9876543219", 1)
        );
    }

    [Fact]
    public void Update_Should_ThrowException_When_Invalid_PhoneNumber()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );

        //Act

        //Assert

        Assert.Throws<InvalidPhoneNoException>(
            () => employee.Update("hari", "email@yahoo.com", "9876543", 1)
        );
    }

    [Fact]
    public void Update_Should_Sucess_When_All_Valid_Value()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );

        //Act
        employee.Update("Bahadur", "email@yahoo.com", "9876543123", 1);

        //Assert
        Assert.Equal("Bahadur", employee.Name);
        Assert.Equal("email@yahoo.com", employee.Email);
    }

    [Fact]
    public void UpdateEmployeeAddress_Should_Sucess_When_All_Valid_Value()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );

        //Act
        employee.UpdateEmployeeAddress(Address.Create("Btm", "Btm", "123"));

        //Assert
        Assert.Equal("Btm", employee.Address!.City);
        Assert.Equal("Btm", employee.Address.Street);
    }

    [Fact]
    public void SubmitRequest_Should_Submit_When_Request_Is_Valid()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );
        employee.SetId(1);

        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            "felling unwell"
        );
        //Act
        employee.SubmitRequest(request);

        //Assert
        Assert.Contains(request, employee.Requests);
    }

    [Fact]
    public void ApproveRequest_Should_Approve_When_Request_Is_Valid()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );
        employee.SetId(1);

        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            "felling unwell"
        );
        employee.SubmitRequest(request);
        //Act
        employee.ApproveRequest(request, 4);

        //Assert
        Assert.Contains(request, employee.Requests);
        Assert.Equal(
            4,
            employee.Requests.Select(x => x.Approval!.ApprovalStatusId).FirstOrDefault()
        );
    }

    [Fact]
    public void DeleteRequest_Should_Delete_When_Request_Is_Valid()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );
        employee.SetId(1);

        var request = GeneralRequest.Create(
            1,
            2,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            "felling unwell"
        );
        employee.SubmitRequest(request);
        //Act
        employee.DeleteRequest(request);

        //Assert
        Assert.DoesNotContain(request, employee.Requests);
    }

    [Fact]
    public void SetAppUserId_Should_Set_AppUserId()
    {
        //Arrange
        var employee = GeneralEmployee.Create(
            "hari",
            "hari@gmail.com",
            "9876543210",
            1,
            Address.Create("ktm", "ktm", "1234")
        );
        employee.SetId(1);

        //Act
        employee.SetAppUserId(2);

        //Assert
        Assert.Equal(2, employee.AppUserId);
    }
}
