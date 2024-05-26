using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ExceptionServices;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayMatAggregate;
using Onitama.Core.PlayMatAggregate.Contracts;
using Onitama.Core.SchoolAggregate;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Onitama.Core.MoveCardAggregate;

/// <inheritdoc cref="IMoveCard"/>
internal class MoveCard : IMoveCard
{
    private string name;
    private MoveCardGridCellType[,] grid;
    private Color stampColor;
    public string Name => name;
    public MoveCardGridCellType[,] Grid => grid;

    public Color StampColor => stampColor;

    public MoveCard(string name, MoveCardGridCellType[,] grid, Color stampColor)
    {
        this.name = name;
        this.grid = grid;
        this.stampColor = stampColor;
    }

    //Do not change this method, it makes sure that two MoveCard instances are equal if their names are equal
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        return obj is IMoveCard other && Equals(other);
    }

    //Do not change this method
    protected bool Equals(IMoveCard other)
    {
        return Name == other.Name;
    }

    //Do not change this method
    public override int GetHashCode()
    {
        return (Name != null ? Name.GetHashCode() : 0);
    }

    public IReadOnlyList<ICoordinate> GetPossibleTargetCoordinates(ICoordinate startCoordinate, Direction playDirection, int matSize)
    {
        IList<ICoordinate> possibleTargetCoordinates = new List<ICoordinate>();
        if (playDirection == Direction.South)
        {
            int moveCardCenterRow = grid.GetLength(0) / 2;
            int moveCardCenterCol = grid.GetLength(1) / 2;
            for (int i = grid.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = grid.GetLength(1) - 1; j >= 0; j--)
                {
                    if (grid[i, j] == MoveCardGridCellType.Target)
                    {
                        IList<int> flipedNumberList = new List<int>(new int[] { 4, 3, 2, 1, 0 });
                        int moveOffsetRow = flipedNumberList.IndexOf(i) - moveCardCenterRow;
                        int moveOffsetCol = flipedNumberList.IndexOf(j) - moveCardCenterCol;
                        int newX = startCoordinate.Row + moveOffsetRow;
                        int newY = startCoordinate.Column + moveOffsetCol;
                        if (newX >= 0 && newX < matSize && newY >= 0 && newY < matSize)
                        {
                            possibleTargetCoordinates.Add(new Coordinate(newX, newY));
                        }
                    }
                }
            }
        } 
        else if (playDirection == Direction.North)
        {
            int moveCardCenterRow = grid.GetLength(0) / 2;
            int moveCardCenterCol = grid.GetLength(1) / 2;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == MoveCardGridCellType.Target)
                    {
                        int moveOffsetRow = i - moveCardCenterRow;
                        int moveOffsetCol = j - moveCardCenterCol;
                        int newX = startCoordinate.Row + moveOffsetRow;
                        int newY = startCoordinate.Column + moveOffsetCol;
                        
                        if (newX >= 0 && newX < matSize && newY >= 0 && newY < matSize)
                        {
                            possibleTargetCoordinates.Add(new Coordinate(newX, newY));
                        }
                    }
                }
            }
        }
        return (IReadOnlyList<ICoordinate>)possibleTargetCoordinates;
    }
}