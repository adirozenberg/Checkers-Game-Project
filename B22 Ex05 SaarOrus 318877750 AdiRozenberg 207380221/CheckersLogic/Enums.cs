using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Enums
    {
        public enum eCoinsOnBoard
        {
            Player1 = 'X',
            Player2 = 'O',
            Blank = ' ',
            KingPlayer1 = 'K',
            KingPlayer2 = 'U'
        }

        public enum ePlayerType
        {
            Human = 0,
            Computer = 1
        }

        public enum eMoveStatus
        {
            ValidMove = 1,
            MustToDoSkipMove,
            InvalidMove
        }
    }
}
