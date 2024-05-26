using Onitama.Core.GameAggregate;
using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.TableAggregate.Contracts;
using Onitama.Core.UserAggregate;
using System.Runtime.CompilerServices;

namespace Onitama.Core.TableAggregate;

/// <inheritdoc cref="ITableManager"/>
internal class TableManager : ITableManager
{
    private readonly ITableRepository _tableRepository;
    private readonly ITableFactory _tableFactory;
    private readonly IGameRepository _gameRepository;
    private readonly IGameFactory _gameFactory;
    private readonly IGamePlayStrategy _gamePlayStrategy;
    public TableManager(ITableRepository tableRepository, ITableFactory tableFactory, IGameRepository gameRepository,IGameFactory gameFactory,IGamePlayStrategy gamePlayStrategy)
    {
        _tableRepository = tableRepository;
        _tableFactory = tableFactory;
        _gameRepository = gameRepository;
        _gameFactory = gameFactory;
        _gamePlayStrategy = gamePlayStrategy;
    }

    public ITable AddNewTableForUser(User user, TablePreferences preferences)
    {
        ITable newTable = _tableFactory.CreateNewForUser(user, preferences);

        _tableRepository.Add(newTable);

        return newTable;
    }

    public void JoinTable(Guid tableId, User user)
    {
        _tableRepository.Get(tableId).Join(user);
    }

    public void LeaveTable(Guid tableId, User user)
    {
        ITable table = _tableRepository.Get(tableId);
        table.Leave(user.Id);
        if (table.SeatedPlayers.Count == 0)
        {
            _tableRepository.Remove(tableId);
        }
    }

    public void FillWithArtificialPlayers(Guid tableId, User user)
    {
        throw new NotImplementedException();
    }

    public IGame StartGameForTable(Guid tableId, User user)
    {
        ITable table = _tableRepository.Get(tableId);
        if (table.OwnerPlayerId != user.Id)
        {
            throw new InvalidOperationException("the user is not the owner of the table");
        }
        if (table.HasAvailableSeat)
        {
            throw new InvalidOperationException("not enough players in the table");
        }
        IGame newGame = _gameFactory.CreateNewForTable(table);
        table.GameId = newGame.Id;

        _gameRepository.Add(newGame);

        return newGame;
    }
}