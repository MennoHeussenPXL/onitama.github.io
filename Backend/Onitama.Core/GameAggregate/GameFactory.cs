using System.Collections;
using System.Drawing;
using Onitama.Core.GameAggregate.Contracts;
using Onitama.Core.MoveCardAggregate;
using Onitama.Core.MoveCardAggregate.Contracts;
using Onitama.Core.PlayerAggregate;
using Onitama.Core.PlayerAggregate.Contracts;
using Onitama.Core.PlayMatAggregate;
using Onitama.Core.PlayMatAggregate.Contracts;
using Onitama.Core.SchoolAggregate;
using Onitama.Core.SchoolAggregate.Contracts;
using Onitama.Core.TableAggregate;
using Onitama.Core.TableAggregate.Contracts;
using Onitama.Core.UserAggregate;
using Onitama.Core.Util;

namespace Onitama.Core.GameAggregate;

internal class GameFactory : IGameFactory
{
    private readonly IMoveCardRepository _moveCardRepository;
    public GameFactory(IMoveCardRepository moveCardRepository)
    {
        _moveCardRepository = moveCardRepository;
    }

    public IGame CreateNewForTable(ITable table)
    {
        IPlayer player = table.SeatedPlayers[0];
        IPlayer player2 = table.SeatedPlayers[1];
        PlayMat playMat = new PlayMat(table.Preferences.PlayerMatSize);
        playMat.PositionSchoolOfPlayer(player);
        playMat.PositionSchoolOfPlayer(player2);
        IPlayer[] playerArray = table.SeatedPlayers.ToArray();
        Color[] possibleStampColors = [player.Color, player2.Color];
        IMoveCard[] moveCards = _moveCardRepository.LoadSet(MoveCardSet.Original, possibleStampColors);
        Random random = new Random();
        IMoveCard[] randomMoveCards = moveCards.OrderBy(x => random.Next()).Take(5).ToArray();
        playerArray[0].MoveCards.Add(randomMoveCards[0]);
        playerArray[0].MoveCards.Add(randomMoveCards[1]);
        playerArray[1].MoveCards.Add(randomMoveCards[2]);
        playerArray[1].MoveCards.Add(randomMoveCards[3]);
        IGame newGame = new Game(Guid.NewGuid(), playMat, playerArray, randomMoveCards[4]);

        return newGame;
    }
}