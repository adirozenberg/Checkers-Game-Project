using System;
using System.Windows.Forms;
using System.Drawing;
using CheckersLogic;

namespace Checkers
{
    public partial class DamkaBoardForm : Form
    {
        private const int k_SizeOfButton = 40;
        private FormGameSettings m_FormGameSettings;
        private CheckersGameLogic m_CheckersLogic;
        private Label labelNamePlayer1 = new Label();
        private Label labelNamePlayer2 = new Label();
        private Button[,] buttonsGameBoard;
        private Button startMoveButton;
        private Button endMoveButton;
        private int m_ButtonsBoardSize;
        private bool m_EndOfMove = false;
        private bool m_IsGameRunning = true;

        public DamkaBoardForm(FormGameSettings i_FormGameSettings)
        {
            m_FormGameSettings = i_FormGameSettings;
            Player Player1 = new Player(m_FormGameSettings.NameOfPlayer1, Enums.eCoinsOnBoard.Player1, true, Enums.ePlayerType.Human, (m_FormGameSettings.ChosenBoardSize / 2) * ((m_FormGameSettings.ChosenBoardSize / 2) - 1));
            Player Player2 = new Player(m_FormGameSettings.NameOfPlayer2, Enums.eCoinsOnBoard.Player2, false, m_FormGameSettings.RivalType, (m_FormGameSettings.ChosenBoardSize / 2) * ((m_FormGameSettings.ChosenBoardSize / 2) - 1));
            m_ButtonsBoardSize = m_FormGameSettings.ChosenBoardSize;
            m_CheckersLogic = new CheckersGameLogic(m_ButtonsBoardSize, Player1, Player2);
            buttonsGameBoard = new Button[m_ButtonsBoardSize, m_ButtonsBoardSize];
            initializeComponent();
            initialButtonsOnBoard();
            buildCheckersBoard();
        }

