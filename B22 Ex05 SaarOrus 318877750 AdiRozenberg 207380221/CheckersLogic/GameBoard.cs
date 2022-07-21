using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class GameBoard
    {
        private readonly int m_SizeOfBoard;
        private Enums.eCoinsOnBoard[,] m_CheckersGameBoard;
        
        public Enums.eCoinsOnBoard[,] CheckersGameBoard
        {
            get { return m_CheckersGameBoard; }
            set { m_CheckersGameBoard = value; }
        }

        public int SizeOfBoard
        {
            get { return m_SizeOfBoard; }
        }
        //// ctor
        public GameBoard(int i_Size)
        {
            m_SizeOfBoard = i_Size;
            m_CheckersGameBoard = new Enums.eCoinsOnBoard[m_SizeOfBoard, m_SizeOfBoard];
            InitializeGameBoard();
        }

        public void AddToGameBoard(Move i_CurrentMove, Enums.eCoinsOnBoard i_Sign)
        {
            m_CheckersGameBoard[i_CurrentMove.EndPoint.Row, i_CurrentMove.EndPoint.Col] = i_Sign;
        }

        public void DeleteFromGameBoard(Move i_CurrentMove, Player i_RivalPlayer, Player i_CurrentPlayer)
        {
            if (i_CurrentMove.IsSkipMove)
            {
                DeleteEatenPlayerInBoard(i_CurrentMove, i_RivalPlayer, i_CurrentPlayer);
            }

            m_CheckersGameBoard[i_CurrentMove.StartPoint.Row, i_CurrentMove.StartPoint.Col] = Enums.eCoinsOnBoard.Blank;
        }

        public void DeleteEatenPlayerInBoard(Move i_CurrentMove, Player o_RivalPlayer, Player io_CurrentPlayer)
        {
            Move.Point eatenPlayerPoint = new Move.Point();
            
            eatenPlayerPoint.Row = (i_CurrentMove.StartPoint.Row + i_CurrentMove.EndPoint.Row) / 2;
            eatenPlayerPoint.Col = (i_CurrentMove.StartPoint.Col + i_CurrentMove.EndPoint.Col) / 2;
            if (m_CheckersGameBoard[eatenPlayerPoint.Row, eatenPlayerPoint.Col] == o_RivalPlayer.Figure)
            {
                o_RivalPlayer.CounterOfRegularPlayers--;
                io_CurrentPlayer.Score++;
            }
            else
            {
                o_RivalPlayer.CounterOfKingPlayers--;
            }

            m_CheckersGameBoard[eatenPlayerPoint.Row, eatenPlayerPoint.Col] = Enums.eCoinsOnBoard.Blank;
        }

        public void UpdateMoveOnGameBoard(Move i_CurrentMove, Player io_CurrentPlayer, Player i_RivalPlayer)
        {
            if (io_CurrentPlayer.Figure == Enums.eCoinsOnBoard.Player1)
            {
                if (CheckIfTransferToKing(i_CurrentMove, io_CurrentPlayer) || m_CheckersGameBoard[i_CurrentMove.StartPoint.Row, i_CurrentMove.StartPoint.Col] == Enums.eCoinsOnBoard.KingPlayer1)
                {
                    AddToGameBoard(i_CurrentMove, Enums.eCoinsOnBoard.KingPlayer1);
                }
                else
                {
                    AddToGameBoard(i_CurrentMove, io_CurrentPlayer.Figure);                   
                }
            }
            else
            {
                if (CheckIfTransferToKing(i_CurrentMove, io_CurrentPlayer) || m_CheckersGameBoard[i_CurrentMove.StartPoint.Row, i_CurrentMove.StartPoint.Col] == Enums.eCoinsOnBoard.KingPlayer2)
                {
                    AddToGameBoard(i_CurrentMove, Enums.eCoinsOnBoard.KingPlayer2);                    
                }
                else
                {
                    AddToGameBoard(i_CurrentMove, io_CurrentPlayer.Figure);                    
                }
            }

            DeleteFromGameBoard(i_CurrentMove, i_RivalPlayer, io_CurrentPlayer);
            io_CurrentPlayer.LastTurnOfPlayer = i_CurrentMove;
        }

        public void InitializeGameBoard()
        {
            int numOfCoinsLines = (m_SizeOfBoard / 2) - 1;

            for (int i = 0; i < m_SizeOfBoard; i++)
            {
                for (int j = 0; j < m_SizeOfBoard; j++)
                {
                    if ((i + j) % 2 != 0 && i < numOfCoinsLines)
                    {
                        m_CheckersGameBoard[i, j] = Enums.eCoinsOnBoard.Player2;
                    }
                    else if ((i + j) % 2 == 0 && i > numOfCoinsLines && i < numOfCoinsLines + 2)
                    {
                        m_CheckersGameBoard[i, j] = Enums.eCoinsOnBoard.Blank;
                    }
                    else if ((i + j) % 2 != 0 && i >= numOfCoinsLines + 2 && i < m_SizeOfBoard)
                    {
                        m_CheckersGameBoard[i, j] = Enums.eCoinsOnBoard.Player1;
                    }
                    else
                    {
                        m_CheckersGameBoard[i, j] = Enums.eCoinsOnBoard.Blank;
                    }
                }
            }
        }

        public bool CheckIfKingOfPlayer1(Move.Point i_CurrentPointOfFigure)
        {
            bool isKingPlayer1 = false;

            if (m_CheckersGameBoard[i_CurrentPointOfFigure.Row, i_CurrentPointOfFigure.Col] == Enums.eCoinsOnBoard.KingPlayer1)
            {
                isKingPlayer1 = true;
            }

            return isKingPlayer1;
        }

        public bool CheckIfKingOfPlayer2(Move.Point i_CurrentPointOfFigure)
        {
            bool isKingPlayer2 = false;

            if (m_CheckersGameBoard[i_CurrentPointOfFigure.Row, i_CurrentPointOfFigure.Col] == Enums.eCoinsOnBoard.KingPlayer2)
            {
                isKingPlayer2 = true;
            }

            return isKingPlayer2;
        }

        public bool CheckIfTransferToKing(Move i_CurrentMove, Player o_CheckedPlayer)
        {
            bool isKing = false;

            if (m_CheckersGameBoard[i_CurrentMove.StartPoint.Row, i_CurrentMove.StartPoint.Col] == Enums.eCoinsOnBoard.Player1)
            {
                if (i_CurrentMove.EndPoint.Row == 0)
                {
                    isKing = true;
                    o_CheckedPlayer.AddKingToCounterOfKingsPlayer();
                }
            }
            else if (m_CheckersGameBoard[i_CurrentMove.StartPoint.Row, i_CurrentMove.StartPoint.Col] == Enums.eCoinsOnBoard.Player2)
            {
                if (i_CurrentMove.EndPoint.Row == m_SizeOfBoard - 1)
                {
                    isKing = true;
                    o_CheckedPlayer.AddKingToCounterOfKingsPlayer();
                }
            }

            return isKing;
        }

        public void UpdateKingOnBoard(Move i_CurrMove, Player i_CheckedPlayer)
        {
            if (CheckIfTransferToKing(i_CurrMove, i_CheckedPlayer))
            {
                if (i_CheckedPlayer.Figure == Enums.eCoinsOnBoard.Player1)
                {
                    AddToGameBoard(i_CurrMove, Enums.eCoinsOnBoard.KingPlayer1);
                }
                else
                {
                    AddToGameBoard(i_CurrMove, Enums.eCoinsOnBoard.KingPlayer2);
                }
            }
        }
    }
}