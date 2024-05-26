using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.MoveCardAggregate;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;

namespace Onitama.Core.GameAggregate;


internal class Move : IMove
{
    private IMoveCard card;
    private IPawn pawn;
    private Direction playerDirection;
    private ICoordinate to;
    public IMoveCard Card => card;

    public IPawn Pawn => pawn;

    public Direction PlayerDirection => playerDirection;

    public ICoordinate To => to;

    public Move(IMoveCard card)
    {
        this.card = card;
    }

    public Move(IMoveCard card, IPawn pawn, Direction playerDirection, ICoordinate to)
    {
        this.card = card;
        this.pawn = pawn;
        this.playerDirection = playerDirection;
        this.to = to;
    }
}