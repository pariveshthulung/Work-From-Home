using Clean.Application.Dto.Approval;
using Clean.Application.Dto.Request;
using Clean.Domain.Entities;
using Clean.Domain.Enums;

namespace Clean.Application.Mapper;

public static class RequestMapper
{
    public static Request ToRequest(this RequestDto requestDto)
    {
        var request = GeneralRequest.Create(
            requestDto.RequestedBy,
            requestDto.RequestedTo,
            requestDto.RequestedTypeId,
            requestDto.FromDate,
            requestDto.ToDate
        );
        if (requestDto.Id > 0)
        {
            request.SetId(requestDto.Id);
        }
        return request;
    }

    // public static RequestDto ToRequestDto(this Request request)
    // {
    //     return new RequestDto
    //     {
    //         Id = request.Id,
    //         RequestedTo = request.RequestedTo,
    //         RequestedBy = request.RequestedBy,
    //         RequestedTypeId = request.RequestedTypeId,
    //         ToDate = request.ToDate,
    //         RequestedType = RequestTypeEnum.FromId(request.RequestedTypeId).Name,
    //         FromDate = request.FromDate,
    //         Approval = new ApprovalDto
    //         {
    //             ApprovalStatusId = request.Approval.ApprovalStatusId,
    //             ApproverId = request.Approval.ApproverId,
    //             RequestId = request.Approval.RequestId,
    //             ApprovalStatus = ApprovalStatusEnum.FromId(request.Approval.ApprovalStatusId).Name
    //         },
    //         GuidId = request.GuidId,
    //         AddedBy = request.AddedBy,
    //         AddedOn = request.AddedOn,
    //         UpdatedBy = request.UpdatedBy,
    //         UpdatedOn = request.UpdatedOn,
    //     };
    // }

    // public static Request ToCreateRequest(this CreateRequestDto request)
    // {
    //     return GeneralRequest.Create(
    //         request.RequestedBy,
    //         request.RequestedTo,
    //         request.RequestedTypeId,
    //         request.FromDate,
    //         request.ToDate
    //     );
    // }
}
