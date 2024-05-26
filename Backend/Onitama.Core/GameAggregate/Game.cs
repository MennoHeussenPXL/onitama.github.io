using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.MoveCardAggregate;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayerAggregate;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.PlayMatAggregate;
using Onitama.Core.PlayMatAggregate.Contracts;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.TableAggregate;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;
using System.Collections.Generic;
using System.Numerics;

namespace Onitama.Core.GameAggregate;

/// <inheritdoc cref="IGame"/>
internal class Game : IGame
{
    private Guid id;
    private IPlayMat playMat;
    private IMoveCard extraMoveCard;
    private IPlayer[] players;
    private Guid playerToPlayId;
    private Guid winnerPlayerId;
    private string winnerMethod;
    public Guid Id => id;

    public IPlayMat PlayMat => playMat;

    public IMoveCard ExtraMoveCard => extraMoveCard;

    public IPlayer[] Players => players;

    public Guid PlayerToPlayId => playerToPlayId;

    public Guid WinnerPlayerId => winnerPlayerId;

    public string WinnerMethod => winnerMethod;

    /// <summary>
    /// Creates a new game and determines the player to play first.
    /// </summary>
    /// <param name="id">The unique identifier of the game</param>
    /// <param name="playMat">
    /// The play mat
    /// (with the schools of the player already positioned on it)
    /// </param>
    /// <param name="players">
    /// The 2 players that will play the game
    /// (with 2 move cards each)
    /// </param>
    /// <param name="extraMoveCard">
    /// The fifth card used to exchange cards after the first move
    /// </param>
    public Game(Guid id, IPlayMat playMat, IPlayer[] players, IMoveCard extraMoveCard)
    {
        this.id = id;
        this.playMat = playMat;
        this.players = players;
        this.extraMoveCard = extraMoveCard;
        //playerToPlayId = players[0].Id;
        playerToPlayId = players.FirstOrDefault(p => p.Color == extraMoveCard.StampColor)?.Id ?? players[0].Id;
    }

    /// <summary>
    /// Creates a game that is a copy of another game.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public Game(IGame otherGame)
    {
        throw new NotImplementedException("TODO: copy the properties of the other game");
        //Attention: the players should be copied, not just referenced
    }

    public IReadOnlyList<IMove> GetPossibleMovesForPawn(Guid playerId, Guid pawnId, string moveCardName)
    {
        if (!players.Any(player => player.Id == playerId))
        {
            throw new InvalidOperationException("Player is not in this game");
        }
        IPlayer player = players.FirstOrDefault(player => player.Id == playerId);
        if (!player.MoveCards.Any(moveCard => moveCard.Name == moveCardName))
        {
            throw new ApplicationException("Player not in possession of the card");
        }
        IMoveCard moveCard = player.MoveCards.FirstOrDefault(moveCard => moveCard.Name == moveCardName);
        IPawn pawn = player.School.AllPawns.FirstOrDefault(pawn => pawn.Id == pawnId);
        IReadOnlyList<IMove> validMoves = playMat.GetValidMoves(pawn, moveCard, player.Direction);
        return validMoves;
    }

    public IReadOnlyList<IMove> GetAllPossibleMovesFor(Guid playerId)
    {
        IPlayer player = players.FirstOrDefault(player => player.Id == playerId);
        IPawn[] pawns = player.School.AllPawns;
        IList<IMoveCard> moveCards = player.MoveCards;
        IList<IMove> allPossibleMoves = new List<IMove>();
        foreach (var pawn in pawns)
        {
            foreach (var moveCard in moveCards)
            {
                IReadOnlyList<IMove> validMoves = playMat.GetValidMoves(pawn, moveCard, player.Direction);
                foreach (var move in validMoves)
                {
                    allPossibleMoves.Add(move);
                }
            }
        }
        return (IReadOnlyList<IMove>)allPossibleMoves;
    }

    public void MovePawn(Guid playerId, Guid pawnId, string moveCardName, ICoordinate to)
    {
        if (playerId != PlayerToPlayId)
        {
            throw new ApplicationException("It's not the player's turn to move.");
        }
        IPlayer player = Players.FirstOrDefault(player => player.Id == playerId);
        IPlayer OtherPlayer = Players.FirstOrDefault(player => player.Id != playerId);
        IPawn pawn = player.School.AllPawns.FirstOrDefault(pawn => pawn.Id == pawnId);
        IMoveCard MoveCard = player.MoveCards.FirstOrDefault(moveCard => moveCard.Name == moveCardName);
        IMove move = new Move(MoveCard, pawn, player.Direction, to);
        IPawn capturedPawn;
        playMat.ExecuteMove(move, out capturedPawn);
        if (capturedPawn != null)
        {
            if (capturedPawn.Type == PawnType.Master)
            {
                winnerPlayerId = playerId;
                winnerMethod = "Way of the stone";
            }
        }
        if (pawn.Type == PawnType.Master)
        {
            ICoordinate startCoordinate = OtherPlayer.Direction.GetStartCoordinate(PlayMat.Size);
            if (to.Row == startCoordinate.Row && to.Column == startCoordinate.Column)
            {
                winnerPlayerId = playerId;
                winnerMethod = "Way of the Stream";
            }
        }
    }


    public void SkipMovementAndExchangeCard(Guid playerId, string moveCardName)
    {
        if (playerId != PlayerToPlayId)
        {
            throw new ApplicationException("It's not the player's turn to move.");
        }
        if (GetAllPossibleMovesFor(playerId).Count > 0)
        {
            throw new ApplicationException("The player has a valid move or more and cannot skip.");
        }

        IPlayer player = Players.FirstOrDefault(player => player.Id == playerId);
        IMoveCard moveCard = player.MoveCards.FirstOrDefault(mc => mc.Name == moveCardName);
        if (moveCard == null)
        {
            throw new InvalidOperationException("Player does not have the specified move card.");
        }

        player.MoveCards.Remove(moveCard);
        player.MoveCards.Add(extraMoveCard);
        extraMoveCard = moveCard;


        playerToPlayId = GetNextOpponent(playerId).Id;
    }

    public IPlayer GetNextOpponent(Guid playerId)
    {
        IPlayer player = Players.FirstOrDefault(player => player.Id != playerId);
        return player;
    }
}