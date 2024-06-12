using System.Net.Http.Json;
using Exciting.TeamModel;

namespace Exciting.TeamClient;

public class TeamApiClient(HttpClient httpClient)
{
    public async Task<TeamMember[]> GetMembersAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<TeamMember>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<TeamMember>("/members", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
                break;

            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}
