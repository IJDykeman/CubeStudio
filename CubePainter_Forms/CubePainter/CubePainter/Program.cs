using System;
using System.Windows.Forms;
using CubeStudio;
using System.Threading;
/*
known bugs-
 * When using the "Save" button after opening a model, it saves to the wring file
 * fill tool BFS is broken -- much too slow!

 * */

namespace CubePainter
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);




            using (MainWindow window = new MainWindow())
            {

                Application.Run(window);

            }
        }

    }


}

