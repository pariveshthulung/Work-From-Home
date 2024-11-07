using AutoMapper;
using Clean.Application.Dto.Enum;
using Clean.Application.Persistence.Contract;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.RequestTypeEnum.Queries;

public class RequestTypeEnumQueryHandler
    : IRequestHandler<RequestTypeEnumQuery, BaseResult<List<RequestTypeEnumDto>>>
{
    public readonly IEnumRepository _enumRepository;
    public readonly IMapper _mapper;

    public RequestTypeEnumQueryHandler(IEnumRepository enumRepository, IMapper mapper)
    {
        _enumRepository = enumRepository;
        _mapper = mapper;
    }

    public async Task<BaseResult<List<RequestTypeEnumDto>>> Handle(
        RequestTypeEnumQuery request,
        CancellationToken cancellationToken
    )
    {
        var requestTypeEnum = await _enumRepository.GetRequestedTypeAsync(cancellationToken);
        return BaseResult<List<RequestTypeEnumDto>>.Ok(
            _mapper.Map<List<RequestTypeEnumDto>>(requestTypeEnum)
        );
    }
}
