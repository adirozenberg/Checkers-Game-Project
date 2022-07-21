using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Move
    {
        public struct Point
        {
            private int m_Col;
            private int m_Row;

            public int Col
            {
                get { return m_Col; }
                set { m_Col = value; }
            }

            public int Row
            {
                get { return m_Row; }
                set { m_Row = value; }
            }
        }

        private Point m_StartPoint;
        private Point m_EndPoint;
        private bool m_IsSkipMove;

        public Point StartPoint
        {
            get { return m_StartPoint; }
            set { m_StartPoint = value; }
        }

        public Point EndPoint
        {
            get { return m_EndPoint; }
            set { m_EndPoint = value; }
        }

        public bool IsSkipMove
        {
            get { return m_IsSkipMove; }
            set { m_IsSkipMove = value; }
        }

        public Move() 
        { 
        }

        public Move(Point i_StartPoint, Point i_EndPoint, bool i_IsSkipMove)
        {
            m_StartPoint = i_StartPoint;
            m_EndPoint = i_EndPoint;
            m_IsSkipMove = i_IsSkipMove;
        }

        public Enums.eMoveStatus IsLegalEndPoint(Player i_CurrentPlayer, GameBoard i_CheckersGameBoard, List<Move> i_RegularMoves, List<Move> i_SkipMoves)
        {
            bool legal = false;
            Enums.eMoveStatus currentMoveStatus = Enums.eMoveStatus.ValidMove;
            List<Move> PossibleSkippingMoves = i_CurrentPlayer.CalculatePossibleSkippingMovesInATurn(i_CheckersGameBoard.SizeOfBoard, i_CheckersGameBoard);
            
            if (PossibleSkippingMoves.Count != 0)
            {
                foreach (Move currentMove in PossibleSkippingMoves)
                {
                    if (CheckIfEqualMoves(currentMove))
                    {
                        m_IsSkipMove = true;
                        legal = true;
                    }
                }

                if(!legal)
                {
                    currentMoveStatus = Enums.eMoveStatus.MustToDoSkipMove;
                }
            }
            else
            {
                if (i_SkipMoves.Count == 0)
                {
                    foreach (Move currentMove in i_RegularMoves)
                    {
                        if (CheckIfEqualMoves(currentMove))
                        {
                            m_IsSkipMove = false;
                            legal = true;
                        }
                    }

                    if (!legal)
                    {
                        currentMoveStatus = Enums.eMoveStatus.InvalidMove;
                    }
                }
            }
            
            PossibleSkippingMoves.Clear();

            return currentMoveStatus;
        }

        public bool              CheckIfEqualMoves(Move i_ComparedMove)
        {
            bool isEqual = false;

            if (i_ComparedMove.StartPoint.Row == m_StartPoint.Row && i_ComparedMove.StartPoint.Col == m_StartPoint.Col &&
                        i_ComparedMove.EndPoint.Row == m_EndPoint.Row && i_ComparedMove.EndPoint.Col == m_EndPoint.Col)
            {
                isEqual = true;
            }

            return isEqual;
        }

        public void              UpdateCurrentMove(int i_RowOfStartMoveButton, int i_ColOfStartMoveButton, int i_RowOfEndMoveButton, int i_ColOfEndMoveButton)
        {
            Move.Point newStartPoint = new Move.Point();
            Move.Point newEndPoint = new Move.Point();

            newStartPoint.Row = i_RowOfStartMoveButton;
            newStartPoint.Col = i_ColOfStartMoveButton;
            m_StartPoint = newStartPoint;            
            newEndPoint.Row = i_RowOfEndMoveButton;
            newEndPoint.Col = i_ColOfEndMoveButton;
            m_EndPoint = newEndPoint;
        }
    }
}