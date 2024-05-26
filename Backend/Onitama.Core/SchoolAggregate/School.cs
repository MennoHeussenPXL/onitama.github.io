using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.Util;
using Onitama.Core.Util.Contracts;
using System.Drawing;

namespace Onitama.Core.SchoolAggregate;

/// <inheritdoc cref="ISchool"/>
internal class School : ISchool
{
    private IPawn master;
    private IPawn[] students;
    private IPawn[] allPawns;
    public IPawn Master => master;
    public IPawn[] Students => students;
    public IPawn[] AllPawns => allPawns;
    public ICoordinate TempleArchPosition { get ; set ; }
    public School(Guid playerId)
    {
        master = new Pawn(playerId, PawnType.Master);
        allPawns = new Pawn[5];
        students = new Pawn[4];
        for (var i = 0; i < 5; i++)
        {
            IPawn pawn = new Pawn(playerId, PawnType.Student);
            if (i == 2)
            {
                allPawns[i] = master;
            }
            else
            {
                allPawns[i] = pawn;
            }
        }
        for (var j = 0; j < 5; j++)
        {
            if (allPawns[j].Type != PawnType.Master)
            {
                if (j > 2)
                {
                    students[j - 1] = allPawns[j];
                } else
                {
                    students[j] = allPawns[j];
                }
            }
        }
    }
    public IPawn GetPawn(Guid pawnId)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Creates a school that is a copy of another school.
    /// </summary>
    /// <remarks>
    /// This is an EXTRA. Not needed to implement the minimal requirements.
    /// To make the mini-max algorithm for an AI game play strategy work, this constructor should be implemented.
    /// </remarks>
    public School(ISchool otherSchool)
    {
        throw new NotImplementedException("TODO: copy properties of other school. Make sure to copy the pawns, not just reference them");
    }
}