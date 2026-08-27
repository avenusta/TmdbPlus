using System.Text.Json.Serialization;
using TmdbPlus.Json;

namespace TmdbPlus.Models;

// Nullability from audit/nullability_decisions.json, entries "/3/tv/{series_id}" and below.

// ---------------------------------------------------------------------------
// Series
// ---------------------------------------------------------------------------

/// <summary>
/// The flat half of a series: the scalars TMDB always returns, no type parameters. A consumer
/// whose entity stores only series columns implements this and skips the sixteen parameters.
/// </summary>
public interface ITvSeriesDetailsBase
{
    int Id { get; set; }
    bool Adult { get; set; }
    bool InProduction { get; set; }
    int NumberOfEpisodes { get; set; }
    int NumberOfSeasons { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    bool Softcore { get; set; }

    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? OriginalLanguage { get; set; }
    string? Overview { get; set; }
    string? Tagline { get; set; }
    string? Homepage { get; set; }
    string? BackdropPath { get; set; }
    string? PosterPath { get; set; }
    string? Type { get; set; }
    string? Status { get; set; }
    DateOnly? FirstAirDate { get; set; }
    DateOnly? LastAirDate { get; set; }
    IList<int>? EpisodeRunTime { get; set; }
    IList<string>? Languages { get; set; }
    IList<string>? OriginCountry { get; set; }
}

/// <summary>
/// A series plus its nested collections and append blocks, each generic in its element or
/// envelope type. Null unless the call requested them.
/// </summary>
public interface ITvSeriesDetails<TGenres, TCreatedBy, TNetworks, TProductionCompanies, TSeasons,
    TLastEpisodeToAir, TNextEpisodeToAir, TExternalIds,
    TCredits, TAggregateCredits, TImages, TVideos, TKeywords,
    TContentRatings, TEpisodeGroups, TScreenedTheatrically> : ITvSeriesDetailsBase
    where TGenres : IGenre
    where TCreatedBy : ISeriesCreator
    where TNetworks : INetwork
    where TProductionCompanies : IProductionCompany
    where TSeasons : ISeasonSummary
    where TLastEpisodeToAir : IEpisodeSummary
    where TNextEpisodeToAir : IEpisodeSummary
    where TExternalIds : ITvExternalIds
    where TCredits : ICreditsBase
    where TAggregateCredits : IAggregateCreditsBase
    where TImages : IImagesBase
    where TVideos : IResultsOfBase
    where TKeywords : ITvKeywordsBase
    where TContentRatings : IResultsOfBase
    where TEpisodeGroups : IResultsOfBase
    where TScreenedTheatrically : IResultsOfBase
{
    IList<TGenres>? Genres { get; set; }
    IList<TCreatedBy>? CreatedBy { get; set; }
    IList<TNetworks>? Networks { get; set; }
    IList<TProductionCompanies>? ProductionCompanies { get; set; }
    IList<ProductionCountry>? ProductionCountries { get; set; }
    IList<SpokenLanguage>? SpokenLanguages { get; set; }
    IList<TSeasons>? Seasons { get; set; }
    TLastEpisodeToAir? LastEpisodeToAir { get; set; }
    TNextEpisodeToAir? NextEpisodeToAir { get; set; }
    TCredits? Credits { get; set; }
    TAggregateCredits? AggregateCredits { get; set; }
    TImages? Images { get; set; }
    TVideos? Videos { get; set; }
    TKeywords? Keywords { get; set; }
    TExternalIds? ExternalIds { get; set; }
    TvAlternativeTitles? AlternativeTitles { get; set; }
    TvTranslations? Translations { get; set; }
    TContentRatings? ContentRatings { get; set; }
    TEpisodeGroups? EpisodeGroups { get; set; }
    TScreenedTheatrically? ScreenedTheatrically { get; set; }
    ChangesResult? Changes { get; set; }
    PagedResult<TvSeriesSummary>? Recommendations { get; set; }
    PagedResult<TvSeriesSummary>? Similar { get; set; }
    PagedResult<Review>? Reviews { get; set; }
    PagedResult<ListSummary>? Lists { get; set; }
    ResultsMap<CountryWatchProviders>? WatchProviders { get; set; }
}

/// <summary>
/// A TV series, with one nullable property per append block. A block is <c>null</c> unless it
/// was requested (issue #3).
/// </summary>
public class TvSeriesDetails : ITvSeriesDetails<Genre, SeriesCreator, Network, ProductionCompany, SeasonSummary,
    EpisodeSummary, EpisodeSummary, TvExternalIds,
    Credits, AggregateCredits, Images, ResultsOf<Video>, TvKeywords,
    ResultsOf<ContentRating>, ResultsOf<EpisodeGroupSummary>, ResultsOf<ScreenedTheatrically>>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("in_production")] public bool InProduction { get; set; }
    [JsonPropertyName("number_of_episodes")] public int NumberOfEpisodes { get; set; }
    [JsonPropertyName("number_of_seasons")] public int NumberOfSeasons { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("softcore")] public bool Softcore { get; set; }

    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("tagline")] public string? Tagline { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }

    /// <summary>Free text ("Scripted", "Reality", ...), not a modelled vocabulary.</summary>
    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    [JsonPropertyName("last_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? LastAirDate { get; set; }

    [JsonPropertyName("episode_run_time")] public IList<int>? EpisodeRunTime { get; set; }
    [JsonPropertyName("languages")] public IList<string>? Languages { get; set; }
    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }
    [JsonPropertyName("genres")] public IList<Genre>? Genres { get; set; }
    [JsonPropertyName("created_by")] public IList<SeriesCreator>? CreatedBy { get; set; }
    [JsonPropertyName("networks")] public IList<Network>? Networks { get; set; }
    [JsonPropertyName("production_companies")] public IList<ProductionCompany>? ProductionCompanies { get; set; }
    [JsonPropertyName("production_countries")] public IList<ProductionCountry>? ProductionCountries { get; set; }
    [JsonPropertyName("spoken_languages")] public IList<SpokenLanguage>? SpokenLanguages { get; set; }
    [JsonPropertyName("seasons")] public IList<SeasonSummary>? Seasons { get; set; }
    [JsonPropertyName("last_episode_to_air")] public EpisodeSummary? LastEpisodeToAir { get; set; }
    [JsonPropertyName("next_episode_to_air")] public EpisodeSummary? NextEpisodeToAir { get; set; }

    // --- append blocks: null unless requested ---
    [JsonPropertyName("credits")] public Credits? Credits { get; set; }
    [JsonPropertyName("aggregate_credits")] public AggregateCredits? AggregateCredits { get; set; }
    [JsonPropertyName("images")] public Images? Images { get; set; }
    [JsonPropertyName("videos")] public ResultsOf<Video>? Videos { get; set; }
    [JsonPropertyName("keywords")] public TvKeywords? Keywords { get; set; }
    [JsonPropertyName("external_ids")] public TvExternalIds? ExternalIds { get; set; }
    [JsonPropertyName("alternative_titles")] public TvAlternativeTitles? AlternativeTitles { get; set; }
    [JsonPropertyName("translations")] public TvTranslations? Translations { get; set; }
    [JsonPropertyName("content_ratings")] public ResultsOf<ContentRating>? ContentRatings { get; set; }
    [JsonPropertyName("episode_groups")] public ResultsOf<EpisodeGroupSummary>? EpisodeGroups { get; set; }
    [JsonPropertyName("screened_theatrically")] public ResultsOf<ScreenedTheatrically>? ScreenedTheatrically { get; set; }
    [JsonPropertyName("changes")] public ChangesResult? Changes { get; set; }
    [JsonPropertyName("recommendations")] public PagedResult<TvSeriesSummary>? Recommendations { get; set; }
    [JsonPropertyName("similar")] public PagedResult<TvSeriesSummary>? Similar { get; set; }
    [JsonPropertyName("reviews")] public PagedResult<Review>? Reviews { get; set; }
    [JsonPropertyName("lists")] public PagedResult<ListSummary>? Lists { get; set; }
    [JsonPropertyName("watch/providers")] public ResultsMap<CountryWatchProviders>? WatchProviders { get; set; }
}

