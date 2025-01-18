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
        public string Description { get; private set; } = default!;
        public Approval? Approval { get; private set; }
        public DateTime ToDate { get; private set; }
        public DateTime FromDate { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }

        protected Request() { }

        protected Request(
            int requestBy,
            int requestTo,
            DateTime fromDate,
            DateTime toDate,
            string description
        )
        {
            RequestedBy = requestBy;
            RequestedTo = requestTo;
            FromDate = ValidationGuard.ValidateFromDate(fromDate, nameof(FromDate));
            ToDate = ValidationGuard.ValidateToDate(toDate, fromDate, nameof(ToDate));
            Description = ValidationGuard.ValidateString(description, nameof(Description));
            RequestedTypeId = RequestTypeEnum.WorkFromHome.Id;
        }

        public virtual void Submit(int requestId)
        {
            Approval = Approval.Create(requestId, ApprovalStatusEnum.FromName("Pending").Id);
        }
    }
}
