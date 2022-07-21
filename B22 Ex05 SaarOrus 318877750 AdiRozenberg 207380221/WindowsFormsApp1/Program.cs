using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Checkers
{
    public class Program
    {
        [STAThread]

        public static void Main()
        {
            FormGameSettings formGameSettings = new FormGameSettings();
            if (formGameSettings.ShowDialog() == DialogResult.OK)
            {
                new DamkaBoardForm(formGameSettings).ShowDialog();
            }
        }
    }
}
