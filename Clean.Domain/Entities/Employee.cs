using Clean.Domain.Primitive;

namespace Clean.Domain.Entities
{
    public abstract class Employee : AggregateRoot
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public int UserRoleId { get; private set; }
        public UserRole? UserRole { get; private set; }
        public string PhoneNumber { get; private set; } = default!;

        public Address? Address { get; set; }
        private List<Request> _request = [];
        public IReadOnlyCollection<Request> Requests => _request;
        public int AppUserId { get; private set; }
        public AppUser? AppUser { get; private set; }

        protected Employee() { }

        protected Employee(
            string name,
            string email,
            string phoneNumber,
            int userRoleId,
            Address address
        )
        {
            Name = ValidationGuard.ValidateString(name, nameof(name));
            Email = ValidationGuard.ValidateEmail(email);
            PhoneNumber = ValidationGuard.ValidatePhoneNumber(phoneNumber);
            UserRoleId = userRoleId;
            // Address = address;
        }

        public void Update(string name, string email, string phoneNumber, int userRoleId)
        {
            Name = ValidationGuard.ValidateString(name, nameof(name));
            Email = ValidationGuard.ValidateEmail(email);
            PhoneNumber = ValidationGuard.ValidatePhoneNumber(phoneNumber);
            UserRoleId = userRoleId;
            UpdatedOn = DateTime.UtcNow;
        }

        public void UpdateEmployeeAddress(Address address)
        {
            this.Address = address;
        }

        public Request SubmitRequest(Request request)
        {
            try
            {
                request.Submit(request.Id);
                _request.Add(request);
                request.SetAddedBy(request.RequestedBy);
                request.SetAddedOn(DateTime.UtcNow);
                // RaiseDomainEvent(new RequestSubmittedDomainEvent(request));
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void ApproveRequest(Request request, int approvalStatusId)
        {
            try
            {
                if (request.Approval is null)
                    throw new ArgumentNullException(nameof(request));
                request.Approval.SetApproverId(Id);
                request.Approval.SetStatus(approvalStatusId);
                request.Approval.SetUpdatedBy(Id);
                request.Approval.SetUpdatdOn(DateTime.UtcNow);
                // RaiseDomainEvent(new RequestApprovedDomainEvent(request));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void DeleteRequest(Request request)
        {
            _request.Remove(request);
        }

        public void SetAppUserId(int id)
        {
            AppUserId = id;
        }

        public void SetAppUser(AppUser appUser)
        {
            AppUser = appUser;
        }
    }
}
