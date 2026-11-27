using Sportstatistik.Models;
using MongoDB.Bson;

namespace Sportstatistik.Services;

/// <summary>
/// Låtsasdatabas: allt ligger i listor i minnet och försvinner när programmet stängs.
/// Den finns bara för att sidorna ska gå att köra från dag ett.
/// Ersätt den med en egen klass som implementerar ILeagueRepository mot er databas,
/// och byt sedan en enda rad i Program.cs (registreringen av ILeagueRepository).
/// </summary>
public class InMemoryLeagueRepository : ILeagueRepository
{
    private readonly List<Position> _positions = new();
    private readonly List<Team> _teams = new();
    private readonly List<Player> _players = new();
    private readonly List<Match> _matches = new();
  

    public InMemoryLeagueRepository()
    {
        Seed();
    }

    public List<Team> GetAllTeams() => _teams.ToList();

    public Team? GetTeam(ObjectId id) =>
    _teams.FirstOrDefault(team => team.Id == id);

    public void AddTeam(Team team)
    {
        // Du behöver bygga ut detta mer: fundera på om två lag får heta samma sak.
        if (team.Id == ObjectId.Empty)
        {
            team.Id = ObjectId.GenerateNewId();
        }
        _teams.Add(team);
    }

    
    public List<Player> GetPlayers(ObjectId teamId) =>
    _players.Where(player => player.TeamId == teamId).ToList();

    public void AddPlayer(Player player)
    {
        if (player.Id == ObjectId.Empty)
        {
            player.Id = ObjectId.GenerateNewId();
        }
        
        _players.Add(player);
    }

    public List<Match> GetAllMatches() => _matches.ToList();

    public Match? GetMatch(ObjectId id) =>
    _matches.FirstOrDefault(match => match.Id == id);

    public void AddMatch(Match match)
    {
        // Du behöver bygga ut detta mer: ett lag får inte möta sig självt (story 6).
        // Kasta ArgumentException med ett meddelande på svenska, så visar sidan texten.
        if (match.Id == ObjectId.Empty)
        {
            match.Id = ObjectId.GenerateNewId();
        }
        
        _matches.Add(match);
    }

    public void DeleteMatch(ObjectId id) =>
    _matches.RemoveAll(match => match.Id == id);
    
    public List<Position> GetAllPositions() => _positions.ToList();

    public void AddPosition(Position position)
    {
        if (position.Id == ObjectId.Empty)
        {
            position.Id = ObjectId.GenerateNewId();
        }
        _positions.Add(position);
    }

    public void DeletePosition(ObjectId id) =>
        _positions.RemoveAll(position => position.Id == id);

    public void MovePlayersToPosition(string fromPosition, string toPosition)
    {
        foreach (var player in _players.Where(player => player.Position == fromPosition))
        {
            player.Position = toPosition;
        }
    }

    // Exempeldata, så att det finns något att titta på direkt.
    // Er riktiga databas ska seeda sig själv på samma sätt vid första start, om den är tom.
    // Här passar till exempel mönstret Builder: en match med lag, mål och datum blir lättare att läsa om den
    // byggs steg för steg, till exempel .Home(ummis).Away(lagret).Score(2, 1).
    private void Seed()
    {
        var ummis = new Team { Name = "Ummis IF" };
        var bryggeriet = new Team { Name = "Bryggeriet BK" };
        var kontoret = new Team { Name = "Kontoret FF" };
        var lagret = new Team { Name = "Lagret United" };

        AddTeam(ummis);
        AddTeam(bryggeriet);
        AddTeam(kontoret);
        AddTeam(lagret);

        AddPlayer(new Player { TeamId = ummis.Id, FirstName = "Alex", LastName = "Exempelsson", Position = "Målvakt" });
        AddPlayer(new Player { TeamId = ummis.Id, FirstName = "Robin", LastName = "Påhittad", Position = "Back" });
        AddPlayer(new Player { TeamId = ummis.Id, FirstName = "Kim", LastName = "Testsson", Position = "Mittfält" });
        AddPlayer(new Player { TeamId = ummis.Id, FirstName = "Sam", LastName = "Demosson", Position = "Forward" });
        AddPlayer(new Player { TeamId = bryggeriet.Id, FirstName = "Charlie", LastName = "Exempelsson", Position = "Målvakt" });
        AddPlayer(new Player { TeamId = bryggeriet.Id, FirstName = "Noa", LastName = "Påhittad", Position = "Forward" });
        AddPlayer(new Player { TeamId = kontoret.Id, FirstName = "Love", LastName = "Testsson", Position = "Back" });
        AddPlayer(new Player { TeamId = kontoret.Id, FirstName = "Mika", LastName = "Demosson", Position = "Övrigt" });
        AddPlayer(new Player { TeamId = lagret.Id, FirstName = "Ellis", LastName = "Exempelsson", Position = "Mittfält" });
        AddPlayer(new Player { TeamId = lagret.Id, FirstName = "Juno", LastName = "Påhittad", Position = "Back" });
        AddPlayer(new Player { TeamId = lagret.Id, FirstName = "Tove", LastName = "Testsson", Position = "Forward" });
        AddPlayer(new Player { TeamId = lagret.Id, FirstName = "Vide", LastName = "Demosson", Position = "Målvakt" });

        AddMatch(new Match { HomeTeamId = ummis.Id, AwayTeamId = bryggeriet.Id, HomeGoals = 2, AwayGoals = 1, Date = new DateTime(2026, 11, 26) });
        AddMatch(new Match { HomeTeamId = kontoret.Id, AwayTeamId = lagret.Id, HomeGoals = 0, AwayGoals = 0, Date = new DateTime(2026, 11, 26) });
        AddMatch(new Match { HomeTeamId = bryggeriet.Id, AwayTeamId = kontoret.Id, HomeGoals = 3, AwayGoals = 1, Date = new DateTime(2026, 11, 26) });
        AddMatch(new Match { HomeTeamId = lagret.Id, AwayTeamId = ummis.Id, HomeGoals = 1, AwayGoals = 2,  Date = new DateTime(2026, 11, 26) });
        AddMatch(new Match { HomeTeamId = ummis.Id, AwayTeamId = kontoret.Id, HomeGoals = 1, AwayGoals = 1,  Date = new DateTime(2026, 11, 26) });
        AddMatch(new Match { HomeTeamId = lagret.Id, AwayTeamId = bryggeriet.Id, HomeGoals = 2, AwayGoals = 2, Date = new DateTime(2026, 11, 26) });
    }
}
