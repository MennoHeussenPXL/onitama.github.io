using System.Numerics;
using Onitama.Core.Util.Contracts;

namespace Onitama.Core.Util;

/// <inheritdoc cref="ICoordinate"/>
internal class Coordinate : ICoordinate
{
    private int row;
    private int column;
    public int Row  => row;
    public int Column => column;
    public Coordinate(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    //Do not change this method
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        return obj is ICoordinate other && Equals(other);
    }

    //Do not change this method
    protected bool Equals(ICoordinate other)
    {
        return Row == other.Row && Column == other.Column;
    }

    //Do not change this method
    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }

    //Do not change this method
    public override string ToString()
    {
        return $"({Row}, {Column})";
    }

    public bool IsOutOfBounds(int playMatSize)
    {
        throw new NotImplementedException();
    }

    public ICoordinate GetNeighbor(Direction direction)
    {
        throw new NotImplementedException();
    }

    public ICoordinate RotateTowards(Direction direction)
    {
        int newRow, newColumn;
        switch (direction.ToString().ToLower())
        {
            case "north":
                newRow = this.Row;
                newColumn = this.Column;
                break;
            case "east":
                newRow = -this.Column;
                newColumn = this.Row;
                break;
            case "south":
                newRow = -this.Row;
                newColumn = -this.Column;
                break;
            case "west":
                newRow = this.Column;
                newColumn = -this.Row;
                break;
            default:
                throw new ArgumentException("Invalid direction");
        }

        return new Coordinate(newRow, newColumn);
    }
    public int GetDistanceTo(ICoordinate other)
    {
        if (other is Coordinate otherCoordinate)
        {
            int xDiff = this.Column - otherCoordinate.Column;
            int yDiff = this.Row - otherCoordinate.Row;
            return (int)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }
        throw new ArgumentException("The provided ICoordinate is not of type Coordinate.");
    }
}