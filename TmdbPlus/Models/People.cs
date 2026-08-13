using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// Nullability from audit/nullability_decisions.json, entries "/3/person/{person_id}" and below.

/// <summary>Blocks that can be appended to a person details request.</summary>
[Flags]
public enum PersonAppend
{
    None = 0,
    Changes = 1 << 0,
    CombinedCredits = 1 << 1,
    ExternalIds = 1 << 2,
    Images = 1 << 3,
    MovieCredits = 1 << 4,
    TaggedImages = 1 << 5,
    Translations = 1 << 6,
    TvCredits = 1 << 7,

    All = Changes | CombinedCredits | ExternalIds | Images | MovieCredits | TaggedImages
        | Translations | TvCredits,
}

internal static class PersonAppendExtensions
{
    internal static string ToQueryValue(this PersonAppend appends)
    {
        if (appends == PersonAppend.None) return string.Empty;

        var parts = new List<string>(8);
        if (appends.HasFlag(PersonAppend.Changes)) parts.Add("changes");
        if (appends.HasFlag(PersonAppend.CombinedCredits)) parts.Add("combined_credits");
        if (appends.HasFlag(PersonAppend.ExternalIds)) parts.Add("external_ids");
        if (appends.HasFlag(PersonAppend.Images)) parts.Add("images");
        if (appends.HasFlag(PersonAppend.MovieCredits)) parts.Add("movie_credits");
        if (appends.HasFlag(PersonAppend.TaggedImages)) parts.Add("tagged_images");
        if (appends.HasFlag(PersonAppend.Translations)) parts.Add("translations");
        if (appends.HasFlag(PersonAppend.TvCredits)) parts.Add("tv_credits");
        return string.Join(',', parts);
    }
}

public interface IPersonDetails<TExternalIds, TCombinedCredits, TMovieCredits, TTvCredits, TImages>
    where TExternalIds : IPersonExternalIds
    where TCombinedCredits : ICombinedCreditsBase
    where TMovieCredits : IPersonMovieCreditsBase
    where TTvCredits : IPersonTvCreditsBase
    where TImages : IPersonImagesBase
{
    int Id { get; set; }
    bool Adult { get; set; }
    int Gender { get; set; }
    double Popularity { get; set; }
    string? Name { get; set; }
    string? Biography { get; set; }
    string? KnownForDepartment { get; set; }
    string? PlaceOfBirth { get; set; }
    string? ProfilePath { get; set; }
    string? Homepage { get; set; }
    string? ImdbId { get; set; }
    DateOnly? Birthday { get; set; }
    DateOnly? Deathday { get; set; }
    IList<string>? AlsoKnownAs { get; set; }

    // Nested collections and append blocks: null unless the call requested them.
    TCombinedCredits? CombinedCredits { get; set; }
    TMovieCredits? MovieCredits { get; set; }
    TTvCredits? TvCredits { get; set; }
    TExternalIds? ExternalIds { get; set; }
    TImages? Images { get; set; }
    PagedResult<TaggedImage>? TaggedImages { get; set; }
    PersonTranslations? Translations { get; set; }
    ChangesResult? Changes { get; set; }
}

public class PersonDetails : IPersonDetails<PersonExternalIds, CombinedCredits, PersonMovieCredits, PersonTvCredits, PersonImages>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("biography")] public string? Biography { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("place_of_birth")] public string? PlaceOfBirth { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
    [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; }

    [JsonPropertyName("birthday")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? Birthday { get; set; }

    [JsonPropertyName("deathday")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? Deathday { get; set; }

    [JsonPropertyName("also_known_as")] public IList<string>? AlsoKnownAs { get; set; }

    // --- append blocks: null unless requested ---
    [JsonPropertyName("combined_credits")] public CombinedCredits? CombinedCredits { get; set; }
    [JsonPropertyName("movie_credits")] public PersonMovieCredits? MovieCredits { get; set; }
    [JsonPropertyName("tv_credits")] public PersonTvCredits? TvCredits { get; set; }
    [JsonPropertyName("external_ids")] public PersonExternalIds? ExternalIds { get; set; }
    [JsonPropertyName("images")] public PersonImages? Images { get; set; }
    [JsonPropertyName("tagged_images")] public PagedResult<TaggedImage>? TaggedImages { get; set; }
    [JsonPropertyName("translations")] public PersonTranslations? Translations { get; set; }
    [JsonPropertyName("changes")] public ChangesResult? Changes { get; set; }
}

public class PersonSummary : IPersonSummary<CombinedCastCredit>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }

    /// <summary>A mixed list of movies and series — check each entry's <c>MediaType</c>.</summary>
    [JsonPropertyName("known_for")] public IList<CombinedCastCredit>? KnownFor { get; set; }
}

