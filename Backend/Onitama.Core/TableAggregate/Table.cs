using System.Drawing;
using System.Numerics;
using Onitama.Core.PlayerAggregate;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.TableAggregate.Contracts;
using Onitama.Core.TableAggregate;
using Onitama.Core.UserAggregate;
using Onitama.Core.Util;

namespace Onitama.Core.TableAggregate;

/// <inheritdoc cref="ITable"/>
internal class Table : ITable
{
    private static readonly Color[] PossibleColors =
        new[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange };
    private Guid id;
    private TablePreferences preferences = new TablePreferences();
    private Guid ownerPlayerId;
    private IList<IPlayer> seatedPlayers = new List<IPlayer>();
    private Guid gameId = Guid.Empty;

    public Guid Id => id; 
    public TablePreferences Preferences => preferences;
    public Guid OwnerPlayerId => ownerPlayerId;
    public IList<IPlayer> SeatedPlayers => seatedPlayers;
    public bool HasAvailableSeat => seatedPlayers.Count() < preferences.NumberOfPlayers;
    public Guid GameId { get => gameId; set => gameId = value; }

    public Table(Guid id, TablePreferences preferences)
    {
        this.id = id;
        this.preferences = preferences;
    }
    public void Join(User user)
    {
        if (!HasAvailableSeat)
        {
            throw new InvalidOperationException("The table is full.");
        }
        foreach (var seatedPlayer in seatedPlayers)
        {
            if (seatedPlayer.Id == user.Id)
            {
                throw new InvalidOperationException("User is already seated.");
            }
        }
        Random rnd = new Random();
        if (seatedPlayers.Count == 0)
        {
            Color randomColor = PossibleColors[rnd.Next(PossibleColors.Length)];
            IPlayer player = new HumanPlayer(user.Id, user.WarriorName, randomColor, "north");
            seatedPlayers.Add(player);
            ownerPlayerId = user.Id;
        } else {
            Color randomColor;
            do
            {
                randomColor = PossibleColors[rnd.Next(PossibleColors.Length)];
            }
            while (seatedPlayers.Any(p => p.Color == randomColor));
            IPlayer player = new HumanPlayer(user.Id, user.WarriorName, randomColor, "south");
            seatedPlayers.Add(player);
        }
    }

    public void Leave(Guid userId)
    {
        foreach (var seatedPlayer in seatedPlayers)
        {
            if (seatedPlayer.Id == userId)
            {
                seatedPlayers.Remove(seatedPlayer);
                if (ownerPlayerId == userId && seatedPlayers.Count() > 0)
                {
                    ownerPlayerId = seatedPlayers[0].Id;
                }
                return;
            }
        }
        throw new InvalidOperationException();
    }

    public void FillWithArtificialPlayers(IGamePlayStrategy gamePlayStrategy)
    {
        // Implementation to fill the table with computer players (optional, not required for minimal requirements)
        throw new NotImplementedException();
    }
}