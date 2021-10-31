using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using Work.Forms;

namespace Work
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var form = new LoginForm())
                if (form.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit(new CancelEventArgs(true));
                    return;
                }
            Application.Run(new MainForm());
        }
    }
}