public interface ITvSeriesSummary
{
    int Id { get; set; }
    bool Adult { get; set; }
    string? Name { get; set; }
    string? OriginalName { get; set; }
    string? OriginalLanguage { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    string? BackdropPath { get; set; }
    DateOnly? FirstAirDate { get; set; }
    double Popularity { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    IList<int>? GenreIds { get; set; }
    IList<string>? OriginCountry { get; set; }
}

/// <summary>The trimmed series shape returned by list, search, and discovery endpoints.</summary>
public class TvSeriesSummary : ITvSeriesSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("original_language")] public string? OriginalLanguage { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("backdrop_path")] public string? BackdropPath { get; set; }

    [JsonPropertyName("first_air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? FirstAirDate { get; set; }

    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }
    [JsonPropertyName("genre_ids")] public IList<int>? GenreIds { get; set; }
    [JsonPropertyName("origin_country")] public IList<string>? OriginCountry { get; set; }
}

public class SeriesCreator : ISeriesCreator
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
}

public class Network : INetwork
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("logo_path")] public string? LogoPath { get; set; }
    [JsonPropertyName("origin_country")] public string? OriginCountry { get; set; }
    [JsonPropertyName("headquarters")] public string? Headquarters { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
}

// ---------------------------------------------------------------------------
// Season
// ---------------------------------------------------------------------------

