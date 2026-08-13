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
- Wire names come from `PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower` on the shared
  options, **not** from the attributes. `[JsonPropertyName]` does not travel through an interface
  to an implementing type, and STJ does not read it from interface declarations either — so a
  consumer's own entity satisfying a contract binds *nothing* without the policy, silently and
  with no exception. The attributes on the library's own types stay: they take precedence, and
  they document each wire name.
  **`SnakeCaseLower` does not break at a digit boundary** — `Iso3166_1` → `iso3166_1`, not
  `iso_3166_1` — so any property with a digit in its name needs an explicit attribute. Six do:
  `Iso3166_1`, `Iso639_1`, `InternalId` (`_id`), `WatchProviders` (`watch/providers`), and the
  deliberate renames `Value` (`certification`) and `Token` (`request_token`).
- Every response type implements a matching `I`-prefixed interface with **settable** members —
  a schema contract consuming apps implement on their own EF entities, so mapping is written
  once per interface rather than once per type. Interfaces never dictate keys or FKs.
- Nested collections on interfaces are `IList<T>`, generic in the element type
  (`ICredits<TCast> where TCast : ICastMember`). `IList<T>` is not covariant and EF cannot map
  an explicit interface implementation, so a non-generic `IList<IElement>` would force every
  consumer into copying shadow properties.
- Every entity-returning call has an **unconstrained generic twin** — `GetAsync<T>(…)` beside
  `GetAsync(…)` — so a consumer deserializes straight into its own EF entity instead of mapping
  off the library's type. Element-generic, not envelope-generic: `Task<PagedResult<T>>`, so the
  library keeps owning `page`/`total_results`. Unconstrained because `where T : ITvSeriesDetails`
  cannot be written (generic in 16 parameters; closing them pins the library's own types — the
  CS0311 bug of issue #18), and a non-generic marker to constrain to is leaky anyway:
  `GetAsync<ITvSeriesDetailsBase>` satisfies it and still throws at runtime, since STJ cannot
  instantiate an interface. No twin where a `T` buys nothing: the 14 `TmdbStatusResponse` writes,
  `IList<string>`, and `V4Lists.CreateAsync` (its body reads `created.Id`). 157 twins.
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
- One call = one request, with one exception: `V4Lists.CreateAsync` with `isPublic: false` follows
  the create with an update, because TMDB accepts `public: false`, answers `success`, and creates a
  public list regardless. Dropping a caller's privacy flag is a correctness gap, not thinness. See
  issue #17.
- No auto-pagination, no retry, no throttling: `AddTmdb` returns
  `IHttpClientBuilder` so the caller composes their own pipeline
  (`.AddStandardResilienceHandler()`). TMDB serves **pages 1–500 only** — past that it answers
  400/`status_code` 22 — while `total_pages` routinely claims far more (58,428 on
  `discover/movie`), so it is not a walkable bound. A 429 surfaces as `TmdbApiException`; filter
  on `HttpStatus`, there is no rate-limit subclass. See issue #16.