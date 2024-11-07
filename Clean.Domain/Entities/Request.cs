// xusing System;
using Clean.Domain.Common;
using Clean.Domain.Enums;
using Clean.Domain.Interface;

namespace Clean.Domain.Entities
{
    public abstract class Request : BaseEntity, ISoftDelete
    {
        public int RequestedBy { get; private set; }
        public Employee? RequestedByEmployee { get; private set; }
        public int RequestedTo { get; private set; }
        public Employee? RequestedToEmployee { get; private set; }
        public int RequestedTypeId { get; private set; }
        public RequestedType? RequestedType { get; private set; }
        public Approval? Approval { get; private set; }
        public DateTime ToDate { get; private set; }
        public DateTime FromDate { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }

        protected Request() { }

        protected Request(
            int requestBy,
            int requestTo,
            int requestedTypeId,
            DateTime fromDate,
            DateTime toDate
        )
        {
            RequestedBy = requestBy;
            RequestedTo = requestTo;
            RequestedTypeId = requestedTypeId;
            FromDate = ValidationGuard.ValidateFromDate(fromDate, nameof(FromDate));
            ToDate = ValidationGuard.ValidateToDate(toDate, fromDate, nameof(ToDate));
        }

        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetGuiId(Guid guidId)
        {
            GuidId = guidId;
        }

        public virtual void Submit(int requestId)
        {
            Approval = Approval.Create(requestId, ApprovalStatusEnum.FromName("Pending").Id);
        }
    }
}
