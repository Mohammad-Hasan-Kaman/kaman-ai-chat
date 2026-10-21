using System;
using System.Windows.Forms;

namespace chatai
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // First run: ask for the API key so it never has to live in the source
            // code or in the shipped executable.
            if (!Settings.HasApiKey())
            {
                using (var dialog = new ApiKeyForm())
                {
                    if (dialog.ShowDialog() != DialogResult.OK) return;

                    if (dialog.Remember)
                    {
                        Settings.SaveApiKey(dialog.ApiKey);
                    }
                }
            }

            Application.Run(new Form1());
        }
    }
}
