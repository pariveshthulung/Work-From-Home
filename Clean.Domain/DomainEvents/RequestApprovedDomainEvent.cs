using System;
using Clean.Domain.Entities;
using Clean.Domain.Primitive;

namespace Clean.Domain.DomainEvents;

public record RequestApprovedDomainEvent(Request Request) : IDomainEvent { }
