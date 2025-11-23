
# Work-From-Home

Work-From-Home is a sample full‑stack application demonstrating a clean separation between UI, application logic and infrastructure. It focuses on:
- A small domain for time/attendance or user-related features.
- A React-based UI for user interactions.
- Modern authentication (Okta / OIDC) for sign-in and secure API calls.
- Clean Architecture and Domain-Driven Design (DDD) to keep business rules independent and testable.

## Architecture (high level)
The code is organized using Clean Architecture (Onion/Hexagonal style). Layers and responsibilities:

- Presentation (UI)
  - React SPA, routing, components, auth wrappers.
- Application
  - Use-cases / interactors that orchestrate domain operations and enforce application-level rules.
- Domain
  - Entities, value objects, domain services, and domain events — pure business logic, no framework dependencies.
- Infrastructure (Adapters)
  - Database repositories, external integrations (Okta, email), HTTP controllers — concrete implementations wired at the outer edge.

ASCII diagram (data / control flow):
  
    [React UI]
        |
        v
    [Presentation / Controllers]  <- handles HTTP/REST/UI events
        |
        v
    [Application / Use-cases]     <- orchestrates domain actions, authorization checks
        |
        v
    [Domain (Entities / Services)]<- core business rules, invariants
        |
        v
    [Infrastructure / Repositories & Adapters] <- DB, Okta token verification, 3rd-party APIs

Dependencies point inward: outer layers depend on abstractions defined by inner layers (interfaces), and concrete adapters are injected at runtime.

## Design patterns used
- Clean Architecture / Hexagonal: clear layer separation and dependency rule (inward).
- Domain-Driven Design (DDD) concepts:
  - Entities and Value Objects
  - Aggregates and Aggregate Roots
  - Repositories (interfaces in domain/application; adapters in infrastructure)
  - Domain Events for side-effects and decoupled integrations
  - Bounded Contexts (separate concerns like Auth, User, TimeTracking)
- Dependency Injection: inject repositories/adapters into use-cases to keep domain testable.
- Adapter/Port pattern (Anti-Corruption Layer) for integrating with Okta and external APIs.
- Authorization at the application boundary (use-case level), not inside entities.

## Typical runtime workflow
1. User opens the React UI and triggers sign-in.
2. React initiates OIDC flow (Authorization Code + PKCE) with Okta and receives ID + access tokens.
3. UI calls backend API attaching Authorization: Bearer <access_token>.
4. Backend middleware verifies token (issuer, audience, signature) via Okta JWKS or SDK.
5. Verified request proceeds to a Use-case interactor with an authenticated user context.
6. Use-case applies business rules using Domain entities and uses repository interfaces for persistence.
7. Infrastructure adapter implements repository operations and returns results to the use-case.
8. Response is returned to the UI
