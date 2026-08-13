namespace TmdbPlus.Models;

// TMDB's vocabularies, as lookup constants rather than enum types.
//
// Response properties hold the raw wire value (string, or int for release type), so a consuming
// EF entity maps a plain column with no value converter and no owned type. These constants exist
// for comparison and switching:
//
//     if (crew.Department == CreditDepartment.VisualEffects) ...
//     switch (video.Type) { case VideoType.Trailer: ... }
//
// `const` members are usable as switch labels, which a static readonly field would not be.
//
// Supersedes the TmdbEnum<T> wrapper from issue #10. That ticket's vocabulary analysis stands --
// `job` is still NOT listed here (94 distinct values in a single film), and an unrecognised value
// is still never an error, it is simply a string matching none of these constants.
//
// MediaType remains a real enum: it is a request parameter (SetFavoriteAsync, SetWatchlistAsync,
// GetItemStatusAsync, V4ListItem), where accepting arbitrary text would put junk in a URL.

/// <summary>Five values, not three -- seasons and episodes appear in change lists.</summary>
public enum MediaType
{
    Unknown = 0,
    Movie,
    Tv,
    Person,
    TvSeason,
    TvEpisode,
}

/// <summary>Wire strings for <see cref="MediaType"/>, as stored on response properties.</summary>
public static class MediaTypes
{
    public const string Movie = "movie";
    public const string Tv = "tv";
    public const string Person = "person";
    public const string TvSeason = "tv_season";
    public const string TvEpisode = "tv_episode";

    /// <summary>
    /// Turns a stored wire string back into the enum, for feeding a response value into a request
    /// parameter. An unrecognised value yields <see cref="MediaType.Unknown"/>.
    /// </summary>
    public static MediaType Parse(string? wire) => wire switch
    {
        Movie => MediaType.Movie,
        Tv => MediaType.Tv,
        Person => MediaType.Person,
        TvSeason => MediaType.TvSeason,
        TvEpisode => MediaType.TvEpisode,
        _ => MediaType.Unknown,
    };
}

/// <summary>Numeric on the wire, unlike the rest -- stored as <c>int</c>.</summary>
public static class ReleaseType
{
    public const int Premiere = 1;
    public const int TheatricalLimited = 2;
    public const int Theatrical = 3;
    public const int Digital = 4;
    public const int Physical = 5;
    public const int Tv = 6;
}

public static class MediaStatus
{
    public const string Rumored = "Rumored";
    public const string Planned = "Planned";
    public const string InProduction = "In Production";
    public const string PostProduction = "Post Production";
    public const string Released = "Released";
    public const string Canceled = "Canceled";
    public const string ReturningSeries = "Returning Series";
    public const string Ended = "Ended";
    public const string Pilot = "Pilot";
}

public static class EpisodeType
{
    public const string Standard = "standard";
    public const string Finale = "finale";
    public const string MidSeason = "mid_season";
}

public static class ChangeAction
{
    public const string Added = "added";
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Deleted = "deleted";
    public const string Destroyed = "destroyed";
}

public static class VideoType
{
    public const string Trailer = "Trailer";
    public const string Teaser = "Teaser";
    public const string Clip = "Clip";
    public const string Featurette = "Featurette";
    public const string BehindTheScenes = "Behind the Scenes";
    public const string Bloopers = "Bloopers";
    public const string OpeningCredits = "Opening Credits";
    public const string Recap = "Recap";
}

public static class CreditDepartment
{
    public const string Acting = "Acting";
    public const string Art = "Art";
    public const string Camera = "Camera";
    public const string CostumeAndMakeUp = "Costume & Make-Up";
    public const string Crew = "Crew";
    public const string Directing = "Directing";
    public const string Editing = "Editing";
    public const string Lighting = "Lighting";
    public const string Production = "Production";
    public const string Sound = "Sound";
    public const string VisualEffects = "Visual Effects";
    public const string Writing = "Writing";
    public const string Actors = "Actors";
}

public static class VideoSite
{
    public const string YouTube = "YouTube";
    public const string Vimeo = "Vimeo";
}
