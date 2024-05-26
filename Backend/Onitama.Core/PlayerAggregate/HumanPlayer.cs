using System.Drawing;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.Util;

namespace Onitama.Core.PlayerAggregate;

/// <inheritdoc cref="IPlayer"/>
internal class HumanPlayer : PlayerBase
{
    public HumanPlayer(Guid userId, string name, Color color, Direction direction) : base(userId, name, color, direction)
    {
    }
}