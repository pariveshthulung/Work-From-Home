using System;
using AutoMapper;
using Clean.Application.Dto.Request;
using Clean.Application.Feature.Request.Requests.Queries;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.Request.Handlers.Queries;

public class GetAllUserRequestQueryHandler
    : IRequestHandler<GetAllUserRequestQuery, BaseResult<List<RequestDto>>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetAllUserRequestQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<RequestDto>>> Handle(
        GetAllUserRequestQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByEmailAsync(
                request.Email,
                cancellationToken
            );
            if (employee is null)
                return BaseResult<List<RequestDto>>.Failure(EmployeeErrors.NotFound());
            return BaseResult<List<RequestDto>>.Ok(
                _mapper.Map<List<RequestDto>>(employee.Requests.ToList())
            );
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
