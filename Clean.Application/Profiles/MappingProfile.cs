using System;
using AutoMapper;
using Clean.Application.Dto.Address;
using Clean.Application.Dto.Approval;
using Clean.Application.Dto.Employee;
using Clean.Application.Dto.Enum;
using Clean.Application.Dto.Request;
using Clean.Application.Helper;
using Clean.Domain.Entities;
using Clean.Domain.Entities.StoreProcedure;

namespace Clean.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Request, GeneralRequest>().IncludeAllDerived();
        CreateMap<GeneralRequest, CreateRequestDto>().ReverseMap();
        CreateMap<GeneralRequest, RequestDto>()
            // .ForMember(x => x.RequestedByEmail, y => y.MapFrom(s => s.RequestedByEmployee.Email))
            .ForMember(x => x.RequestedByEmail, y => y.MapFrom(s => s.Employee.Email))
            .ForMember(x => x.RequestedToEmail, y => y.MapFrom(s => s.RequestedToEmployee.Email))
            .ForMember(x => x.RequestedType, y => y.MapFrom(s => s.RequestedType.Type))
            .ForMember(x => x.EmployeeGuidId, y => y.MapFrom(s => s.Employee.GuidId))
            .ReverseMap();
        CreateMap<Employee, GeneralEmployee>().ReverseMap();
        CreateMap<GeneralEmployee, RegisterEmployeeDto>().ReverseMap();
        CreateMap<GeneralEmployee, GetAllEmployees>().ReverseMap();
        CreateMap<GeneralEmployee, EmployeeDto>()
            .ForMember(x => x.UserRole, y => y.MapFrom(s => s.UserRole.Name))
            .ReverseMap();
        CreateMap<GeneralEmployee, CreateEmployeeDto>().ReverseMap();
        CreateMap<GeneralEmployee, UpdateEmployeeDto>().ReverseMap();
        CreateMap<UserRole, UserRoleEnumDto>().ReverseMap();
        CreateMap<RequestedType, RequestTypeEnumDto>().ReverseMap();
        CreateMap<ApprovalStatus, ApprovalStatusEnumDto>().ReverseMap();
        // CreateMap<PagedList<EmployeeDto>, Employee>().ReverseMap();
        CreateMap<Approval, ApprovalDto>()
            .ForMember(x => x.ApprovalStatus, y => y.MapFrom(s => s.ApprovalStatus.Status))
            .ForMember(
                x => x.ApproverEmail,
                y => y.MapFrom(s => s.Request.RequestedToEmployee.Email)
            )
            .ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}
