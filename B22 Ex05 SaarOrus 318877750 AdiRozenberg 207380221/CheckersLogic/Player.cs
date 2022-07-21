using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Player
    {
        private readonly Enums.ePlayerType m_PlayerType;
        private string m_NameOfPlayer;
        private int m_CounterOfRegularPlayers;
        private int m_CounterOfKingPlayers;
        private List<Move> m_RegularMoves;
        private List<Move> m_SkipMoves;
        private Enums.eCoinsOnBoard m_Figure;
        private bool m_IsWinner = false;
        private int m_Score;
        private bool m_IsPlayerTurn;
        private Move m_LastTurnOfPlayer;

        public int CounterOfRegularPlayers
        {
            get { return m_CounterOfRegularPlayers; }
            set { m_CounterOfRegularPlayers = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public int CounterOfKingPlayers
        {
            get { return m_CounterOfKingPlayers; }
            set { m_CounterOfKingPlayers = value; }
        }

        public List<Move> RegularMoves
        {
            get { return m_RegularMoves; }
            set { m_RegularMoves = value; }
        }

        public List<Move> SkipMoves
        {
            get { return m_SkipMoves; }
            set { m_SkipMoves = value; }
        }

        public Enums.ePlayerType TypeOfPlayer
        {
            get { return m_PlayerType; }
        }

        public string NameOfPlayer
        {
            get { return m_NameOfPlayer; }
            set { m_NameOfPlayer = value; }
        }

        public Enums.eCoinsOnBoard Figure
        {
            get { return m_Figure; }
            set { m_Figure = value; }
        } // Player1 is always 'O', Player2 is always 'X'

        public bool IsWinner
        {
            get { return m_IsWinner; }
            set { m_IsWinner = value; }
        }

        public bool IsPlayerTurn
        {
            get { return m_IsPlayerTurn; }
            set { m_IsPlayerTurn = value; }
        }

        public Move LastTurnOfPlayer
        {
            get { return m_LastTurnOfPlayer; }
            set { m_LastTurnOfPlayer = value; }
        }

        ////ctor
        public Player(string i_NameOfPlayer, Enums.eCoinsOnBoard i_Figure, bool i_IsPlayerTurn, Enums.ePlayerType i_PlayerType, int i_CounterOfRegularPlayers)
        {
            m_RegularMoves = new List<Move>();
            m_SkipMoves = new List<Move>();
            m_NameOfPlayer = i_NameOfPlayer;
            m_Figure = i_Figure;
            m_IsWinner = false;
            m_Score = 0;
            m_IsPlayerTurn = i_IsPlayerTurn;
            m_PlayerType = i_PlayerType;
            m_CounterOfRegularPlayers = i_CounterOfRegularPlayers;
            m_CounterOfKingPlayers = 0;
        }

        public bool       IsComputer
        {
            get { return m_PlayerType == Enums.ePlayerType.Computer; }
        }

        public void       InitialPlayer(int i_SizeOfBoard, bool i_IsPlayerTurn)
        {
            m_CounterOfRegularPlayers = (i_SizeOfBoard / 2) * ((i_SizeOfBoard / 2) - 1);
            m_CounterOfKingPlayers = 0;
            ResetArraysOfMovesBeforeNextTurn();
            m_IsWinner = false;
            m_LastTurnOfPlayer = null;
            m_IsPlayerTurn = i_IsPlayerTurn;
            m_Score = 0;
        }

        public void       BuildArraysOfDownMoves(Move.Point i_FromMove, int i_SizeOfBoard, GameBoard i_CheckersGameBoard)
        {
            if (CheckIfEnteredMoveContainOtherPlayer(i_FromMove, i_CheckersGameBoard))
            {
                ResetArraysOfMovesBeforeNextTurn();
            }
            else
            {
                ////build regular moves array for player2
                CheckAndInsertDownRegularMoves(i_FromMove, i_SizeOfBoard, i_CheckersGameBoard);
                if (i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.KingPlayer1)
                {
                    CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard.Player2, Enums.eCoinsOnBoard.KingPlayer2, i_FromMove.Row, i_FromMove.Col, i_FromMove, i_SizeOfBoard, i_CheckersGameBoard, m_SkipMoves);
                }
                else
                {
                    CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard.Player1, Enums.eCoinsOnBoard.KingPlayer1, i_FromMove.Row, i_FromMove.Col, i_FromMove, i_SizeOfBoard, i_CheckersGameBoard, m_SkipMoves);
                }
            }
        }

        public bool       CheckIfEnteredMoveContainOtherPlayer(Move.Point i_FromMove, GameBoard i_CheckersGameBoard)
        {
            bool player1 = false, player2 = false;
            bool isContainOtherPlayer = false;

            if (m_Figure == Enums.eCoinsOnBoard.Player1)
            {
                player1 = true;
            }
            else
            {
                player2 = true;
            }

            if ((player1 && i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.Player2) || (player2 && i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.Player1)
                || (i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.Blank) || (player1 && i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.KingPlayer2)
                || (player2 && i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.KingPlayer1))
            {
                isContainOtherPlayer = true;
            }

            return isContainOtherPlayer;
        }

        public void       CheckAndInsertUpRegularMoves(Move.Point i_FromMove, int i_SizeOfBoard, GameBoard i_CheckersGameBoard)
        {
            int fromMoveCol = i_FromMove.Col;
            int fromMoveRow = i_FromMove.Row;

            ////left up regular move
            if (!IsCellOutOfBoundries(fromMoveCol - 1, fromMoveRow - 1, i_SizeOfBoard) &&
                IsCellBlank(fromMoveCol - 1, fromMoveRow - 1, i_CheckersGameBoard))
            {
                Move.Point currentLegalPoint = new Move.Point();

                currentLegalPoint.Col = fromMoveCol - 1;
                currentLegalPoint.Row = fromMoveRow - 1;
                Move currentLegalMove = new Move(i_FromMove, currentLegalPoint, false);
                m_RegularMoves.Add(currentLegalMove);
            }

            ////right up regular move
            if (!IsCellOutOfBoundries(fromMoveCol + 1, fromMoveRow - 1, i_SizeOfBoard) &&
                IsCellBlank(fromMoveCol + 1, fromMoveRow - 1, i_CheckersGameBoard))
            {
                Move.Point currentLegalPoint = new Move.Point();

                currentLegalPoint.Col = fromMoveCol + 1;
                currentLegalPoint.Row = fromMoveRow - 1;
                Move currentLegalMove = new Move(i_FromMove, currentLegalPoint, false);
                m_RegularMoves.Add(currentLegalMove);
            }
        }

        public void       CheckAndInsertDownRegularMoves(Move.Point i_FromMove, int i_SizeOfBoard, GameBoard i_CheckersGameBoard)
        {
            int fromMoveCol = i_FromMove.Col;
            int fromMoveRow = i_FromMove.Row;

            ////left down regular move
            if (!IsCellOutOfBoundries(fromMoveCol - 1, fromMoveRow + 1, i_SizeOfBoard) &&
            IsCellBlank(fromMoveCol - 1, fromMoveRow + 1, i_CheckersGameBoard))
            {
                Move.Point currentLegalPoint = new Move.Point();

                currentLegalPoint.Col = fromMoveCol - 1;
                currentLegalPoint.Row = fromMoveRow + 1;
                Move currentLegalMove = new Move(i_FromMove, currentLegalPoint, false);
                m_RegularMoves.Add(currentLegalMove);
            }

            ////right down regular move
            if (!IsCellOutOfBoundries(fromMoveCol + 1, fromMoveRow + 1, i_SizeOfBoard) &&
                IsCellBlank(fromMoveCol + 1, fromMoveRow + 1, i_CheckersGameBoard))
            {
                Move.Point currentLegalPoint = new Move.Point();

                currentLegalPoint.Col = fromMoveCol + 1;
                currentLegalPoint.Row = fromMoveRow + 1;
                Move currentLegalMove = new Move(i_FromMove, currentLegalPoint, false);
                m_RegularMoves.Add(currentLegalMove);
            }
        }

        public void       BuildArraysOfUpMoves(Move.Point i_FromMove, int i_SizeOfBoard, GameBoard i_CheckersGameBoard)
        {
            if (CheckIfEnteredMoveContainOtherPlayer(i_FromMove, i_CheckersGameBoard))
            {
                ResetArraysOfMovesBeforeNextTurn();
            }
            else
            {
                ////build regular moves array for player1
                CheckAndInsertUpRegularMoves(i_FromMove, i_SizeOfBoard, i_CheckersGameBoard);
                if (i_CheckersGameBoard.CheckersGameBoard[i_FromMove.Row, i_FromMove.Col] == Enums.eCoinsOnBoard.KingPlayer2)
                {
                    CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard.Player1, Enums.eCoinsOnBoard.KingPlayer1, i_FromMove.Row, i_FromMove.Col, i_FromMove, i_SizeOfBoard, i_CheckersGameBoard, m_SkipMoves);
                }
                else
                {
                    CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard.Player2, Enums.eCoinsOnBoard.KingPlayer2, i_FromMove.Row, i_FromMove.Col, i_FromMove, i_SizeOfBoard, i_CheckersGameBoard, m_SkipMoves);
                }
            }
        }

        public void       ResetArraysOfMovesBeforeNextTurn()
        {
            m_SkipMoves.Clear();
            m_RegularMoves.Clear();
        }

        public bool       IsCellContainsRival(Enums.eCoinsOnBoard i_Rival, Enums.eCoinsOnBoard i_KingsRival, int i_EndMoveCol, int i_EndMoveRow, GameBoard i_CheckersGameBoard)
        {
            bool containsRivalOfPlayer = false;

            if (i_CheckersGameBoard.CheckersGameBoard[i_EndMoveRow, i_EndMoveCol] == i_Rival || i_CheckersGameBoard.CheckersGameBoard[i_EndMoveRow, i_EndMoveCol] == i_KingsRival)
            {
                containsRivalOfPlayer = true;
            }

            return containsRivalOfPlayer;
        }

        public bool       IsCellOutOfBoundries(int i_EndMoveCol, int i_EndMoveRow, int i_SizeOfBoard)
        {
            bool isOutOfBounds = true;

            if ((i_EndMoveCol >= 0 && i_EndMoveCol <= i_SizeOfBoard - 1) &&
                (i_EndMoveRow >= 0 && i_EndMoveRow <= i_SizeOfBoard - 1))
            {
                isOutOfBounds = false;
            }

            return isOutOfBounds;
        }

        public bool       IsCellBlank(int i_EndMoveCol, int i_EndMoveRow, GameBoard i_CheckersGameBoard)
        {
            bool cellIsBlank = false;

            if (i_CheckersGameBoard.CheckersGameBoard[i_EndMoveRow, i_EndMoveCol] == Enums.eCoinsOnBoard.Blank)
            {
                cellIsBlank = true;
            }

            return cellIsBlank;
        }

        public bool       CheckIfLegalMovesLeft(int i_BoardSize, GameBoard i_CheckersGameBoard)
        {
            bool isLeftLegalMoves = false;
            bool player1 = false, player2 = false;

            if (m_Figure == Enums.eCoinsOnBoard.Player1)
            {
                player1 = true;
            }
            else
            {
                player2 = true;
            }

            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    if (i_CheckersGameBoard.CheckersGameBoard[i, j] != Enums.eCoinsOnBoard.Blank)
                    {
                        Move.Point currentStartPoint = new Move.Point();

                        currentStartPoint.Row = i;
                        currentStartPoint.Col = j;
                        if (player1)
                        {
                            if (i_CheckersGameBoard.CheckersGameBoard[i, j] == m_Figure)
                            {
                                BuildArraysOfUpMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                            }
                            else if (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.KingPlayer1)
                            {
                                BuildArraysOfUpMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                                BuildArraysOfDownMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                            }

                            if (m_RegularMoves.Count != 0 || m_SkipMoves.Count != 0)
                            {
                                isLeftLegalMoves = true;
                            }
                        }

                        if (player2)
                        {
                            if (i_CheckersGameBoard.CheckersGameBoard[i, j] == m_Figure)
                            {
                                BuildArraysOfDownMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                            }
                            else if (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.KingPlayer2)
                            {
                                BuildArraysOfDownMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                                BuildArraysOfUpMoves(currentStartPoint, i_BoardSize, i_CheckersGameBoard);
                            }

                            if (m_RegularMoves.Count != 0 || m_SkipMoves.Count != 0)
                            {
                                isLeftLegalMoves = true;
                            }
                        }
                    }
                }
            }

            return isLeftLegalMoves;
        }

        public List<Move> CalculatePossibleSkippingMovesInATurn(int i_BoardSize, GameBoard i_CheckersGameBoard)
        {
            List<Move> PossibleSkippingMoves = new List<Move>();
            Move currentMove = new Move();
            Move.Point currPointOnBoard = new Move.Point();
            bool player1 = false, player2 = false;

            PossibleSkippingMoves.Clear();
            if (m_Figure == Enums.eCoinsOnBoard.Player1)
            {
                player1 = true;
            }
            else
            {
                player2 = true;
            }

            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    if (i_CheckersGameBoard.CheckersGameBoard[i, j] != Enums.eCoinsOnBoard.Blank)
                    {
                        currPointOnBoard.Col = j;
                        currPointOnBoard.Row = i;
                        currentMove.StartPoint = currPointOnBoard;
                        if (player1 && (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.Player1))
                        {
                            CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard.Player2, Enums.eCoinsOnBoard.KingPlayer2, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                        }
                        else if (player2 && (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.Player2))
                        {
                            CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard.Player1, Enums.eCoinsOnBoard.KingPlayer1, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                        }
                        else if (player1 && (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.KingPlayer1))
                        {
                            CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard.Player2, Enums.eCoinsOnBoard.KingPlayer2, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                            CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard.Player2, Enums.eCoinsOnBoard.KingPlayer2, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                        }
                        else if (player2 && (i_CheckersGameBoard.CheckersGameBoard[i, j] == Enums.eCoinsOnBoard.KingPlayer2))
                        {
                            CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard.Player1, Enums.eCoinsOnBoard.KingPlayer1, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                            CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard.Player1, Enums.eCoinsOnBoard.KingPlayer1, i, j, currentMove.StartPoint, i_BoardSize, i_CheckersGameBoard, PossibleSkippingMoves);
                        }
                    }
                }
            }

            return PossibleSkippingMoves;
        }

        public void       CheckAndInsertUpSkipMoves(Enums.eCoinsOnBoard i_RivalFigure, Enums.eCoinsOnBoard i_KingsRival, int i_CurrentRow, int i_CurrentCol, Move.Point i_CurrentPoint, int i_BoardSize, GameBoard i_CheckersGameBoard, List<Move> o_PossibleSkippingMoves)
        {   // left up skip move
            if (!IsCellOutOfBoundries(i_CurrentPoint.Col - 2, i_CurrentPoint.Row - 2, i_BoardSize) &&
            IsCellContainsRival(i_RivalFigure, i_KingsRival, i_CurrentPoint.Col - 1, i_CurrentPoint.Row - 1, i_CheckersGameBoard) &&
            IsCellBlank(i_CurrentPoint.Col - 2, i_CurrentPoint.Row - 2, i_CheckersGameBoard))
            {
                Move insertedMove = new Move();
                Move.Point insertedPoint1 = new Move.Point();
                Move.Point insertedPoint2 = new Move.Point();

                insertedPoint1.Col = i_CurrentCol;
                insertedPoint1.Row = i_CurrentRow;
                insertedMove.StartPoint = insertedPoint1;
                insertedPoint2.Col = i_CurrentCol - 2;
                insertedPoint2.Row = i_CurrentRow - 2;
                insertedMove.EndPoint = insertedPoint2;
                insertedMove.IsSkipMove = true;
                o_PossibleSkippingMoves.Add(insertedMove);
            }

            // right up skip move
            if (!IsCellOutOfBoundries(i_CurrentPoint.Col + 2, i_CurrentPoint.Row - 2, i_BoardSize) &&
            IsCellContainsRival(i_RivalFigure, i_KingsRival, i_CurrentPoint.Col + 1, i_CurrentPoint.Row - 1, i_CheckersGameBoard) &&
            IsCellBlank(i_CurrentPoint.Col + 2, i_CurrentPoint.Row - 2, i_CheckersGameBoard))
            {
                Move insertedMove = new Move();
                Move.Point insertedPoint1 = new Move.Point();
                Move.Point insertedPoint2 = new Move.Point();

                insertedPoint1.Col = i_CurrentCol;
                insertedPoint1.Row = i_CurrentRow;
                insertedMove.StartPoint = insertedPoint1;
                insertedPoint2.Col = i_CurrentCol + 2;
                insertedPoint2.Row = i_CurrentRow - 2;
                insertedMove.EndPoint = insertedPoint2;
                insertedMove.IsSkipMove = true;
                o_PossibleSkippingMoves.Add(insertedMove);
            }
        }

        public void       CheckAndInsertDownSkipMoves(Enums.eCoinsOnBoard i_RivalFigure, Enums.eCoinsOnBoard i_KingsRival, int i_CurrentRow, int i_CurrentCol, Move.Point i_CurrentPoint, int i_BoardSize, GameBoard i_CheckersGameBoard, List<Move> o_PossibleSkippingMoves)
        {
            // left down skip move
            if (!IsCellOutOfBoundries(i_CurrentPoint.Col - 2, i_CurrentPoint.Row + 2, i_BoardSize) &&
            IsCellContainsRival(i_RivalFigure, i_KingsRival, i_CurrentPoint.Col - 1, i_CurrentPoint.Row + 1, i_CheckersGameBoard) &&
            IsCellBlank(i_CurrentPoint.Col - 2, i_CurrentPoint.Row + 2, i_CheckersGameBoard))
            {
                Move insertedMove = new Move();
                Move.Point insertedPoint1 = new Move.Point();
                Move.Point insertedPoint2 = new Move.Point();

                insertedPoint1.Col = i_CurrentCol;
                insertedPoint1.Row = i_CurrentRow;
                insertedMove.StartPoint = insertedPoint1;
                insertedPoint2.Col = i_CurrentCol - 2;
                insertedPoint2.Row = i_CurrentRow + 2;
                insertedMove.EndPoint = insertedPoint2;
                insertedMove.IsSkipMove = true;
                o_PossibleSkippingMoves.Add(insertedMove);
            }

            // right down skip move
            if (!IsCellOutOfBoundries(i_CurrentPoint.Col + 2, i_CurrentPoint.Row + 2, i_BoardSize) &&
            IsCellContainsRival(i_RivalFigure, i_KingsRival, i_CurrentPoint.Col + 1, i_CurrentPoint.Row + 1, i_CheckersGameBoard) &&
            IsCellBlank(i_CurrentPoint.Col + 2, i_CurrentPoint.Row + 2, i_CheckersGameBoard))
            {
                Move insertedMove = new Move();
                Move.Point insertedPoint1 = new Move.Point();
                Move.Point insertedPoint2 = new Move.Point();

                insertedPoint1.Col = i_CurrentCol;
                insertedPoint1.Row = i_CurrentRow;
                insertedMove.StartPoint = insertedPoint1;
                insertedPoint2.Col = i_CurrentCol + 2;
                insertedPoint2.Row = i_CurrentRow + 2;
                insertedMove.EndPoint = insertedPoint2;
                insertedMove.IsSkipMove = true;
                o_PossibleSkippingMoves.Add(insertedMove);
            }
        }

        public bool       IsExcistAnotherSkippingHumans(GameBoard i_CheckersBoard)
        {
            bool anotherSkipping = false;

            if (m_LastTurnOfPlayer.IsSkipMove)
            {
                if (i_CheckersBoard.CheckersGameBoard[m_LastTurnOfPlayer.EndPoint.Row, m_LastTurnOfPlayer.EndPoint.Col] == Enums.eCoinsOnBoard.KingPlayer1)
                {
                    BuildArraysOfUpMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    BuildArraysOfDownMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    if (m_SkipMoves.Count != 0)
                    {
                        anotherSkipping = true;
                    }
                }
                else if (i_CheckersBoard.CheckersGameBoard[m_LastTurnOfPlayer.EndPoint.Row, m_LastTurnOfPlayer.EndPoint.Col] == Enums.eCoinsOnBoard.KingPlayer2)
                {
                    BuildArraysOfUpMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    BuildArraysOfDownMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    if (m_SkipMoves.Count != 0)
                    {
                        anotherSkipping = true;
                    }
                }
                else if (m_Figure == Enums.eCoinsOnBoard.Player1)
                {
                    BuildArraysOfUpMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    if (m_SkipMoves.Count != 0)
                    {
                        anotherSkipping = true;
                    }
                }
                else
                {
                    BuildArraysOfDownMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                    if (m_SkipMoves.Count != 0)
                    {
                        anotherSkipping = true;
                    }
                }
            }

            return anotherSkipping;
        }

        public bool       IsExcistAnotherSkippingComputer(GameBoard i_CheckersBoard)
        {
            bool anotherSkipping = false;

            if (m_LastTurnOfPlayer.IsSkipMove == true)
            {
                BuildArraysOfDownMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                if (i_CheckersBoard.CheckersGameBoard[m_LastTurnOfPlayer.EndPoint.Row, m_LastTurnOfPlayer.EndPoint.Col] == Enums.eCoinsOnBoard.KingPlayer2)
                {
                    BuildArraysOfUpMoves(m_LastTurnOfPlayer.EndPoint, i_CheckersBoard.SizeOfBoard, i_CheckersBoard);
                }

                if (m_SkipMoves.Count != 0)
                {
                    anotherSkipping = true;
                }
            }

            return anotherSkipping;
        }

        public int        CalculateAndUpdateWinnersFinalScore(Player i_LooserPlayer)
        {
            int CalcsumOfPoints = 0, returnSumOfPoints = 0;

            CalcsumOfPoints = m_CounterOfRegularPlayers + (m_CounterOfKingPlayers * 4) - (i_LooserPlayer.CounterOfRegularPlayers + (i_LooserPlayer.CounterOfKingPlayers * 4));
            m_Score += CalcsumOfPoints;
            returnSumOfPoints = m_Score;

            return returnSumOfPoints;
        }

        public void       AddKingToCounterOfKingsPlayer()
        {
            m_CounterOfRegularPlayers--;
            m_CounterOfKingPlayers++;
        }
    }
}