public class SeasonSummary : ISeasonSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
    [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }

    [JsonPropertyName("air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? AirDate { get; set; }
}

/// <summary>
/// The flat half of a season: the scalars TMDB always returns, no type parameters.
/// </summary>
public interface ITvSeasonDetailsBase
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? PosterPath { get; set; }
    int SeasonNumber { get; set; }
    double VoteAverage { get; set; }
    DateOnly? AirDate { get; set; }
    string? InternalId { get; set; }
}

/// <summary>
/// A season plus its nested collections and append blocks, each generic in its element or
/// envelope type. Null unless the call requested them.
/// </summary>
public interface ITvSeasonDetails<TEpisodes, TNetworks, TExternalIds,
    TCredits, TAggregateCredits, TImages, TVideos> : ITvSeasonDetailsBase
    where TEpisodes : ITvEpisodeDetailsBase
    where TNetworks : INetwork
    where TExternalIds : ITvExternalIds
    where TCredits : ICreditsBase
    where TAggregateCredits : IAggregateCreditsBase
    where TImages : IImagesBase
    where TVideos : IResultsOfBase
{
    IList<TEpisodes>? Episodes { get; set; }
    IList<TNetworks>? Networks { get; set; }
    TCredits? Credits { get; set; }
    TAggregateCredits? AggregateCredits { get; set; }
    TImages? Images { get; set; }
    TVideos? Videos { get; set; }
    TExternalIds? ExternalIds { get; set; }
    TvTranslations? Translations { get; set; }
    ResultsMap<CountryWatchProviders>? WatchProviders { get; set; }
}

