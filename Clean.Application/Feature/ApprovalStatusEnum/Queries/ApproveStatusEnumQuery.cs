using System;
using Clean.Application.Dto.Enum;
using Clean.Application.Wrappers;
using MediatR;

namespace Clean.Application.Feature.ApprovalStatusEnums.Queries;

public class ApproveStatusEnumQuery : IRequest<BaseResult<List<ApprovalStatusEnumDto>>> { }
