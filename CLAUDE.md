# TmdbPlus

Strongly typed .NET wrapper for the TMDB API (v3 + v4).

## Goal

Create a client with an API key, call endpoints, get typed results back. Records/interfaces for every request and response — no `dynamic`, no raw JSON handed to callers.

## Stack

- net10.0, nullable enabled, implicit usings
- `System.Text.Json` + `HttpClient` (no third-party deps)

## Conventions

- Responses are `record`s with `[JsonPropertyName]` for TMDB's snake_case
- Endpoints grouped by TMDB area (Movies, TV, People, Search, ...)
- Async only, `CancellationToken` on every call