/// <summary>
/// A season. Note <c>changes</c> is NOT appendable here — TMDB rejects it, and the season-level
/// changes endpoint is keyed by season id rather than series/season number (issue #6).
/// </summary>
public class TvSeasonDetails : ITvSeasonDetails<TvEpisodeDetails, Network, TvExternalIds,
    Credits, AggregateCredits, Images, ResultsOf<Video>>
{
    /// <summary>TMDB returns this alongside <c>id</c>; it is the internal object id.</summary>
    [JsonPropertyName("_id")] public string? InternalId { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("poster_path")] public string? PosterPath { get; set; }
    [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }

    [JsonPropertyName("air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? AirDate { get; set; }

    [JsonPropertyName("episodes")] public IList<TvEpisodeDetails>? Episodes { get; set; }
    [JsonPropertyName("networks")] public IList<Network>? Networks { get; set; }

    // --- append blocks ---
    [JsonPropertyName("credits")] public Credits? Credits { get; set; }
    [JsonPropertyName("aggregate_credits")] public AggregateCredits? AggregateCredits { get; set; }
    [JsonPropertyName("images")] public Images? Images { get; set; }
    [JsonPropertyName("videos")] public ResultsOf<Video>? Videos { get; set; }
    [JsonPropertyName("external_ids")] public TvExternalIds? ExternalIds { get; set; }
    [JsonPropertyName("translations")] public TvTranslations? Translations { get; set; }
    [JsonPropertyName("watch/providers")] public ResultsMap<CountryWatchProviders>? WatchProviders { get; set; }
}

// ---------------------------------------------------------------------------
// Episode
// ---------------------------------------------------------------------------

public class EpisodeSummary : IEpisodeSummary
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("still_path")] public string? StillPath { get; set; }
    [JsonPropertyName("production_code")] public string? ProductionCode { get; set; }
    [JsonPropertyName("episode_number")] public int EpisodeNumber { get; set; }
    [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
    [JsonPropertyName("show_id")] public int ShowId { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }

    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

    [JsonPropertyName("air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? AirDate { get; set; }
}

public interface ITvEpisodeDetailsBase
{
    int Id { get; set; }
    string? Name { get; set; }
    string? Overview { get; set; }
    string? StillPath { get; set; }
    int EpisodeNumber { get; set; }
    int SeasonNumber { get; set; }
    int? Runtime { get; set; }
    double VoteAverage { get; set; }
    int VoteCount { get; set; }
    string? EpisodeType { get; set; }
    DateOnly? AirDate { get; set; }
    string? ProductionCode { get; set; }
    int ShowId { get; set; }
}

public interface ITvEpisodeDetails<TCrew, TGuestStars, TExternalIds, TCredits, TImages, TVideos>
    : ITvEpisodeDetailsBase
    where TCrew : ICrewMember
    where TGuestStars : ICastMember
    where TExternalIds : ITvExternalIds
    where TCredits : IEpisodeCreditsBase
    where TImages : IImagesBase
    where TVideos : IResultsOfBase
{
    IList<TCrew>? Crew { get; set; }
    IList<TGuestStars>? GuestStars { get; set; }
    TCredits? Credits { get; set; }
    TImages? Images { get; set; }
    TVideos? Videos { get; set; }
    TExternalIds? ExternalIds { get; set; }
    TvTranslations? Translations { get; set; }
}

/// <summary>An episode. <c>changes</c> is not appendable here either (issue #6).</summary>
public class TvEpisodeDetails : ITvEpisodeDetails<CrewMember, CastMember, TvExternalIds, EpisodeCredits, Images, ResultsOf<Video>>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("still_path")] public string? StillPath { get; set; }
    [JsonPropertyName("production_code")] public string? ProductionCode { get; set; }
    [JsonPropertyName("episode_number")] public int EpisodeNumber { get; set; }
    [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
    [JsonPropertyName("show_id")] public int ShowId { get; set; }
    [JsonPropertyName("runtime")] public int? Runtime { get; set; }
    [JsonPropertyName("vote_average")] public double VoteAverage { get; set; }
    [JsonPropertyName("vote_count")] public int VoteCount { get; set; }

    [JsonPropertyName("episode_type")]
    public string? EpisodeType { get; set; }

    [JsonPropertyName("air_date")]
    [JsonConverter(typeof(TmdbDateOnlyConverter))]
    public DateOnly? AirDate { get; set; }

    /// <summary>Present on the season details response, which embeds full episodes.</summary>
    [JsonPropertyName("crew")] public IList<CrewMember>? Crew { get; set; }

    [JsonPropertyName("guest_stars")] public IList<CastMember>? GuestStars { get; set; }

    // --- append blocks ---
    [JsonPropertyName("credits")] public EpisodeCredits? Credits { get; set; }
    [JsonPropertyName("images")] public Images? Images { get; set; }
    [JsonPropertyName("videos")] public ResultsOf<Video>? Videos { get; set; }
    [JsonPropertyName("external_ids")] public TvExternalIds? ExternalIds { get; set; }
    [JsonPropertyName("translations")] public TvTranslations? Translations { get; set; }
}

/// <summary>Episode credits carry guest stars alongside the usual cast and crew.</summary>
public class EpisodeCredits : Credits, IEpisodeCredits<CastMember>
{
    [JsonPropertyName("guest_stars")] public IList<CastMember>? GuestStars { get; set; }
}

// ---------------------------------------------------------------------------
// Aggregate credits — the series/season shape, where a person holds several roles
// ---------------------------------------------------------------------------

public class AggregateCredits : IAggregateCredits<AggregateCastMember, AggregateCrewMember>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("cast")] public IList<AggregateCastMember>? Cast { get; set; }
    [JsonPropertyName("crew")] public IList<AggregateCrewMember>? Crew { get; set; }
}

public class AggregateCastMember : IAggregateCastMember<AggregateRole>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("order")] public int? Order { get; set; }
    [JsonPropertyName("total_episode_count")] public int TotalEpisodeCount { get; set; }

    /// <summary>One entry per character the person played across the run.</summary>
    [JsonPropertyName("roles")] public IList<AggregateRole>? Roles { get; set; }
}

public class AggregateRole : IAggregateRole
{
    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("character")] public string? Character { get; set; }
    [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
}

