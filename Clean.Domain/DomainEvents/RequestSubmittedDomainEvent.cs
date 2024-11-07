using Clean.Domain.Entities;
using Clean.Domain.Primitive;

namespace Clean.Domain.DomainEvents;

public record RequestSubmittedDomainEvent(Request Request) : IDomainEvent { }
