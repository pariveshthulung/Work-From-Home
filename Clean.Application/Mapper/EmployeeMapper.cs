// using Clean.Application.Dto.Employee;
// using Clean.Domain.Entities;
// using Clean.Domain.Entities.StoreProcedure;
// using Clean.Domain.Enums;

// namespace Clean.Application.Mapper;

// public static class EmployeeMapper
// {
//     public static EmployeeDto ToEmployeeDto(this Employee employee)
//     {
//         return new EmployeeDto
//         {
//             Id = employee.Id,
//             GuidId = employee.GuidId,
//             Name = employee.Name,
//             Email = employee.Email,
//             UserRoleId = employee.UserRoleId,
//             PhoneNumber = employee.PhoneNumber,
//             UserRole = UserRoleEnum.FromId(employee.UserRoleId).Name,
//             Address = employee.Address?.ToAddressDto(),
//             Request = employee.Requests.Select(x => x.ToRequestDto()).ToList(),
//             AddedBy = employee.AddedBy,
//             AddedOn = employee.AddedOn,
//             UpdatedBy = employee.UpdatedBy,
//             UpdatedOn = employee.UpdatedOn,
//         };
//     }

//     public static Employee ToEmployee(this EmployeeDto employeeDto)
//     {
//         // var address = employeeDto.Address.ToAddress();

//         var employee = GeneralEmployee.Create(
//             employeeDto.Name,
//             employeeDto.Email,
//             employeeDto.PhoneNumber,
//             employeeDto.UserRoleId,
//             employeeDto.Address.ToAddress()
//         );
//         if (employeeDto.Id > 0)
//             employee.SetId(employeeDto.Id);

//         return employee;
//     }

//     public static Employee ToEmployee(this GetAllEmployees employeeDto)
//     {
//         // var address = employeeDto.Address.ToAddress();

//         var employee = GeneralEmployee.Create(
//             employeeDto.Name,
//             employeeDto.Email,
//             employeeDto.PhoneNumber,
//             employeeDto.UserRoleId,
//             Address.Create(employeeDto.Street, employeeDto.City, employeeDto.PostalCode)
//         );
//         if (employeeDto.Id > 0)
//             employee.SetId(employeeDto.Id);

//         return employee;
//     }

//     public static Employee ToCreateEmployee(this CreateEmployeeDto employee)
//     {
//         return GeneralEmployee.Create(
//             employee.Name,
//             employee.Email,
//             employee.PhoneNumber,
//             employee.UserRoleId,
//             employee.Address.ToAddress()
//         );
//     }

//     public static Employee ToUpdateEmployee(this UpdateEmployeeDto employeeDto)
//     {
//         var employee = GeneralEmployee.Create(
//             employeeDto.Name,
//             employeeDto.Email,
//             employeeDto.PhoneNumber,
//             employeeDto.UserRoleId,
//             employeeDto.Address.ToAddress()
//         );
//         employee.SetId(employeeDto.Id);
//         return employee;
//     }

//     public static Employee ToEmployee(this RegisterEmployeeDto employeeDto)
//     {
//         var role = UserRoleEnum.FromName(employeeDto.UserRole);
//         var employee = GeneralEmployee.Create(
//             employeeDto.Name,
//             employeeDto.Email,
//             employeeDto.PhoneNumber,
//             role.Id,
//             employeeDto.Address?.ToAddress()
//         );
//         return employee;
//     }
// }
