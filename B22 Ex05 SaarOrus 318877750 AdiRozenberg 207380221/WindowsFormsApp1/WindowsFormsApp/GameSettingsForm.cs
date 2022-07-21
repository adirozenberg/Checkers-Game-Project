using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;

namespace Checkers
{
    public partial class FormGameSettings : Form
    {
        private int m_ChosenBoardSize;
        private string m_NameOfPlayer1;
        private string m_NameOfPlayer2 = "Computer";
        private Enums.ePlayerType m_RivalType = Enums.ePlayerType.Computer;
        private Label labelBoardSize = new Label();
        private Label labelPlayers = new Label();
        private Label labelPlayer1 = new Label();
        private RadioButton radioButton6x6 = new RadioButton();
        private RadioButton radioButton8x8 = new RadioButton();
        private RadioButton radioButton10x10 = new RadioButton();
        private TextBox textBoxPlayer1 = new TextBox();
        private TextBox textBoxPlayer2 = new TextBox();
        private CheckBox checkBoxPlayer2 = new CheckBox();
        private Button buttonDone = new Button();

        public int ChosenBoardSize
        {
            get { return m_ChosenBoardSize; }
        }

        public string NameOfPlayer1
        {
            get { return m_NameOfPlayer1; }
        }

        public string NameOfPlayer2
        {
            get { return m_NameOfPlayer2; }
        }

        public Enums.ePlayerType RivalType
        {
            get { return m_RivalType; }
            set { m_RivalType = value;  }
        }

        public FormGameSettings()
        {
            initializeComponent();
        }

        private void initializeComponent()
        {
            this.SuspendLayout();
            //// labelBoardSize
            labelBoardSize.AutoSize = true;
            labelBoardSize.Location = new Point(36, 22);
            labelBoardSize.Name = "labelBoardSize";
            labelBoardSize.Size = new Size(81, 17);
            labelBoardSize.Text = "Board Size:";
            //// radioButton6x6
            radioButton6x6.AutoSize = true;
            radioButton6x6.Location = new Point(60, 57);
            radioButton6x6.Name = "radioButton6x6";
            radioButton6x6.Size = new Size(59, 21);
            radioButton6x6.TabStop = true;
            radioButton6x6.TabIndex = 1;
            radioButton6x6.Text = "6 x 6";
            radioButton6x6.UseVisualStyleBackColor = true;
            radioButton6x6.CheckedChanged += new EventHandler(this.radioButton6x6_CheckedChanged);
            //// radioButton8x8
            radioButton8x8.AutoSize = true;
            radioButton8x8.Location = new Point(159, 57);
            radioButton8x8.Name = "radioButton8x8";
            radioButton8x8.Size = new Size(59, 21);
            radioButton8x8.TabStop = true;
            radioButton8x8.Text = "8 x 8";
            radioButton8x8.UseVisualStyleBackColor = true;
            radioButton8x8.CheckedChanged += new EventHandler(this.radioButton8x8_CheckedChanged);
            //// radioButton10x10
            radioButton10x10.AutoSize = true;
            radioButton10x10.Location = new Point(252, 57);
            radioButton10x10.Name = "radioButton10x10";
            radioButton10x10.Size = new Size(75, 21);
            radioButton10x10.TabStop = true;
            radioButton10x10.Text = "10 x 10";
            radioButton10x10.UseVisualStyleBackColor = true;
            radioButton10x10.CheckedChanged += new EventHandler(this.radioButton10x10_CheckedChanged);
            //// labelPlayers
            labelPlayers.AutoSize = true;
            labelPlayers.Location = new Point(36, 104);
            labelPlayers.Name = "labelPlayers";
            labelPlayers.Size = new Size(59, 17);
            labelPlayers.Text = "Players:";
            //// textBoxPlayer1
            textBoxPlayer1.Location = new Point(152, 142);
            textBoxPlayer1.Name = "textBoxPlayer1";
            textBoxPlayer1.Size = new Size(100, 22);
            textBoxPlayer1.TextChanged += new EventHandler(this.textBoxPlayer1_TextChanged);
            //// labelPlayer1
            labelPlayer1.AutoSize = true;
            labelPlayer1.Location = new Point(55, 145);
            labelPlayer1.Name = "labelPlayer1";
            labelPlayer1.Size = new Size(64, 17);
            labelPlayer1.Text = "Player 1:";
            //// checkBoxPlayer2
            checkBoxPlayer2.AutoSize = true;
            checkBoxPlayer2.Location = new Point(60, 189);
            checkBoxPlayer2.Name = "checkBoxPlayer2";
            checkBoxPlayer2.Size = new Size(86, 21);
            checkBoxPlayer2.Text = "Player 2:";
            checkBoxPlayer2.UseVisualStyleBackColor = true;
            checkBoxPlayer2.CheckedChanged += new EventHandler(this.checkBoxPlayer2_CheckedChanged);
            //// buttonDone
            buttonDone.Location = new Point(287, 247);
            buttonDone.Name = "buttonDone";
            buttonDone.Size = new Size(75, 23);
            buttonDone.Text = "Done";
            buttonDone.UseVisualStyleBackColor = true;
            buttonDone.Click += new EventHandler(this.buttonDone_Click);
            buttonDone.Enabled = false;
            //// textBoxPlayer2
            textBoxPlayer2.Enabled = false;
            textBoxPlayer2.Location = new Point(153, 189);
            textBoxPlayer2.Name = "textBoxPlayer2";
            textBoxPlayer2.Size = new Size(100, 22);
            textBoxPlayer2.Text = "[Computer]";
            textBoxPlayer2.TextChanged += new EventHandler(this.textBoxPlayer2_TextChanged);
            //// FormGameSettings
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(397, 291);
            Controls.Add(this.textBoxPlayer2);
            Controls.Add(this.buttonDone);
            Controls.Add(this.checkBoxPlayer2);
            Controls.Add(this.labelPlayer1);
            Controls.Add(this.textBoxPlayer1);
            Controls.Add(this.labelPlayers);
            Controls.Add(this.radioButton10x10);
            Controls.Add(this.radioButton8x8);
            Controls.Add(this.radioButton6x6);
            Controls.Add(this.labelBoardSize);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "FormGameSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPlayer2.Enabled = true;
            m_RivalType = Enums.ePlayerType.Human;
        }

        private void textBoxPlayer1_TextChanged(object sender, EventArgs e)
        {
            buttonDone.Enabled = true;
            m_NameOfPlayer1 = textBoxPlayer1.Text;            
        }

        private void textBoxPlayer2_TextChanged(object sender, EventArgs e)
        {
            m_NameOfPlayer2 = textBoxPlayer2.Text;
        }

        private void radioButton6x6_CheckedChanged(object sender, EventArgs e)
        {
            m_ChosenBoardSize = 6;
        }

        private void radioButton8x8_CheckedChanged(object sender, EventArgs e)
        {
            m_ChosenBoardSize = 8;
        }

        private void radioButton10x10_CheckedChanged(object sender, EventArgs e)
        {
            m_ChosenBoardSize = 10;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