public class AggregateCrewMember : IAggregateCrewMember<AggregateJob>
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("original_name")] public string? OriginalName { get; set; }
    [JsonPropertyName("known_for_department")] public string? KnownForDepartment { get; set; }
    [JsonPropertyName("profile_path")] public string? ProfilePath { get; set; }
    [JsonPropertyName("gender")] public int Gender { get; set; }
    [JsonPropertyName("adult")] public bool Adult { get; set; }
    [JsonPropertyName("popularity")] public double Popularity { get; set; }
    [JsonPropertyName("total_episode_count")] public int TotalEpisodeCount { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("jobs")] public IList<AggregateJob>? Jobs { get; set; }
}

public class AggregateJob : IAggregateJob
{
    [JsonPropertyName("credit_id")] public string? CreditId { get; set; }
    [JsonPropertyName("job")] public string? Job { get; set; }
    [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
}

// ---------------------------------------------------------------------------
// Block wrappers and leaf types specific to TV
// ---------------------------------------------------------------------------

public class TvKeywords : ITvKeywords<Keyword>
{
    [JsonPropertyName("id")] public int Id { get; set; }

    /// <summary>TV wraps under <c>results</c> where movies wrap under <c>keywords</c>.</summary>
    [JsonPropertyName("results")] public IList<Keyword>? Results { get; set; }
}

public class TvAlternativeTitles : ITvAlternativeTitles
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("results")] public IList<AlternativeTitle>? Results { get; set; }
}

public class TvTranslations : ITvTranslations
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("translations")] public IList<TvTranslation>? Translations { get; set; }
}

public class TvTranslation : ITvTranslation
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("iso_639_1")] public string? Iso639_1 { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("english_name")] public string? EnglishName { get; set; }
    [JsonPropertyName("data")] public TvTranslationData? Data { get; set; }
}

public class TvTranslationData : ITvTranslationData
{
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("overview")] public string? Overview { get; set; }
    [JsonPropertyName("tagline")] public string? Tagline { get; set; }
    [JsonPropertyName("homepage")] public string? Homepage { get; set; }
}

public class TvExternalIds : ITvExternalIds
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("imdb_id")] public string? ImdbId { get; set; }
    [JsonPropertyName("tvdb_id")] public int? TvdbId { get; set; }
    [JsonPropertyName("tvrage_id")] public int? TvrageId { get; set; }
    [JsonPropertyName("wikidata_id")] public string? WikidataId { get; set; }
    [JsonPropertyName("facebook_id")] public string? FacebookId { get; set; }
    [JsonPropertyName("instagram_id")] public string? InstagramId { get; set; }
    [JsonPropertyName("twitter_id")] public string? TwitterId { get; set; }
    [JsonPropertyName("freebase_mid")] public string? FreebaseMid { get; set; }
    [JsonPropertyName("freebase_id")] public string? FreebaseId { get; set; }
}

public class ContentRating : IContentRating
{
    [JsonPropertyName("iso_3166_1")] public string? Iso3166_1 { get; set; }
    [JsonPropertyName("rating")] public string? Rating { get; set; }
    [JsonPropertyName("descriptors")] public IList<string>? Descriptors { get; set; }
}

public class ScreenedTheatrically : IScreenedTheatrically
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("episode_number")] public int EpisodeNumber { get; set; }
    [JsonPropertyName("season_number")] public int SeasonNumber { get; set; }
}

public class EpisodeGroupSummary : IEpisodeGroupSummary<Network>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
    [JsonPropertyName("group_count")] public int GroupCount { get; set; }
    [JsonPropertyName("type")] public int? Type { get; set; }
    [JsonPropertyName("network")] public Network? Network { get; set; }
}

public class EpisodeGroupDetails : IEpisodeGroupDetails<Network, EpisodeGroup>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("episode_count")] public int EpisodeCount { get; set; }
    [JsonPropertyName("group_count")] public int GroupCount { get; set; }
    [JsonPropertyName("type")] public int? Type { get; set; }
    [JsonPropertyName("network")] public Network? Network { get; set; }
    [JsonPropertyName("groups")] public IList<EpisodeGroup>? Groups { get; set; }
}

public class EpisodeGroup : IEpisodeGroup<TvEpisodeDetails>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("order")] public int? Order { get; set; }
    [JsonPropertyName("locked")] public bool Locked { get; set; }
    [JsonPropertyName("episodes")] public IList<TvEpisodeDetails>? Episodes { get; set; }
}

/// <summary>The <c>airing_today</c> / <c>on_the_air</c> page shape.</summary>
public class TvSeriesPage : PagedResult<TvSeriesSummary>;
