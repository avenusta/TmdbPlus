# TmdbPlus

Strongly typed .NET wrapper for the TMDB API (v3 + v4).

## Goal

Create a client with a read access token, call endpoints, get typed results back. Classes plus interfaces for every request and response — no `dynamic`, no raw JSON handed to callers.

## Stack

- net10.0, nullable enabled, implicit usings
- `System.Text.Json` + `HttpClient` (no third-party deps)

## Conventions

- Responses are mutable `class`es (`{ get; set; }`) with `[JsonPropertyName]` for TMDB's snake_case.
  Not records: init-only properties break EF Core change tracking, and `with` yields a second
  instance with the same key. See issue #4.
- Every response type implements a matching `I`-prefixed interface with **settable** members —
  a schema contract consuming apps implement on their own EF entities, so mapping is written
  once per interface rather than once per type. Interfaces never dictate keys or FKs.
- Nested collections on interfaces are `IList<T>`, generic in the element type
  (`ICredits<TCast> where TCast : ICastMember`). `IList<T>` is not covariant and EF cannot map
  an explicit interface implementation, so a non-generic `IList<IElement>` would force every
  consumer into copying shadow properties.
- Nullability comes from observed API responses, never from TMDB's OpenAPI spec (it declares
  nothing nullable). No prior + no observed null → nullable. See `audit/`.
- Endpoints are hand-written: build the URL, deserialize straight into the public type, ~4 lines.
  `Client/TmdbClient.Generated.cs` is **reference material, not compiled** — an index of TMDB's
  152 paths and parameter names to consult while writing an endpoint. Its DTOs cannot hold
  `append_to_response` blocks and declare almost nothing nullable, so routing through them would
  drop data and cost ~5,554 mapping assignments. See issue #9.
- Auth is a `DelegatingHandler` adding `Authorization: Bearer`, token resolved per request.
  Sessions are an explicit parameter on the 29 session-scoped ops, never client state.
- Endpoints grouped by TMDB area (Movies, TV, People, Search, ...)
- Async only, `CancellationToken` on every call
- One call = one request. No auto-pagination, no retry, no throttling: `AddTmdb` returns
  `IHttpClientBuilder` so the caller composes their own pipeline
  (`.AddStandardResilienceHandler()`). TMDB serves **pages 1–500 only** — past that it answers
  400/`status_code` 22 — while `total_pages` routinely claims far more (58,428 on
  `discover/movie`), so it is not a walkable bound. A 429 surfaces as `TmdbApiException`; filter
  on `HttpStatus`, there is no rate-limit subclass. See issue #16.