using Onitama.Core.GameAggregate;
using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayerAggregate;
using Onitama.Core.PlayMatAggregate.Contracts;
using Onitama.Core.SchoolAggregate;
using Onitama.Core.SchoolAggregate.Contracts;
using System.Data.Common;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.TableAggregate;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace Onitama.Core.PlayMatAggregate;

/// <inheritdoc cref="IPlayMat"/>
internal class PlayMat : IPlayMat
{
    private IPawn[,] grid;
    private int size;

    public IPawn[,] Grid => grid;

    public int Size => size;

    /// <summary>
    /// Creates a play mat that is a copy of another play mat
    /// </summary>
    /// <param name="otherPlayMat">The play mat to copy</param>
    /// <param name="copiedPlayers">
    /// Copies of the players (with their school)
    /// that should be used to position pawn on the copy of the <paramref name="otherPlayMat"/>.</param>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public PlayMat(IPlayMat otherPlayMat, IPlayer[] copiedPlayers)
    {
        throw new NotImplementedException("TODO: copy properties of other playmat");
    }
    public PlayMat(int size)
    {
        this.size = size;
        this.grid = new IPawn[size, size];
    }
    public void ExecuteMove(IMove move, out IPawn capturedPawn)
    {
        if (grid[move.To.Row, move.To.Column] != null)
        {
            capturedPawn = grid[move.To.Row, move.To.Column];
            grid[move.To.Row, move.To.Column].Position = null;
        }
        else
        {
            capturedPawn = null;
        }
        grid[move.Pawn.Position.Row, move.Pawn.Position.Column] = null;
        grid[move.To.Row, move.To.Column] = move.Pawn;
        move.Pawn.Position = new Coordinate(move.To.Row, move.To.Column);

    }
    public IReadOnlyList<IMove> GetValidMoves(IPawn pawn, IMoveCard card, Direction playerDirection)
    {
        IList<IMove> validMoves = new List<IMove>();
        IReadOnlyList<ICoordinate> possibleTargetCoordinates = card.GetPossibleTargetCoordinates(pawn.Position, playerDirection, Size);
        foreach (ICoordinate targetCoordinate in possibleTargetCoordinates)
        {
            if ((targetCoordinate.Row >= 0 && targetCoordinate.Row < 5) && (targetCoordinate.Column >= 0 && targetCoordinate.Column < 5))
            {
                if (grid[targetCoordinate.Row, targetCoordinate.Column] != null)
                {
                    if (grid[targetCoordinate.Row, targetCoordinate.Column].OwnerId != pawn.OwnerId)
                    {
                        validMoves.Add(new Move(card, pawn, playerDirection, targetCoordinate));
                    }
                }
            }
        }
        return (IReadOnlyList<IMove>)validMoves;
    }
    public void PositionSchoolOfPlayer(IPlayer player)
    {
        int row;
        IPawn[] pawns = player.School.AllPawns;
        if (player.Direction == "North")
        {
            row = 0;
        } else {
            row = grid.GetLength(0) - 1;
        }
        for (int col = 0; col < pawns.Length; col++)
        {
            IPawn pawn = pawns[col];
            pawn.Position = new Coordinate(row, col);
            grid[row, col] = pawn;
        }
    }
}