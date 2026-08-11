using TmdbPlus.Models;

namespace TmdbPlus;

// Discover takes 38 parameters for movies and 33 for TV. As optional method arguments that is an
// unusable signature, so each is an options object with settable properties: the caller sets what
// it cares about and the rest stay absent. TMDB's dotted parameter names (`vote_average.gte`) are
// spelled out in ToQuery rather than encoded in property names.

/// <summary>Sort orders accepted by <c>/discover/movie</c>.</summary>
public enum MovieSortBy
{
    PopularityDesc = 0,
    PopularityAsc,
    RevenueDesc,
    RevenueAsc,
    PrimaryReleaseDateDesc,
    PrimaryReleaseDateAsc,
    VoteAverageDesc,
    VoteAverageAsc,
    VoteCountDesc,
    VoteCountAsc,
    TitleAsc,
    TitleDesc,
}

/// <summary>Sort orders accepted by <c>/discover/tv</c>.</summary>
public enum TvSortBy
{
    PopularityDesc = 0,
    PopularityAsc,
    FirstAirDateDesc,
    FirstAirDateAsc,
    VoteAverageDesc,
    VoteAverageAsc,
    VoteCountDesc,
    VoteCountAsc,
    NameAsc,
    NameDesc,
}

internal static class SortByExtensions
{
    internal static string ToWire(this MovieSortBy s) => s switch
    {
        MovieSortBy.PopularityDesc => "popularity.desc",
        MovieSortBy.PopularityAsc => "popularity.asc",
        MovieSortBy.RevenueDesc => "revenue.desc",
        MovieSortBy.RevenueAsc => "revenue.asc",
        MovieSortBy.PrimaryReleaseDateDesc => "primary_release_date.desc",
        MovieSortBy.PrimaryReleaseDateAsc => "primary_release_date.asc",
        MovieSortBy.VoteAverageDesc => "vote_average.desc",
        MovieSortBy.VoteAverageAsc => "vote_average.asc",
        MovieSortBy.VoteCountDesc => "vote_count.desc",
        MovieSortBy.VoteCountAsc => "vote_count.asc",
        MovieSortBy.TitleAsc => "title.asc",
        MovieSortBy.TitleDesc => "title.desc",
        _ => "popularity.desc",
    };

    internal static string ToWire(this TvSortBy s) => s switch
    {
        TvSortBy.PopularityDesc => "popularity.desc",
        TvSortBy.PopularityAsc => "popularity.asc",
        TvSortBy.FirstAirDateDesc => "first_air_date.desc",
        TvSortBy.FirstAirDateAsc => "first_air_date.asc",
        TvSortBy.VoteAverageDesc => "vote_average.desc",
        TvSortBy.VoteAverageAsc => "vote_average.asc",
        TvSortBy.VoteCountDesc => "vote_count.desc",
        TvSortBy.VoteCountAsc => "vote_count.asc",
        TvSortBy.NameAsc => "name.asc",
        TvSortBy.NameDesc => "name.desc",
        _ => "popularity.desc",
    };
}

/// <summary>
/// Filters for <c>/discover/movie</c>. Everything is optional; unset properties are omitted from
/// the query. The <c>with_*</c> string filters accept TMDB's own <c>,</c> (AND) and <c>|</c> (OR)
/// syntax, which is passed through untouched.
/// </summary>
public sealed class DiscoverMovieOptions
{
    public MovieSortBy SortBy { get; set; } = MovieSortBy.PopularityDesc;
    public int? Page { get; set; }
    public string? Language { get; set; }
    public string? Region { get; set; }
    public bool? IncludeAdult { get; set; }
    public bool? IncludeVideo { get; set; }

    public int? Year { get; set; }
    public int? PrimaryReleaseYear { get; set; }
    public DateOnly? PrimaryReleaseDateFrom { get; set; }
    public DateOnly? PrimaryReleaseDateTo { get; set; }
    public DateOnly? ReleaseDateFrom { get; set; }
    public DateOnly? ReleaseDateTo { get; set; }

    public double? VoteAverageFrom { get; set; }
    public double? VoteAverageTo { get; set; }
    public double? VoteCountFrom { get; set; }
    public double? VoteCountTo { get; set; }
    public int? RuntimeFrom { get; set; }
    public int? RuntimeTo { get; set; }

    public string? Certification { get; set; }
    public string? CertificationFrom { get; set; }
    public string? CertificationTo { get; set; }
    public string? CertificationCountry { get; set; }

    public string? WithGenres { get; set; }
    public string? WithoutGenres { get; set; }
    public string? WithKeywords { get; set; }
    public string? WithoutKeywords { get; set; }
    public string? WithCompanies { get; set; }
    public string? WithoutCompanies { get; set; }
    public string? WithPeople { get; set; }
    public string? WithCast { get; set; }
    public string? WithCrew { get; set; }
    public string? WithOriginCountry { get; set; }
    public string? WithOriginalLanguage { get; set; }
    public int? WithReleaseType { get; set; }

    public string? WatchRegion { get; set; }
    public string? WithWatchProviders { get; set; }
    public string? WithoutWatchProviders { get; set; }
    public string? WithWatchMonetizationTypes { get; set; }