// ---------------------------------------------------------------------------
// Credits. Combined credits mix movies and series in one list, discriminated by
// media_type -- the one place MediaType is load-bearing rather than decorative.
// ---------------------------------------------------------------------------

public class CombinedCredits : ICombinedCredits<CombinedCastCredit, CombinedCrewCredit>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("cast")] public IList<CombinedCastCredit>? Cast { get; set; }
    [JsonPropertyName("crew")] public IList<CombinedCrewCredit>? Crew { get; set; }
}

/// <summary>
/// One acting credit. Movie fields (<c>title</c>, <c>release_date</c>) and TV fields
/// (<c>name</c>, <c>first_air_date</c>, <c>episode_count</c>) are both present on the type;
/// which are populated depends on <see cref="MediaType"/>.
/// </summary>
public class CombinedCastCredit : ICombinedCastCredit
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("softcore")] public bool Softcore { get; set; }
    [JsonPropertyName("video")] public bool Video { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("character")] public string? Character { get; set; }
    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("order")] public int? Order { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("genre_ids")] public IList<int>? GenreIds { get; set; }

    // Movie-shaped fields.
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    // TV-shaped fields.
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("episode_count")] public int? EpisodeCount { get; set; }
    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    /// <summary>The title or name, whichever this credit carries.</summary>
    [JsonIgnore] public string? DisplayName => Title ?? Name;
}

/// <inheritdoc cref="CombinedCastCredit"/>
public class CombinedCrewCredit : ICombinedCrewCredit
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("softcore")] public bool Softcore { get; set; }
    [JsonPropertyName("video")] public bool Video { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("job")] public string? Job { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("genre_ids")] public IList<int>? GenreIds { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("original_title")] public string? OriginalTitle { get; set; }

    [JsonPropertyName("release_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? ReleaseDate { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("episode_count")] public int? EpisodeCount { get; set; }
    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    [JsonIgnore] public string? DisplayName => Title ?? Name;
}

/// <summary>Movie-only credits: the same entries, without the TV fields or a media type.</summary>
public class PersonMovieCredits : IPersonMovieCredits<CombinedCastCredit, CombinedCrewCredit>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("cast")] public IList<CombinedCastCredit>? Cast { get; set; }
    [JsonPropertyName("crew")] public IList<CombinedCrewCredit>? Crew { get; set; }
}

/// <summary>TV-only credits.</summary>
public class PersonTvCredits : IPersonTvCredits<CombinedCastCredit, CombinedCrewCredit>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("cast")] public IList<CombinedCastCredit>? Cast { get; set; }
    [JsonPropertyName("crew")] public IList<CombinedCrewCredit>? Crew { get; set; }
}

// ---------------------------------------------------------------------------
// Person-specific blocks
// ---------------------------------------------------------------------------

public class PersonImages : IPersonImages<ImageInfo>
{
    [JsonPropertyName("id")] public int Id { get; set; }

    /// <summary>People have profiles where titles have posters and backdrops.</summary>
    [JsonPropertyName("profiles")] public IList<ImageInfo>? Profiles { get; set; }
}

/// <summary>An image the person is tagged in, carrying the media it belongs to.</summary>
public class TaggedImage : ITaggedImage<CombinedCastCredit>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("file_path")] public string? FilePath { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("aspect_ratio")] public double AspectRatio { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("image_type")] public string? ImageType { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    /// <summary>The movie or series this still comes from, in its summary shape.</summary>
    [JsonPropertyName("media")] public CombinedCastCredit? Media { get; set; }
}

public class PersonExternalIds : IPersonExternalIds
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; }
    [JsonPropertyName("wikidata_id")] public string? WikidataId { get; set; }
    [JsonPropertyName("facebook_id")] public string? FacebookId { get; set; }
    [JsonPropertyName("instagram_id")] public string? InstagramId { get; set; }
    [JsonPropertyName("twitter_id")] public string? TwitterId { get; set; }
    [JsonPropertyName("tiktok_id")] public string? TiktokId { get; set; }
    [JsonPropertyName("youtube_id")] public string? YoutubeId { get; set; }
    [JsonPropertyName("freebase_id")] public string? FreebaseId { get; set; }
    [JsonPropertyName("freebase_mid")] public string? FreebaseMid { get; set; }
    [JsonPropertyName("tvrage_id")] public int? TvrageId { get; set; }
}

public class PersonTranslations : IPersonTranslations
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("translations")] public IList<PersonTranslation>? Translations { get; set; }
}

public class PersonTranslation : IPersonTranslation
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("data")] public PersonTranslationData? Data { get; set; }
}

public class PersonTranslationData : IPersonTranslationData
{
    [JsonPropertyName("biography")] public string? Biography { get; set; }
}
