using System;
using System.Diagnostics.Metrics;
using System.Drawing;
using Onitama.Core.MoveCardAggregate;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.SchoolAggregate;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.TableAggregate;
using Onitama.Core.Util;

namespace Onitama.Core.PlayerAggregate;

/// <inheritdoc cref="IPlayer"/>
internal class PlayerBase : IPlayer
{
    private Guid id;
    private string name;
    private Color color;
    private Direction direction;
    private ISchool school; 
    private IList<IMoveCard> moveCards = new List<IMoveCard>();
    public Guid Id => id;
    public string Name => name;
    public Color Color => color;
    public Direction Direction => direction;
    public ISchool School => school;
    public IList<IMoveCard> MoveCards => moveCards;
    protected PlayerBase(Guid id, string name, Color color, Direction direction)
    {
        this.id = id;
        this.name = name;
        this.color = color;
        this.direction = direction;
        school = new School(id);
    }
    /// <summary>
    /// Creates a player that is a copy of an other player.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public PlayerBase(IPlayer otherPlayer)
    {
        throw new NotImplementedException("TODO: copy properties of other player");
    }
}