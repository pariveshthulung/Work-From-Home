using System;
using AutoMapper;
using Clean.Application.Dto.Enum;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using Clean.Domain.Entities;
using MediatR;

namespace Clean.Application.Feature.ApprovalStatusEnums.Queries;

public class ApproveStatusEnumQueryHandler
    : IRequestHandler<ApproveStatusEnumQuery, BaseResult<List<ApprovalStatusEnumDto>>>
{
    private readonly IEnumRepository _enumRepository;
    private readonly IMapper _mapper;

    public ApproveStatusEnumQueryHandler(IEnumRepository enumRepository, IMapper mapper)
    {
        _enumRepository = enumRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<ApprovalStatusEnumDto>>> Handle(
        ApproveStatusEnumQuery request,
        CancellationToken cancellationToken
    )
    {
        List<ApprovalStatus> status = await _enumRepository.GetApprovalStatusAsync(
            cancellationToken
        );
        return BaseResult<List<ApprovalStatusEnumDto>>.Ok(
            _mapper.Map<List<ApprovalStatusEnumDto>>(status)
        );
    }
}
