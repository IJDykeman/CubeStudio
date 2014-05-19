using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


namespace CubeStudio
{
    public abstract class StudioGraphicsDeviceControl : WinFormsGraphicsDevice.GraphicsDeviceControl
    {
        public abstract void newEvent();
        public abstract void openEvent(string path);
        public abstract void saveAsEvent();
        public abstract void saveEvent(string path);
        public MainWindow mainWindow;
        protected TimeSpan FrameInterval = TimeSpan.FromMilliseconds(1000.0 / 60.5);
        protected DateTime PrevFrameTime = DateTime.Now;
        protected ContentManager content;

        protected Stopwatch timer;

    }
}
