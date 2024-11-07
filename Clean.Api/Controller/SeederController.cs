// using Clean.Application.Dto.Employee;
// using Clean.Application.Persistence.Contract;
// using Clean.Domain.Entities;
// using Clean.Domain.Enums;
// using Clean.Infrastructure.Data;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace Clean.Api.Controller
// {
//     [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
//     [ApiController]
//     public class SeederController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly UserManager<AppUser> _userManager;
//         private readonly RoleManager<UserRole> _roleManager;

//         public SeederController(
//             ApplicationDbContext context,
//             IEmployeeRepository employeeRepository,
//             UserManager<AppUser> userManager,
//             RoleManager<UserRole> roleManager
//         )
//         {
//             _context = context;
//             _userManager = userManager;
//             _roleManager = roleManager;
//         }

//         [HttpPost("seedapprovalstatus")]
//         public async Task<IActionResult> SeedApprovalStatusAsync()
//         {
//             try
//             {
//                 var exist = await _context.ApprovalStatuses.ToListAsync();
//                 if (exist.Count > 0)
//                     return Ok("Already approval exist");
//                 using var transaction = _context.Database.BeginTransaction();
//                 var approvalStatuses = new List<ApprovalStatus>()
//                 {
//                     ApprovalStatus.Create(1, "Draft"),
//                     ApprovalStatus.Create(2, "Pending"),
//                     ApprovalStatus.Create(3, "Accepted"),
//                     ApprovalStatus.Create(4, "Rejected"),
//                 };
//                 _context.Database.ExecuteSqlRaw(
//                     "SET IDENTITY_INSERT [WorkFromHomeDb].[dbo].[ApprovalStatuses] ON;"
//                 );
//                 _context.ApprovalStatuses.AddRange(approvalStatuses);
//                 await _context.SaveChangesAsync();
//                 _context.Database.ExecuteSqlRaw(
//                     "SET IDENTITY_INSERT [WorkFromHomeDb].[dbo].[ApprovalStatuses] OFF;"
//                 );
//                 transaction.Commit();
//                 return Ok("Approval Status added.");
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }
//         }

//         [HttpPost("seedsuperadmin")]
//         public async Task<IActionResult> SeedSuperAdminAsync()
//         {
//             try
//             {
//                 using var tx = _context.Database.BeginTransaction();
//                 var superadminExist = _context.Employees.Any(emp =>
//                     emp.UserRoleId == UserRoleEnum.SuperAdmin.Id
//                 );
//                 if (superadminExist)
//                     return BadRequest("Super admin already exist!");

//                 var superAdmin = new RegisterEmployeeDto()
//                 {
//                     Name = "SuperAdmin",
//                     Email = "superadmin@gmail.com",
//                     Password = "@Admin123",
//                     Role = "SuperAdmin",
//                     PhoneNumber = "9876543210",
//                     Address = Address.Create("admin", "admin", "admin").ToAddressDto()
//                 };
//                 var employeeAdmin = superAdmin.ToEmployee();

//                 AppUser appUser =
//                     new() { UserName = employeeAdmin.Name, Email = employeeAdmin.Email, };

//                 var result = await _userManager.CreateAsync(appUser, superAdmin.Password);

//                 if (result.Succeeded)
//                 {
//                     var role = UserRoleEnum.FromName(superAdmin.Role);
//                     await _userManager.AddToRoleAsync(appUser, role.Name);
//                     _context.Database.ExecuteSqlRaw(
//                         "SET IDENTITY_INSERT [WorkFromHomeDb].[dbo].[Employees] ON;"
//                     );
//                     employeeAdmin.SetAppUserId(appUser.Id);
//                     employeeAdmin.SetId(1);
//                     await _context.Employees.AddAsync(employeeAdmin);
//                     await _context.SaveChangesAsync();
//                     _context.Database.ExecuteSqlRaw(
//                         "SET IDENTITY_INSERT [WorkFromHomeDb].[dbo].[Employees] OFF;"
//                     );
//                     tx.Commit();
//                     return Ok(employeeAdmin.ToEmployeeDto());
//                 }
//                 else
//                 {
//                     return BadRequest(result.Errors);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }
//         }

//         [HttpPost("seedrequesttype")]
//         public async Task<IActionResult> SeedRequestedTypeAsync()
//         {
//             try
//             {
//                 var exist = await _context.RequestedTypes.ToListAsync();
//                 if (exist.Count > 0)
//                     return Ok("Already requesttype exist");
//                 using var transaction = _context.Database.BeginTransaction();
//                 var requestedTypes = new List<RequestedType>()
//                 {
//                     RequestedType.Create(1, "WorkFromHome"),
//                     RequestedType.Create(2, "WorkFromOffice"),
//                     RequestedType.Create(3, "Leave"),
//                 };
//                 _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[RequestedTypes] ON;");
//                 await _context.RequestedTypes.AddRangeAsync(requestedTypes);
//                 await _context.SaveChangesAsync();
//                 _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[RequestedTypes] OFF;");
//                 transaction.Commit();

//                 return Ok("RequestedTypes added.");
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }
//         }

//         [HttpPost("seedroletype")]
//         public async Task<IActionResult> SeedRoleTypeAsync()
//         {
//             try
//             {
//                 var exist = await _context.UserRoles.ToListAsync();
//                 if (exist.Count > 0)
//                     return Ok("Already Roletype exist");
//                 using var transaction = _context.Database.BeginTransaction();
//                 var userRoles = new List<UserRole>()
//                 {
//                     UserRole.Create(1, "SuperAdmin"),
//                     UserRole.Create(2, "Admin"),
//                     UserRole.Create(3, "Ceo"),
//                     UserRole.Create(4, "Manager"),
//                     UserRole.Create(5, "Intern"),
//                     UserRole.Create(6, "Developer"),
//                 };
//                 _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[AspNetRoles] ON;");
//                 foreach (var role in userRoles)
//                 {
//                     var result = await _roleManager.CreateAsync(role);
//                     if (!result.Succeeded)
//                     {
//                         throw new Exception(
//                             $"Failed to create role: {role.Name}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}"
//                         );
//                     }
//                 }
//                 await _context.SaveChangesAsync();
//                 _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF;");
//                 transaction.Commit();

//                 return Ok("Users' roles added.");
//             }
//             catch (Exception ex)
//             {
//                 return BadRequest(ex.Message);
//             }
//         }
//     }
// }
