using System.Text.Json.Serialization;

namespace TmdbPlus.Models;

// The 8 closed vocabularies from issue #10. Every one carries Unknown = 0: an unrecognised
// value degrades, never throws, and the raw text is kept via TmdbEnum<T> where it matters.
// `job` is deliberately NOT an enum -- 94 distinct values appeared in a single film.

/// <summary>Five values, not three -- seasons and episodes appear in change lists.</summary>
public enum MediaType
{
    Unknown = 0,
    [JsonStringEnumMemberName("movie")] Movie,
    [JsonStringEnumMemberName("tv")] Tv,
    [JsonStringEnumMemberName("person")] Person,
    [JsonStringEnumMemberName("tv_season")] TvSeason,
    [JsonStringEnumMemberName("tv_episode")] TvEpisode,
}

/// <summary>Numeric on the wire, unlike the rest.</summary>
public enum ReleaseType
{
    Unknown = 0,
    Premiere = 1,
    TheatricalLimited = 2,
    Theatrical = 3,
    Digital = 4,
    Physical = 5,
    Tv = 6,
}

public enum MediaStatus
{
    Unknown = 0,
    [JsonStringEnumMemberName("Rumored")] Rumored,
    [JsonStringEnumMemberName("Planned")] Planned,
    [JsonStringEnumMemberName("In Production")] InProduction,
    [JsonStringEnumMemberName("Post Production")] PostProduction,
    [JsonStringEnumMemberName("Released")] Released,
    [JsonStringEnumMemberName("Canceled")] Canceled,
    [JsonStringEnumMemberName("Returning Series")] ReturningSeries,
    [JsonStringEnumMemberName("Ended")] Ended,
    [JsonStringEnumMemberName("Pilot")] Pilot,
}

public enum EpisodeType
{
    Unknown = 0,
    [JsonStringEnumMemberName("standard")] Standard,
    [JsonStringEnumMemberName("finale")] Finale,
    [JsonStringEnumMemberName("mid_season")] MidSeason,
}

public enum ChangeAction
{
    Unknown = 0,
    [JsonStringEnumMemberName("added")] Added,
    [JsonStringEnumMemberName("created")] Created,
    [JsonStringEnumMemberName("updated")] Updated,
    [JsonStringEnumMemberName("deleted")] Deleted,
    [JsonStringEnumMemberName("destroyed")] Destroyed,
}

public enum VideoType
{
    Unknown = 0,
    [JsonStringEnumMemberName("Trailer")] Trailer,
    [JsonStringEnumMemberName("Teaser")] Teaser,
    [JsonStringEnumMemberName("Clip")] Clip,
    [JsonStringEnumMemberName("Featurette")] Featurette,
    [JsonStringEnumMemberName("Behind the Scenes")] BehindTheScenes,
    [JsonStringEnumMemberName("Bloopers")] Bloopers,
    [JsonStringEnumMemberName("Opening Credits")] OpeningCredits,
    [JsonStringEnumMemberName("Recap")] Recap,
}

public enum CreditDepartment
{
    Unknown = 0,
    [JsonStringEnumMemberName("Acting")] Acting,
    [JsonStringEnumMemberName("Art")] Art,
    [JsonStringEnumMemberName("Camera")] Camera,
    [JsonStringEnumMemberName("Costume & Make-Up")] CostumeAndMakeUp,
    [JsonStringEnumMemberName("Crew")] Crew,
    [JsonStringEnumMemberName("Directing")] Directing,
    [JsonStringEnumMemberName("Editing")] Editing,
    [JsonStringEnumMemberName("Lighting")] Lighting,
    [JsonStringEnumMemberName("Production")] Production,
    [JsonStringEnumMemberName("Sound")] Sound,
    [JsonStringEnumMemberName("Visual Effects")] VisualEffects,
    [JsonStringEnumMemberName("Writing")] Writing,
    [JsonStringEnumMemberName("Actors")] Actors,
}

public enum VideoSite
{
    Unknown = 0,
    [JsonStringEnumMemberName("YouTube")] YouTube,
    [JsonStringEnumMemberName("Vimeo")] Vimeo,
}
