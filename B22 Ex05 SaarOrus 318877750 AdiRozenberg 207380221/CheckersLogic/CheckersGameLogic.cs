using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class CheckersGameLogic
    {
        private Player m_Player1;
        private Player m_Player2;
        private GameBoard m_gameBoard;

        public Player Player1
        {
            get { return m_Player1; }
            set { m_Player1 = value; }
        }

        public Player Player2
        {
            get { return m_Player2; }
            set { m_Player2 = value; }
        }

        public GameBoard GameBoardCheckers
        {
            get { return m_gameBoard; }
            set { m_gameBoard = value; }
        }

        public CheckersGameLogic(int i_SizeOfBoard, Player i_Player1, Player i_Player2)
        {
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            m_gameBoard = new GameBoard(i_SizeOfBoard);
        }

        public bool              CheckVictoryPlayer1()
        {
            bool isVictory = false;

            if ((m_Player2.CounterOfRegularPlayers == 0 && m_Player2.CounterOfKingPlayers == 0) ||
                !m_Player2.CheckIfLegalMovesLeft(m_gameBoard.SizeOfBoard, m_gameBoard))
            {
                isVictory = true;
                m_Player1.IsWinner = true;
            }

            return isVictory;
        }

        public bool              CheckVictoryPlayer2()
        {
            bool isVictory = false;

            if ((m_Player1.CounterOfRegularPlayers == 0 && m_Player1.CounterOfKingPlayers == 0) ||
                 !m_Player1.CheckIfLegalMovesLeft(m_gameBoard.SizeOfBoard, m_gameBoard))
            {
                isVictory = true;
                m_Player2.IsWinner = true;
            }

            return isVictory;
        }

        public bool              CheckIfTie()
        {
            bool isTie = false;

            if (!m_Player1.CheckIfLegalMovesLeft(m_gameBoard.SizeOfBoard, m_gameBoard) && (!m_Player2.CheckIfLegalMovesLeft(m_gameBoard.SizeOfBoard, m_gameBoard)))
            {
                m_Player1.IsWinner = m_Player2.IsWinner = true;
                isTie = true;
            }

            return isTie;
        }

        public bool              CheckIfGameOver()
        {
            bool isGameOver = false;

            if (CheckVictoryPlayer1() || CheckVictoryPlayer2() || CheckIfTie())
            {
                isGameOver = true;
            }

            m_Player1.ResetArraysOfMovesBeforeNextTurn();
            m_Player2.ResetArraysOfMovesBeforeNextTurn();

            return isGameOver;
        }

        public int               CalculateSumPointsOfWinner()
        {
            int returnSumOfPoints = 0;

            if (m_Player1.IsWinner)
            {
                returnSumOfPoints = m_Player1.CalculateAndUpdateWinnersFinalScore(m_Player2);
            }
            else
            {
                returnSumOfPoints = m_Player2.CalculateAndUpdateWinnersFinalScore(m_Player1);
            }

            return returnSumOfPoints;
        }

        public Player            CheckHowGameOverAndReturnWinner()
        {
            Player winnerPlayer = null;

            if (m_Player1.IsWinner && !m_Player2.IsWinner)
            {
                winnerPlayer = m_Player1;
            }
            else if (!m_Player1.IsWinner && m_Player2.IsWinner)
            {
                winnerPlayer = m_Player2;
            }

            return winnerPlayer;
        }

        public void              SwitchTurnsOfPlayers()
        {
            if (Player1.IsPlayerTurn)
            {
                Player1.IsPlayerTurn = false;
                Player2.IsPlayerTurn = true;
            }
            else if (Player2.IsPlayerTurn)
            {
                Player1.IsPlayerTurn = true;
                Player2.IsPlayerTurn = false;
            }
        }

        public void              CheckAndUpdateAnotherSkipMoveForComputer(Player i_UpdatedPlayer, Move io_CurrentMove)
        {
            if (i_UpdatedPlayer.IsExcistAnotherSkippingComputer(GameBoardCheckers))
            {
                Random randomSkipFromSkipMoveArray = new Random();
                io_CurrentMove = Player2.SkipMoves[randomSkipFromSkipMoveArray.Next(Player2.SkipMoves.Count)];
                GameBoardCheckers.UpdateMoveOnGameBoard(io_CurrentMove, Player2, Player1);
            }
        }

        public Move              GetAndCheckRandomMovesForComputerPlayer()
        {
            Move currentMove = new Move();

            Player2.CheckIfLegalMovesLeft(GameBoardCheckers.SizeOfBoard, GameBoardCheckers);
            Random randomMoveFromRegularMovesArray = new Random();
            if (Player2.SkipMoves.Count > 0)
            {
                currentMove = Player2.SkipMoves[randomMoveFromRegularMovesArray.Next(Player2.SkipMoves.Count)];
            }
            else
            {
                currentMove = Player2.RegularMoves[randomMoveFromRegularMovesArray.Next(Player2.RegularMoves.Count)];
            }

            Player2.ResetArraysOfMovesBeforeNextTurn();

            return currentMove;
        }

        public Enums.eMoveStatus GetAndCheckMoveFromBuildArraysOfPlayer1(Move i_CurrentMove)
        {
            Enums.eMoveStatus currentStatusMove;

            Player1.ResetArraysOfMovesBeforeNextTurn();
            Player1.BuildArraysOfUpMoves(i_CurrentMove.StartPoint, GameBoardCheckers.SizeOfBoard, GameBoardCheckers);
            if (GameBoardCheckers.CheckIfKingOfPlayer1(i_CurrentMove.StartPoint))
            {
               Player1.BuildArraysOfDownMoves(i_CurrentMove.StartPoint, GameBoardCheckers.SizeOfBoard, GameBoardCheckers);
            }

            currentStatusMove = i_CurrentMove.IsLegalEndPoint(Player1, GameBoardCheckers, Player1.RegularMoves, Player1.SkipMoves);

            return currentStatusMove;
        }

        public Enums.eMoveStatus GetAndCheckMoveFromBuildArraysOfPlayer2(Move i_CurrentMove)
        {
            Enums.eMoveStatus currentStatusMove;

            Player2.ResetArraysOfMovesBeforeNextTurn();
            Player2.BuildArraysOfDownMoves(i_CurrentMove.StartPoint, GameBoardCheckers.SizeOfBoard, GameBoardCheckers);
            if (GameBoardCheckers.CheckIfKingOfPlayer2(i_CurrentMove.StartPoint))
            {
                Player2.BuildArraysOfUpMoves(i_CurrentMove.StartPoint, GameBoardCheckers.SizeOfBoard, GameBoardCheckers);
            }

            currentStatusMove = i_CurrentMove.IsLegalEndPoint(Player2, GameBoardCheckers, Player2.RegularMoves, Player2.SkipMoves);

            return currentStatusMove;
        }
    }
}
