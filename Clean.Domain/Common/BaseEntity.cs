using System;

namespace Clean.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }
        public Guid GuidId { get; protected set; }
        public virtual int? UpdatedBy { get; protected set; }
        public virtual DateTime? UpdatedOn { get; protected set; }
        public virtual DateTime AddedOn { get; protected set; }
        public virtual int? AddedBy { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected BaseEntity()
        {
            AddedOn = DateTime.Now;
            IsDeleted = false;
            GuidId = Guid.NewGuid();
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetGuidId(Guid guidId)
        {
            GuidId = guidId;
        }

        public void SetAddedBy(int id)
        {
            AddedBy = id;
        }

        public void SetAddedOn(DateTime dateTime)
        {
            AddedOn = dateTime;
        }

        public void SetUpdatedBy(int id)
        {
            UpdatedBy = id;
        }

        public void SetUpdatdOn(DateTime dateTime)
        {
            UpdatedOn = dateTime;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}
