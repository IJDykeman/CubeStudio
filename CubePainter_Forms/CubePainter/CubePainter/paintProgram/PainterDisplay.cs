using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace CubePainter
{
    public class PainterDisplay : CubeStudio.StudioGraphicsDeviceControl
    {
        PainterMain game;





        protected override void Initialize()
        {
            
            game = new PainterMain();
            game.resize(Width, Height);
            content = new ContentManager(Services, "Content");
            game.start(GraphicsDevice, content);
            //Application.Idle += new EventHandler(Application_Idle);
            //GraphicsDevice = game.GraphicsDevice;
            //currently the window gets no draw input at all
            //must bind the devices properly

               // Draw();

            //drawing = new Thread(new ThreadStart(() => displayLoop()));
            //drawing.Start();
            timer = Stopwatch.StartNew();
            Application.Idle += delegate { Invalidate(); };
            this.Click += new System.EventHandler(OnClick);
            
        }

        void displayLoop()
        {
           /* Random rand = new Random();
            while (true)
            {

                game.drawPub();
                GraphicsDevice.Present();
                GraphicsDevice.Clear(new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
            }*/
        }


        protected override void Draw()
        {

            game.setScreenLocation(mainWindow.Location.X + Location.X, mainWindow.Location.Y + Location.Y);
            game.drawPub();

            DateTime CurrentFrameTime = DateTime.Now;

                TimeSpan rest = FrameInterval - (CurrentFrameTime - PrevFrameTime);
                if (rest > new TimeSpan())
                {
                    Thread.Sleep(rest);
                }

            TimeSpan diff = DateTime.Now - PrevFrameTime;
            PrevFrameTime = CurrentFrameTime;
            PrevFrameTime = DateTime.Now;

        }


        private void Application_Idle(object pSender, EventArgs pEventArgs)
        {
            //Draw();
            

        }


        public void OnClick(object sender, EventArgs e)
        {
           // MessageBox.Show("HAi");
        }

        public override void newEvent()
        {
            game.addAction(new PlayerNewDocumentAction());

        }

        public override void openEvent(string path)
        {
            game.addAction(new PlayerOpenDocumentAction(path));
        }

        public override void saveEvent(string path)
        {
            game.addAction(new PlayerSaveDocumentAction(path));
        }

        public override void saveAsEvent()
        {
           // game.addAction(new PlayerSaveDocumentAction());
        }
    }
}