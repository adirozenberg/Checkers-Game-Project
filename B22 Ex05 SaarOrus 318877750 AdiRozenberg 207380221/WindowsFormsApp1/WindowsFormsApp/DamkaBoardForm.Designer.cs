using System.Drawing;

namespace Checkers
{
    public partial class DamkaBoardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /*private void InitializeComponent()
        {
            this.labelNamePlayer1 = new System.Windows.Forms.Label();
            this.labelNamePlayer2 = new System.Windows.Forms.Label();
            this.labelScorePlayer1 = new System.Windows.Forms.Label();
            this.labelScorePlayer2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNamePlayer1
            // 
            this.labelNamePlayer1.AutoSize = true;
            this.labelNamePlayer1.Location = new System.Drawing.Point(120, 26);
            this.labelNamePlayer1.Name = "labelNamePlayer1";
            this.labelNamePlayer1.Size = new System.Drawing.Size(0, 17);
            this.labelNamePlayer1.TabIndex = 0;
            this.labelNamePlayer1.Click += new System.EventHandler(this.labelPlayerName1_Click);
            // 
            // labelNamePlayer2
            // 
            this.labelNamePlayer2.AutoSize = true;
            this.labelNamePlayer2.Location = new System.Drawing.Point(320, 26);
            this.labelNamePlayer2.Name = "labelNamePlayer2";
            this.labelNamePlayer2.Size = new System.Drawing.Size(0, 17);
            this.labelNamePlayer2.TabIndex = 1;
            this.labelNamePlayer2.Click += new System.EventHandler(this.labelPlayerName2_Click);
            // 
            // labelScorePlayer1
            // 
            this.labelScorePlayer1.AutoSize = true;
            this.labelScorePlayer1.Location = new System.Drawing.Point(0, 0);
            this.labelScorePlayer1.Name = "labelScorePlayer1";
            this.labelScorePlayer1.Size = new System.Drawing.Size(46, 17);
            this.labelScorePlayer1.TabIndex = 2;
            this.labelScorePlayer1.Text = "label1";
            this.labelScorePlayer1.Click += new System.EventHandler(this.labelScorePlayer1_Click);
            // 
            // labelScorePlayer2
            // 
            this.labelScorePlayer2.AutoSize = true;
            this.labelScorePlayer2.Location = new System.Drawing.Point(0, 0);
            this.labelScorePlayer2.Name = "labelScorePlayer2";
            this.labelScorePlayer2.Size = new System.Drawing.Size(46, 17);
            this.labelScorePlayer2.TabIndex = 3;
            this.labelScorePlayer2.Text = "label2";
            this.labelScorePlayer2.Click += new System.EventHandler(this.labelScorePlayer2_Click);
            // 
            // DamkaBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(586, 554);
            this.Controls.Add(this.labelScorePlayer2);
            this.Controls.Add(this.labelScorePlayer1);
            this.Controls.Add(this.labelNamePlayer2);
            this.Controls.Add(this.labelNamePlayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DamkaBoardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DamkaBoard";
            this.Load += new System.EventHandler(this.DamkaBoardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitialButtonsOnBoard()
        {
            bool isGray;

            isGray = true;
            for (int i = 0; i < m_FormGameSettings.ChosenBoardSize; i++)
            {
                for (int j = 0; j < m_FormGameSettings.ChosenBoardSize; j++)
                {
                    m_ButtonsGameBoard[i, j] = new System.Windows.Forms.Button();
                    m_ButtonsGameBoard[i, j].Size = new Size(30, 30);
                    if (isGray)
                    {
                        m_ButtonsGameBoard[i, j].BackColor = Color.Gray;
                        m_ButtonsGameBoard[i, j].Enabled = false;
                        isGray = false;
                    }
                    else
                    {
                        m_ButtonsGameBoard[i, j].BackColor = Color.White;
                        isGray = true;
                    }
                }
                isGray = !isGray;
            }
        }

        private void BuildCheckersBoard()
        {
            int numOfCoinsLines = (m_FormGameSettings.ChosenBoardSize / 2) - 1;
            int locationI, locationJ, state;

            labelNamePlayer1.Text = m_FormGameSettings.NameOfPlayer1 + " :";
            labelNamePlayer2.Text = m_FormGameSettings.NameOfPlayer2 + " :";
            labelScorePlayer1.Location = new System.Drawing.Point(labelNamePlayer1.Size.Width + labelNamePlayer1.Left + 5, labelNamePlayer1.Top);
            labelScorePlayer2.Location = new System.Drawing.Point(labelNamePlayer2.Size.Width + labelNamePlayer2.Left + 5, labelNamePlayer2.Top);
            if (m_FormGameSettings.ChosenBoardSize == 6)
                state=locationI = locationJ = 120;
            else if (m_FormGameSettings.ChosenBoardSize == 8)
                state=locationI = locationJ = 100;
            else
                state=locationI = locationJ = 80;
            InitialButtonsOnBoard();
            for (int i = 0; i < m_FormGameSettings.ChosenBoardSize; i++)
            {
                for (int j = 0; j < m_FormGameSettings.ChosenBoardSize; j++)
                {
                    if ((i + j) % 2 != 0 && i < numOfCoinsLines)
                    {           
                        m_ButtonsGameBoard[i, j].Text = "O";
                        m_ButtonsGameBoard[i, j].Location = new Point(locationI,locationJ); 
                        
                    }
                    else if ((i + j) % 2 == 0 && i > numOfCoinsLines && i < numOfCoinsLines + 2)
                    { 
                        m_ButtonsGameBoard[i, j].Location = new Point(locationI, locationJ);
                    }
                    else if ((i + j) % 2 != 0 && i >= numOfCoinsLines + 2 && i < m_FormGameSettings.ChosenBoardSize)
                    {
                        m_ButtonsGameBoard[i, j].Text = "X";
                        m_ButtonsGameBoard[i, j].Location = new Point(locationI, locationJ);
                    }
                    else
                    {
                        m_ButtonsGameBoard[i, j].Location = new Point(locationI, locationJ);
                    }

                    this.Controls.Add(m_ButtonsGameBoard[i, j]);
                    
                    locationI += 30;
                }

                locationI = state;
                locationJ += 30;
            }
        }

        private void PlayPlayer1Turn()
        {
            ColorButtonClicked();
        }

        private void PlayPlayer2Turn()
        {
            ColorButtonClicked();
        }

        private void ColorButtonClicked()
        {
            this.BackColor = Color.LightBlue;
        }

        private void ReturnColorButtonClicked()
        {
            this.BackColor = Color.White;
        }
*/
        #endregion

        /*private System.Windows.Forms.Label labelNamePlayer1;
        private System.Windows.Forms.Label labelNamePlayer2;
        private System.Windows.Forms.Label labelScorePlayer1;
        private System.Windows.Forms.Label labelScorePlayer2;*/
    }
}