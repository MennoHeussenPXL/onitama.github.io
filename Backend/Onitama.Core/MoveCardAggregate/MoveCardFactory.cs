using System.Drawing;
using Onitama.Core.MoveCardAggregate.Contracts;

namespace Onitama.Core.MoveCardAggregate;

/// <inheritdoc cref="IMoveCardFactory"/>
internal class MoveCardFactory : IMoveCardFactory
{
    public IMoveCard Create(string name, MoveCardGridCellType[,] grid, Color[] possibleStampColors)
    {
        Random random = new Random();
        int randomIndex = random.Next(0, possibleStampColors.Length);
        Color randomColor = possibleStampColors[randomIndex];

        IMoveCard newMoveCard = new MoveCard(name, grid, randomColor);
        return newMoveCard;
    }
}