        private void  initializeComponent()
        {
            this.SuspendLayout();
            //// labelNamePlayer1
            labelNamePlayer1.AutoSize = true;
            labelNamePlayer1.Text = m_CheckersLogic.Player1.NameOfPlayer + ": " + m_CheckersLogic.Player1.Score.ToString();
            labelNamePlayer1.Location = new Point(30 + k_SizeOfButton, 20);
            labelNamePlayer1.Name = "labelNamePlayer1";
            labelNamePlayer1.Size = new Size(100, 20);
            //// labelNamePlayer2
            labelNamePlayer2.AutoSize = true;
            labelNamePlayer2.Text = m_CheckersLogic.Player2.NameOfPlayer + ": " + m_CheckersLogic.Player2.Score.ToString();
            labelNamePlayer2.Location = new Point(30 + ((m_ButtonsBoardSize - 2) * k_SizeOfButton), labelNamePlayer1.Height);
            labelNamePlayer2.Name = "labelNamePlayer2";
            labelNamePlayer2.Size = new Size(100, 20);
            //// DamkaBoardForm
            Size = new Size((m_ButtonsBoardSize * k_SizeOfButton) + 100, (m_ButtonsBoardSize * k_SizeOfButton) + 100);
            AutoSize = true;
            Controls.Add(this.labelNamePlayer2);
            Controls.Add(this.labelNamePlayer1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "DamkaBoardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DamkaBoard";
            MaximizeBox = false;
            ResumeLayout(false);
            PerformLayout();
        }

        private void  initialButtonsOnBoard()
        {
            int startPositionRow, startPositionCol, currentPositionsRow, currentPostionCol;
            bool isUnaccessibleCell = true;

            startPositionRow = 40;
            startPositionCol = labelNamePlayer1.Height + 20;
            for (int i = 0; i < m_FormGameSettings.ChosenBoardSize; i++)
            {
                for (int j = 0; j < m_FormGameSettings.ChosenBoardSize; j++)
                {
                    buttonsGameBoard[i, j] = new Button();
                    buttonsGameBoard[i, j].Size = new Size(k_SizeOfButton, k_SizeOfButton);
                    currentPositionsRow = startPositionRow + (i * k_SizeOfButton);
                    currentPostionCol = startPositionCol + (j * k_SizeOfButton);
                    buttonsGameBoard[i, j].Location = new Point(currentPositionsRow, currentPostionCol);
                    if (isUnaccessibleCell)
                    {
                        buttonsGameBoard[i, j].BackColor = Color.SaddleBrown;
                        buttonsGameBoard[i, j].Enabled = false;
                        isUnaccessibleCell = false;
                    }
                    else
                    {
                        buttonsGameBoard[i, j].BackColor = Color.Moccasin;
                        isUnaccessibleCell = true;
                    }

                    this.Controls.Add(buttonsGameBoard[i, j]);
                    buttonsGameBoard[i, j].Click += new EventHandler(buttonOnBoard_Click);
                }

                isUnaccessibleCell = !isUnaccessibleCell;
            }
        }

        private void  buildCheckersBoard()
        {
            for (int i = 0; i < m_ButtonsBoardSize; i++)
            {
                for (int j = 0; j < m_ButtonsBoardSize; j++)
                {
                    buttonsGameBoard[j, i].BackgroundImage = convertFiguresOnBoardToImage(m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[i, j]);
                    buttonsGameBoard[j, i].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        private Image convertFiguresOnBoardToImage(Enums.eCoinsOnBoard i_FigureOnBoard)
        {
            Image cellImage = null;

            switch (i_FigureOnBoard)
            {
                case Enums.eCoinsOnBoard.Player1:
                    cellImage = Properties.Resources.player1;
                    break;
                case Enums.eCoinsOnBoard.KingPlayer1:
                    cellImage = Properties.Resources.player1king;
                    break;
                case Enums.eCoinsOnBoard.Player2:
                    cellImage = Properties.Resources.player2;
                    break;
                case Enums.eCoinsOnBoard.KingPlayer2:
                    cellImage = Properties.Resources.player2king;
                    break;
                default:
                    break;
            }

            return cellImage;
        }

        private void  buttonOnBoard_Click(object sender, EventArgs e)
        {
            if (!m_EndOfMove)
            {
                startMoveButton = sender as Button;
                checkValidationOfStartMove(startMoveButton);
            }
            else
            {
                endMoveButton = sender as Button;
                checkValidationOfEndMove(startMoveButton, endMoveButton);
                if (!m_CheckersLogic.Player1.IsPlayerTurn && m_CheckersLogic.Player2.TypeOfPlayer == Enums.ePlayerType.Computer && m_IsGameRunning)
                {
                    computerTurn();
                }
            }
        }

        private void  checkValidationOfStartMove(Button i_StartMoveButton)
        {
            bool isValidSrartMoveButton = false;
            int rowOfStartMoveButton = (startMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;
            int colOfStartMoveButton = (startMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;

            if (m_CheckersLogic.Player1.IsPlayerTurn)
            {
                if (m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[rowOfStartMoveButton, colOfStartMoveButton] == Enums.eCoinsOnBoard.Player1 ||
                    m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[rowOfStartMoveButton, colOfStartMoveButton] == Enums.eCoinsOnBoard.KingPlayer1)
                {
                    isValidSrartMoveButton = true;
                }
            }
            else
            {
                if (m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[rowOfStartMoveButton, colOfStartMoveButton] == Enums.eCoinsOnBoard.Player2 ||
                    m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[rowOfStartMoveButton, colOfStartMoveButton] == Enums.eCoinsOnBoard.KingPlayer2)
                {
                    isValidSrartMoveButton = true;
                }
            }

            if (isValidSrartMoveButton)
            {
                i_StartMoveButton.BackColor = Color.Blue;
                m_EndOfMove = true;
            }
        }

        private void  checkValidationOfEndMove(Button i_StartMoveButton, Button i_EndMoveButton)
        {
            Enums.eMoveStatus currentStatusMove;
            int rowOfStartMoveButton = (startMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;
            int colOfStartMoveButton = (startMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int colOfEndMoveButton = (endMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int rowOfEndMoveButton = (endMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;

            if (!i_StartMoveButton.Equals(i_EndMoveButton))
            {
                Move currentMove = new Move();
                currentMove.UpdateCurrentMove(rowOfStartMoveButton, colOfStartMoveButton, rowOfEndMoveButton, colOfEndMoveButton);
                if (m_CheckersLogic.Player1.IsPlayerTurn)
                {
                    currentStatusMove = m_CheckersLogic.GetAndCheckMoveFromBuildArraysOfPlayer1(currentMove);
                }
                else
                {
                    currentStatusMove = m_CheckersLogic.GetAndCheckMoveFromBuildArraysOfPlayer2(currentMove);
                }

                showErrorMessageOrPlayTurn(currentStatusMove, currentMove);
            }

            i_StartMoveButton.BackColor = Color.Moccasin;
            m_EndOfMove = false;
        }

        private void  showErrorMessageOrPlayTurn(Enums.eMoveStatus i_CurrentStatusMove, Move i_CurrentMove)
        {
            switch (i_CurrentStatusMove)
            {
                case Enums.eMoveStatus.ValidMove:
                    playersTurns(i_CurrentMove);
                    break;
                case Enums.eMoveStatus.InvalidMove:
                    showErrorMessage("Invalid move.");
                    break;
                case Enums.eMoveStatus.MustToDoSkipMove:
                    showErrorMessage("Must do skip move.");
                    break;
            }
        }

        private void  playersTurns(Move i_CurrentMove)
        {
            if (m_CheckersLogic.Player1.IsPlayerTurn)
            {
                player1Turn(i_CurrentMove);
            }
            else
            {
                player2Turn(i_CurrentMove);
            }
        }

        private void  updatePlayersLabelScore()
        {
            labelNamePlayer1.Text = m_CheckersLogic.Player1.NameOfPlayer + ": " + m_CheckersLogic.Player1.Score;
            labelNamePlayer2.Text = m_CheckersLogic.Player2.NameOfPlayer + ": " + m_CheckersLogic.Player2.Score;
        }

        private void  computerTurn()
        {
            Move currentMove = new Move();

            currentMove = m_CheckersLogic.GetAndCheckRandomMovesForComputerPlayer();
            m_CheckersLogic.GameBoardCheckers.UpdateMoveOnGameBoard(currentMove, m_CheckersLogic.Player2, m_CheckersLogic.Player1);
            startMoveButton = buttonsGameBoard[currentMove.StartPoint.Col, currentMove.StartPoint.Row];
            startMoveButton.BackgroundImage = buttonsGameBoard[currentMove.StartPoint.Col, currentMove.StartPoint.Row].BackgroundImage;
            endMoveButton = buttonsGameBoard[currentMove.EndPoint.Col, currentMove.EndPoint.Row];
            updateButtonsBoard(currentMove, endMoveButton, startMoveButton);
            m_CheckersLogic.Player2.ResetArraysOfMovesBeforeNextTurn();
            m_CheckersLogic.CheckAndUpdateAnotherSkipMoveForComputer(m_CheckersLogic.Player2, currentMove);
            updatePlayersLabelScore();
            if (m_CheckersLogic.CheckIfGameOver())
            {
                m_IsGameRunning = false;
                showEndOfGameMassage();
            }
            else
            {
                m_CheckersLogic.SwitchTurnsOfPlayers();
            }
        }

        private void  showEndOfGameMassage()
        {
            DialogResult anotherRoundUserChoise;

            if (m_CheckersLogic.Player1.IsWinner && m_CheckersLogic.Player2.IsWinner)
            {
                anotherRoundUserChoise = MessageBox.Show("Tie! \nAnother Round?", "Damka", MessageBoxButtons.YesNo);
            }
            else if (m_CheckersLogic.Player1.IsWinner)
            {
                anotherRoundUserChoise = MessageBox.Show("Player 1 Won! \nAnother Round?", "Damka", MessageBoxButtons.YesNo);
            }
            else
            {
                anotherRoundUserChoise = MessageBox.Show("Player 2 Won! \nAnother Round?", "Damka", MessageBoxButtons.YesNo);
            }

            switch (anotherRoundUserChoise)
            {
                case DialogResult.Yes:
                    resetRoundOfGame();
                    break;
                case DialogResult.No:
                    this.Close();
                    break;
            }
        }

        private void  player1Turn(Move i_CurrentMove)
        {
            m_CheckersLogic.GameBoardCheckers.UpdateMoveOnGameBoard(i_CurrentMove, m_CheckersLogic.Player1, m_CheckersLogic.Player2);
            startMoveButton = buttonsGameBoard[i_CurrentMove.StartPoint.Col, i_CurrentMove.StartPoint.Row];
            startMoveButton.BackgroundImage = buttonsGameBoard[i_CurrentMove.StartPoint.Col, i_CurrentMove.StartPoint.Row].BackgroundImage;
            endMoveButton = buttonsGameBoard[i_CurrentMove.EndPoint.Col, i_CurrentMove.EndPoint.Row];
            updateButtonsBoard(i_CurrentMove, endMoveButton, startMoveButton);
            m_CheckersLogic.Player1.ResetArraysOfMovesBeforeNextTurn();
            checkAndUpdateAnotherSkipMoveForHumans(m_CheckersLogic.Player1);
        }

        private void  player2Turn(Move i_CurrentMove)
        {
            m_CheckersLogic.GameBoardCheckers.UpdateMoveOnGameBoard(i_CurrentMove, m_CheckersLogic.Player2, m_CheckersLogic.Player1);
            startMoveButton = buttonsGameBoard[i_CurrentMove.StartPoint.Col, i_CurrentMove.StartPoint.Row];
            startMoveButton.BackgroundImage = buttonsGameBoard[i_CurrentMove.StartPoint.Col, i_CurrentMove.StartPoint.Row].BackgroundImage;
            endMoveButton = buttonsGameBoard[i_CurrentMove.EndPoint.Col, i_CurrentMove.EndPoint.Row];
            updateButtonsBoard(i_CurrentMove, endMoveButton, startMoveButton);
            m_CheckersLogic.Player2.ResetArraysOfMovesBeforeNextTurn();
            checkAndUpdateAnotherSkipMoveForHumans(m_CheckersLogic.Player2);
        }

        private void  checkAndUpdateAnotherSkipMoveForHumans(Player i_UpdatedPlayer)
        {
            if (!i_UpdatedPlayer.IsExcistAnotherSkippingHumans(m_CheckersLogic.GameBoardCheckers))
            {
                updatePlayersLabelScore();
                if (m_CheckersLogic.CheckIfGameOver())
                {
                    m_IsGameRunning = false;
                    showEndOfGameMassage();
                }
                else
                {
                    m_CheckersLogic.SwitchTurnsOfPlayers();
                }
            }
        }

        private void  updateButtonsBoard(Move i_CurrentMove, Button i_EndMoveButton, Button i_StartMoveButton)
        {
            addToButtonsBoard(i_EndMoveButton, i_StartMoveButton);
            deleteFromButtonsBoard(i_CurrentMove, i_StartMoveButton, i_EndMoveButton);
        }

        private void  addToButtonsBoard(Button i_EndMoveButton, Button i_StartMoveButton)
        {
            int colOfEndMoveButton = (i_EndMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int rowOfEndMoveButton = (i_EndMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;
            Enums.eCoinsOnBoard figureOnBoard = m_CheckersLogic.GameBoardCheckers.CheckersGameBoard[rowOfEndMoveButton, colOfEndMoveButton];
            Image imageFigure = convertFiguresOnBoardToImage(figureOnBoard);

            buttonsGameBoard[colOfEndMoveButton, rowOfEndMoveButton].BackgroundImage = imageFigure;
        }

        private void  deleteFromButtonsBoard(Move i_CurrentMove, Button i_StartMoveButton, Button i_EndMoveButton)
        {
            int colOfStartMoveButton = (i_StartMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int rowOfStartMoveButton = (i_StartMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;

            if (i_CurrentMove.IsSkipMove)
            {
                deleteEatenPlayerInButtonsBoard(i_StartMoveButton, i_EndMoveButton);
            }

            buttonsGameBoard[colOfStartMoveButton, rowOfStartMoveButton].BackgroundImage = null;
        }

        private void  deleteEatenPlayerInButtonsBoard(Button i_StartMoveButton, Button i_EndMoveButton)
        {
            Move.Point eatenPlayerPoint = new Move.Point();
            int colOfStartMoveButton = (i_StartMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int rowOfStartMoveButton = (i_StartMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;
            int colOfEndMoveButton = (i_EndMoveButton.Left - buttonsGameBoard[0, 0].Location.X) / k_SizeOfButton;
            int rowOfEndMoveButton = (i_EndMoveButton.Location.Y - buttonsGameBoard[0, 0].Location.Y) / k_SizeOfButton;

            eatenPlayerPoint.Row = (rowOfStartMoveButton + rowOfEndMoveButton) / 2;
            eatenPlayerPoint.Col = (colOfStartMoveButton + colOfEndMoveButton) / 2;
            buttonsGameBoard[eatenPlayerPoint.Col, eatenPlayerPoint.Row].BackgroundImage = null;
        }

        private void  resetRoundOfGame()
        {
            m_CheckersLogic.GameBoardCheckers.InitializeGameBoard();
            m_CheckersLogic.Player1.InitialPlayer(m_CheckersLogic.GameBoardCheckers.SizeOfBoard, true);
            m_CheckersLogic.Player2.InitialPlayer(m_CheckersLogic.GameBoardCheckers.SizeOfBoard, false);
            buildCheckersBoard();
            updatePlayersLabelScore();
            m_IsGameRunning = true;
        }

        private void  showErrorMessage(string i_Message)
        {
            MessageBox.Show(i_Message, "Oops!", MessageBoxButtons.OK);
        }
    }
}
