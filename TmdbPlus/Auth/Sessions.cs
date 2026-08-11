namespace TmdbPlus.Auth;

// Sessions are an explicit parameter on the 29 session-scoped ops, never client state, so the
// client stays immutable and singleton-safe (issue #5). Distinct types because the two are not
// interchangeable: 16 ops take a user session, 3 take a guest session, 10 accept either.

/// <summary>A user session id, from the v3 authentication flow.</summary>
public readonly record struct UserSession(string SessionId)
{
    public override string ToString() => SessionId;
}

/// <summary>A guest session id. Valid for rating writes only.</summary>
public readonly record struct GuestSession(string GuestSessionId)
{
    public override string ToString() => GuestSessionId;
}

/// <summary>A session for the 10 ops that accept either kind.</summary>
public readonly struct AnySession
{
    AnySession(string? user, string? guest) { UserSessionId = user; GuestSessionId = guest; }

    public string? UserSessionId { get; }
    public string? GuestSessionId { get; }

    public static implicit operator AnySession(UserSession s) => new(s.SessionId, null);
    public static implicit operator AnySession(GuestSession s) => new(null, s.GuestSessionId);
}

internal static class SessionExtensions
{
    /// <summary>Whichever of the two session parameters this session carries.</summary>
    internal static QueryString ToQuery(this AnySession session) => new QueryString()
        .Add("session_id", session.UserSessionId)
        .Add("guest_session_id", session.GuestSessionId);
}
