using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.PlayerAggregate;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;

namespace Onitama.Core.GameAggregate;

internal class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IGame GetGame(Guid gameId)
    {
        return _gameRepository.GetById(gameId);
    }

    public IReadOnlyList<IMove> GetPossibleMovesForPawn(Guid gameId, Guid playerId, Guid pawnId, string moveCardName)
    {
        IGame game = _gameRepository.GetById(gameId);
        if (game == null)
            throw new KeyNotFoundException("Game not found.");

        return game.GetPossibleMovesForPawn(playerId, pawnId, moveCardName);
    }

    public void MovePawn(Guid gameId, Guid playerId, Guid pawnId, string moveCardName, ICoordinate to)
    {
        IGame game = _gameRepository.GetById(gameId);
        if (game == null)
            throw new KeyNotFoundException("Game not found.");

       
        var possibleMoves = game.GetPossibleMovesForPawn(playerId, pawnId, moveCardName);
        if (!possibleMoves.Any(m => m.Pawn.Id == pawnId && m.To.Equals(to)))
            throw new InvalidOperationException("Invalid move.");

        game.MovePawn(playerId, pawnId, moveCardName, to);
    }

    public void SkipMovementAndExchangeCard(Guid gameId, Guid playerId, string moveCardName)
    {
        IGame game = _gameRepository.GetById(gameId);
        if (game == null)
            throw new KeyNotFoundException("Game not found.");

        game.SkipMovementAndExchangeCard(playerId, moveCardName);
    }
}