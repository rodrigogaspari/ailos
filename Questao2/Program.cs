using Newtonsoft.Json;
using System.Net.Http.Headers;

public class Program
{
    public static async Task Main()
    {
        
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://jsonmock.hackerrank.com/api/football_matches");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        int? totalGooals = 0;

        //Total de gols como Team1
        totalGooals += await GetFootballMatchsPerTeam(team, year, client, TeamGameType.Team1);

        //Total de gols como Team2
        totalGooals += await GetFootballMatchsPerTeam(team, year, client, TeamGameType.Team2);

        return totalGooals.Value;
    }

    static async Task<int?> GetFootballMatchsPerTeam(string team, int year, HttpClient client, TeamGameType teamType)
    {
        int? totalGooals = 0; 

        ResponseFootballMatch footballMatchs = new() { Page = 0, TotalPages = 1 };

        while (footballMatchs.Page <= footballMatchs.TotalPages)
        {
            string path = string.Concat(client.BaseAddress, $"?page={footballMatchs.Page + 1}&year={year}&team{(int)teamType}={team}");

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                footballMatchs = await response.Content.ReadAsAsync<ResponseFootballMatch>();
            }

            if (teamType == TeamGameType.Team1)
                totalGooals += footballMatchs.Data?.Select(x => x.Team1goals).Sum();
            else
                totalGooals += footballMatchs.Data?.Select(x => x.Team2goals).Sum();
        }

        return totalGooals; 
    }


    public enum TeamGameType
    { 
        Team1 = 1,
        Team2 = 2 
    }

    public class ResponseFootballMatch
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        public IList<FootballMatch>? Data { get; set; }
    }

    public class FootballMatch
    {
        public string? Competition { get; set; }

        public int Year { get; set; }

        public string? Round { get; set; }

        public string? Team1 { get; set; }

        public string? Team2 { get; set; }

        public int Team1goals { get; set; }

        public int Team2goals { get; set; }
    }
}