    internal QueryString ToQuery(string? defaultLanguage, string? defaultRegion) => new QueryString()
        .Add("sort_by", SortBy.ToWire())
        .Add("page", Page)
        .Add("language", Language ?? defaultLanguage)
        .Add("region", Region ?? defaultRegion)
        .Add("include_adult", IncludeAdult)
        .Add("include_video", IncludeVideo)
        .Add("year", Year)
        .Add("primary_release_year", PrimaryReleaseYear)
        .Add("primary_release_date.gte", PrimaryReleaseDateFrom)
        .Add("primary_release_date.lte", PrimaryReleaseDateTo)
        .Add("release_date.gte", ReleaseDateFrom)
        .Add("release_date.lte", ReleaseDateTo)
        .Add("vote_average.gte", VoteAverageFrom)
        .Add("vote_average.lte", VoteAverageTo)
        .Add("vote_count.gte", VoteCountFrom)
        .Add("vote_count.lte", VoteCountTo)
        .Add("with_runtime.gte", RuntimeFrom)
        .Add("with_runtime.lte", RuntimeTo)
        .Add("certification", Certification)
        .Add("certification.gte", CertificationFrom)
        .Add("certification.lte", CertificationTo)
        .Add("certification_country", CertificationCountry)
        .Add("with_genres", WithGenres)
        .Add("without_genres", WithoutGenres)
        .Add("with_keywords", WithKeywords)
        .Add("without_keywords", WithoutKeywords)
        .Add("with_companies", WithCompanies)
        .Add("without_companies", WithoutCompanies)
        .Add("with_people", WithPeople)
        .Add("with_cast", WithCast)
        .Add("with_crew", WithCrew)
        .Add("with_origin_country", WithOriginCountry)
        .Add("with_original_language", WithOriginalLanguage)
        .Add("with_release_type", WithReleaseType)
        .Add("watch_region", WatchRegion)
        .Add("with_watch_providers", WithWatchProviders)
        .Add("without_watch_providers", WithoutWatchProviders)
        .Add("with_watch_monetization_types", WithWatchMonetizationTypes);
}

/// <inheritdoc cref="DiscoverMovieOptions"/>
public sealed class DiscoverTvOptions
{
    public TvSortBy SortBy { get; set; } = TvSortBy.PopularityDesc;
    public int? Page { get; set; }
    public string? Language { get; set; }
    public string? Timezone { get; set; }
    public bool? IncludeAdult { get; set; }
    public bool? IncludeNullFirstAirDates { get; set; }
    public bool? ScreenedTheatrically { get; set; }

    public int? FirstAirDateYear { get; set; }
    public DateOnly? FirstAirDateFrom { get; set; }
    public DateOnly? FirstAirDateTo { get; set; }
    public DateOnly? AirDateFrom { get; set; }
    public DateOnly? AirDateTo { get; set; }

    public double? VoteAverageFrom { get; set; }
    public double? VoteAverageTo { get; set; }
    public double? VoteCountFrom { get; set; }
    public double? VoteCountTo { get; set; }
    public int? RuntimeFrom { get; set; }
    public int? RuntimeTo { get; set; }

    public string? WithGenres { get; set; }
    public string? WithoutGenres { get; set; }
    public string? WithKeywords { get; set; }
    public string? WithoutKeywords { get; set; }
    public string? WithCompanies { get; set; }
    public string? WithoutCompanies { get; set; }
    public int? WithNetworks { get; set; }
    public string? WithOriginCountry { get; set; }
    public string? WithOriginalLanguage { get; set; }

    /// <summary>TMDB's numeric status filter, not the <c>MediaStatus</c> vocabulary.</summary>
    public string? WithStatus { get; set; }

    public string? WithType { get; set; }

    public string? WatchRegion { get; set; }
    public string? WithWatchProviders { get; set; }
    public string? WithoutWatchProviders { get; set; }
    public string? WithWatchMonetizationTypes { get; set; }

    internal QueryString ToQuery(string? defaultLanguage) => new QueryString()
        .Add("sort_by", SortBy.ToWire())
        .Add("page", Page)
        .Add("language", Language ?? defaultLanguage)
        .Add("timezone", Timezone)
        .Add("include_adult", IncludeAdult)
        .Add("include_null_first_air_dates", IncludeNullFirstAirDates)
        .Add("screened_theatrically", ScreenedTheatrically)
        .Add("first_air_date_year", FirstAirDateYear)
        .Add("first_air_date.gte", FirstAirDateFrom)
        .Add("first_air_date.lte", FirstAirDateTo)
        .Add("air_date.gte", AirDateFrom)
        .Add("air_date.lte", AirDateTo)
        .Add("vote_average.gte", VoteAverageFrom)
        .Add("vote_average.lte", VoteAverageTo)
        .Add("vote_count.gte", VoteCountFrom)
        .Add("vote_count.lte", VoteCountTo)
        .Add("with_runtime.gte", RuntimeFrom)
        .Add("with_runtime.lte", RuntimeTo)
        .Add("with_genres", WithGenres)
        .Add("without_genres", WithoutGenres)
        .Add("with_keywords", WithKeywords)
        .Add("without_keywords", WithoutKeywords)
        .Add("with_companies", WithCompanies)
        .Add("without_companies", WithoutCompanies)
        .Add("with_networks", WithNetworks)
        .Add("with_origin_country", WithOriginCountry)
        .Add("with_original_language", WithOriginalLanguage)
        .Add("with_status", WithStatus)
        .Add("with_type", WithType)
        .Add("watch_region", WatchRegion)
        .Add("with_watch_providers", WithWatchProviders)
        .Add("without_watch_providers", WithoutWatchProviders)
        .Add("with_watch_monetization_types", WithWatchMonetizationTypes);
}
