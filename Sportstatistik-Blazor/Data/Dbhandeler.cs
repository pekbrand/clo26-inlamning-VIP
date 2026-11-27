using MongoDB.Driver;
using Sportstatistik.Models;

namespace Sportstatistik.Data;


public sealed class SportDatabase
{
    private readonly IMongoDatabase _database;

    public SportDatabase(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "MongoDB-anslutningen saknas. Ladda .env innan du startar appen.");
        }

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("sportstatistik");
    }

    public IMongoCollection<Team> Teams =>
        _database.GetCollection<Team>("teams");

    public IMongoCollection<Player> Players =>
    _database.GetCollection<Player>("players");

    public IMongoCollection<Match> Matches =>
        _database.GetCollection<Match>("matches");

    public IMongoCollection<Position> Positions =>
        _database.GetCollection<Position>("positions");
}