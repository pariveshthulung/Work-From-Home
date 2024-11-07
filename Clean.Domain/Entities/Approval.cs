using Clean.Domain.Common;
using Clean.Domain.Interface;

namespace Clean.Domain.Entities
{
    public class Approval : BaseEntity, ISoftDelete
    {
        public int ApprovalStatusId { get; private set; }
        public ApprovalStatus? ApprovalStatus { get; private set; }
        public int ApproverId { get; private set; }

        public Employee? ApproverIdEmployee { get; private set; }
        public int RequestId { get; private set; }
        public Request? Request { get; private set; }

        private Approval() { }

        private Approval(int requestId, int approvalStatusId)
        {
            ApprovalStatusId = approvalStatusId;
            RequestId = requestId;
        }

        public static Approval Create(int requestId, int approvalStatusId)
        {
            return new Approval(requestId, approvalStatusId);
        }

        public void SetStatus(int approvalStatusId)
        {
            ApprovalStatusId = approvalStatusId;
        }

        public void SetApproverId(int approveId)
        {
            ApproverId = approveId;
        }

        public void SetRequestId(int requestId)
        {
            RequestId = requestId;
        }
    }
}
