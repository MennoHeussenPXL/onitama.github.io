using Onitama.Core.GameAggregate;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.TableAggregate;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;

namespace Onitama.Core.SchoolAggregate;

/// <inheritdoc cref="IPawn"/>
internal class Pawn : IPawn
{
    private Guid id;
    private Guid ownerId;
    private PawnType type;
    private ICoordinate position;
    public Guid Id => id;
    public Guid OwnerId => ownerId;
    public PawnType Type => type;
    public ICoordinate Position { get => position; set => position = value; }
    public Pawn(Guid ownerId, PawnType type)
    {
        this.id = Guid.NewGuid();
        this.ownerId = ownerId;
        this.type = type;
    }
}