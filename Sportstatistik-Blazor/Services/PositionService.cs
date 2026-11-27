using MongoDB.Bson;
using Sportstatistik.Models;

namespace Sportstatistik.Services;

public class PositionService(ILeagueRepository repository) : IPositionService
{
    public List<Position> GetAll()
    {
        EnsureDefaultPositions();
        return repository.GetAllPositions();
    }

    public void Add(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Skriv ett namn på positionen.");
        }

        name = name.Trim();

        if (repository.GetAllPositions().Any(position => position.Name == name))
        {
            throw new ArgumentException($"Positionen \"{name}\" finns redan.");
        }
        
        repository.AddPosition(new Position { Name = name});
    }

    public void Delete(ObjectId positionId)
    {
        var position = repository.GetAllPositions().FirstOrDefault(position => position.Id == positionId);
        if (position is null)
        {
            throw new ArgumentException("Positionen finns inte");
        }

        if (position.IsProtected)
        {
            throw new ArgumentException($"{position.Name} kan inte tas bort.");
        }
        
        repository.MovePlayersToPosition(position.Name, "Övrigt");
        repository.DeletePosition(positionId);
    }
    
    private void EnsureDefaultPositions()
    {
        if (repository.GetAllPositions().Count > 0)
        {
            return;
        }

        repository.AddPosition(new Position { Name = "Målvakt" });
        repository.AddPosition(new Position { Name = "Back" });
        repository.AddPosition(new Position { Name = "Mittfält" });
        repository.AddPosition(new Position { Name = "Forward" });
        repository.AddPosition(new Position { Name = "Övrigt", IsProtected = true });
    }
}