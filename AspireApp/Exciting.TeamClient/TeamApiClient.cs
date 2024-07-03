using System.Net.Http.Json;
using Exciting.TeamModel;

namespace Exciting.TeamClient;

public class TeamApiClient(HttpClient httpClient)
{
    public async Task<TeamMember[]> GetMembersAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var members = new List<TeamMember>();

        await foreach (var member in httpClient.GetFromJsonAsAsyncEnumerable<TeamMember>("/members", cancellationToken))
        {
            if (members.Count >= maxItems)
                break;

            if (member is null)
                continue;

            members.Add(member);
        }

        return members.ToArray();
    }
}
