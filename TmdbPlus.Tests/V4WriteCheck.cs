using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using TmdbPlus;
using TmdbPlus.Models;

// Exercises the v4 list write path -- the one surface never run against the live API (issue #13).
//
// It cannot run unattended: v4 user-scoped writes need an access token minted from a request token
// the *user* approves in a browser. So this pauses once, for that approval, then drives every
// mutating call and prints the observed shapes.
//
// Run:  dotnet run --project TmdbPlus.Tests -- v4writes
//
// Everything it creates, it deletes -- except when a write fails midway, in which case the list id
// is printed so it can be cleaned up by hand.

static class V4WriteCheck
{
    public static async Task RunAsync(Action<bool, string> check)
    {
        var token = Environment.GetEnvironmentVariable("TMDB_READ_ACCESS_TOKEN") ?? ReadDotEnv();
        if (token is null)
        {
            Console.WriteLine("v4 write check SKIPPED (no TMDB_READ_ACCESS_TOKEN).");
            return;
        }

        var client = new ServiceCollection()
            .AddTmdb(o => o.ReadAccessToken = token)
            .Services.BuildServiceProvider()
            .GetRequiredService<ITmdbClient>();

        // --- The one manual step -------------------------------------------------------------
        var requestToken = await client.V4Authentication.CreateRequestTokenAsync();
        check(!string.IsNullOrEmpty(requestToken.RequestToken), "a request token should be issued");

        Console.WriteLine("\n  Approve this request token in a browser, then press Enter:\n");
        Console.WriteLine($"    https://www.themoviedb.org/auth/access?request_token={requestToken.RequestToken}\n");
        Console.ReadLine();

        var access = await client.V4Authentication.CreateAccessTokenAsync(requestToken.RequestToken!);
        check(!string.IsNullOrEmpty(access.AccessToken), "approved token should exchange for an access token");
        check(!string.IsNullOrEmpty(access.AccountId), "access token response should carry account_id");
        Console.WriteLine($"  access:    account {access.AccountId}");

        var userToken = access.AccessToken!;
        int? listId = null;

        try
        {
            // --- create ----------------------------------------------------------------------
            var created = await client.V4Lists.CreateAsync(userToken,
                name: "TmdbPlus v4 write check",
                description: "Temporary; created by the issue #13 verification run.",
                isPublic: false);

            check(created.Success == true, "create should report success");
            check(created.Id is > 0, "create should return the new list id");
            listId = created.Id;
            var id = created.Id;
            Console.WriteLine($"  create:    list {id} (success={created.Success}, status={created.StatusCode})");

            // --- add items -------------------------------------------------------------------
            // Two movies and a series, so the mixed-media claim is actually exercised.
            V4ListItem[] items =
            [
                new(MediaType.Movie, 603),
                new(MediaType.Movie, 550),
                new(MediaType.Tv, 1396),
            ];

            var added = await client.V4Lists.AddItemsAsync(id, userToken, items);
            check(added.Success == true, "add should report success");
            Console.WriteLine($"  add:       success={added.Success}, status={added.StatusCode}, " +
                              $"results={added.Results?.Count.ToString() ?? "null"}");
            Dump("add results", added.Results);

            // --- read back, confirming the write landed --------------------------------------
            var afterAdd = await client.V4Lists.GetAsync(id, accessToken: userToken);
            check(afterAdd.Results is { Count: 3 }, "all three items should be on the list");
            Console.WriteLine($"  read:      {afterAdd.Results?.Count} items, " +
                              $"name=\"{afterAdd.Name}\", public={afterAdd.Public}");

            // --- item_status on a present item (the 404 case is already covered in LiveCheck) --
            var status = await client.V4Lists.GetItemStatusAsync(id, MediaType.Movie, 603);
            check(status.Success == true, "a present item should report success");
            Console.WriteLine($"  status:    present item -> success={status.Success}");

            // --- duplicate add: is it idempotent, as v3's rating DELETE turned out to be? ------
            var duplicate = await client.V4Lists.AddItemsAsync(id, userToken,
                [new(MediaType.Movie, 603)]);
            Console.WriteLine($"  dup add:   success={duplicate.Success}, status={duplicate.StatusCode}");
            Dump("duplicate results", duplicate.Results);

            // --- update the per-item comment --------------------------------------------------
            var commented = await client.V4Lists.UpdateItemsAsync(id, userToken,
                [new(MediaType.Movie, 603, "set by the write check")]);
            check(commented.Success == true, "comment update should report success");
            Console.WriteLine($"  comment:   success={commented.Success}, status={commented.StatusCode}");

            // --- update the list itself -------------------------------------------------------
            var updated = await client.V4Lists.UpdateAsync(id, userToken,
                new V4UpdateListRequest { Name = "TmdbPlus v4 write check (renamed)", Public = false });
            check(updated.Success == true, "list update should report success");
            Console.WriteLine($"  update:    success={updated.Success}, status={updated.StatusCode}");

            // --- remove an item ---------------------------------------------------------------
            var removed = await client.V4Lists.RemoveItemsAsync(id, userToken,
                [new(MediaType.Movie, 550)]);
            check(removed.Success == true, "remove should report success");
            Console.WriteLine($"  remove:    success={removed.Success}, status={removed.StatusCode}");

            // --- remove an item that is not there: the absent-item failure shape ---------------
            try
            {
                var ghost = await client.V4Lists.RemoveItemsAsync(id, userToken,
                    [new(MediaType.Movie, 999999)]);
                Console.WriteLine($"  ghost rm:  success={ghost.Success}, status={ghost.StatusCode} (no throw)");
                Dump("ghost results", ghost.Results);
            }
            catch (TmdbApiException ex)
            {
                Console.WriteLine($"  ghost rm:  threw {(int)ex.HttpStatus} status_code={ex.StatusCode}");
            }

            // --- clear ------------------------------------------------------------------------
            var cleared = await client.V4Lists.ClearAsync(id, userToken);
            check(cleared.Success == true, "clear should report success");
            var afterClear = await client.V4Lists.GetAsync(id, accessToken: userToken);
            check(afterClear.Results is null or { Count: 0 }, "list should be empty after clear");
            Console.WriteLine($"  clear:     success={cleared.Success}, " +
                              $"{afterClear.Results?.Count ?? 0} items remain");

            // --- unauthorised: a write with a bogus token -------------------------------------
            try
            {
                await client.V4Lists.UpdateAsync(id, "not-a-real-token",
                    new V4UpdateListRequest { Name = "should not happen" });
                check(false, "a bogus token should not be accepted");
            }
            catch (TmdbApiException ex)
            {
                Console.WriteLine($"  bad token: threw {(int)ex.HttpStatus} status_code={ex.StatusCode}");
                check(true, "a bogus token should throw");
            }
        }
        finally
        {
            // --- delete, which also verifies the last mutating call ------------------------------
            if (listId is { } createdId)
            {
                try
                {
                    var deleted = await client.V4Lists.DeleteAsync(createdId, userToken);
                    Console.WriteLine($"  delete:    success={deleted.Success}, status={deleted.StatusCode}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  delete FAILED ({ex.Message}) -- clean up list {listId} by hand.");
                }
            }

            await client.V4Authentication.DeleteAccessTokenAsync(userToken);
            Console.WriteLine("  revoked:   access token invalidated");
        }
    }

    /// <summary>Prints the raw shape of a per-item result block, which is what this run is for.</summary>
    static void Dump(string label, IList<V4ItemResult>? results)
    {
        if (results is null) { Console.WriteLine($"             {label}: null"); return; }
        foreach (var r in results)
            Console.WriteLine($"             {label}: " + JsonSerializer.Serialize(r,
                new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull }));
    }

    static string? ReadDotEnv()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "../../../../.env");
        if (!File.Exists(path)) return null;
        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split('=', 2);
            if (parts.Length == 2 && parts[0].Trim() == "TMDB_READ_ACCESS_TOKEN") return parts[1].Trim();
        }
        return null;
    }
}
