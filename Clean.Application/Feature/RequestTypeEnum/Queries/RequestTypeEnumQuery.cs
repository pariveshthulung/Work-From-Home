using System;
using Clean.Application.Dto.Enum;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.RequestTypeEnum.Queries;

public class RequestTypeEnumQuery : IRequest<BaseResult<List<RequestTypeEnumDto>>> { }
