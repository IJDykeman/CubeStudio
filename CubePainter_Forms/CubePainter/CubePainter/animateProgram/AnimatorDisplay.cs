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
using CubeAnimator;
using CubePainter;
using System.IO;

namespace CubeStudio
{
    public class AnimatorDisplay : StudioGraphicsDeviceControl
    {
        AnimatorMain game;
        

        


        protected override void Initialize()
        {
            
            game = new AnimatorMain();
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
                //Console.WriteLine("in loop yo");
                GraphicsDevice.Clear(new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
                Console.WriteLine(Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space));
            }*/
        }



        protected override void Draw()
        {
            game.resize(Width, Height);
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
        protected override void OnResize(EventArgs e)
        {
           // if(game != null)
                //game.resize(Width, Height);
        }

        private void Application_Idle(object pSender, EventArgs pEventArgs)
        {
            //Draw();
            
            //Console.WriteLine("idle");

        }

        public void OnClick(object sender, EventArgs e)
        {
           // MessageBox.Show("HAi");
        }

        public override void newEvent()
        {
            game.newEvent_main();
        }

        public override void openEvent(string path)
        {
            if (new FileInfo(path).Extension.Equals(".vox"))
            {
                BodyPartType type = MainWindow.proptUserForBodySelection();
                if (type == CubeAnimator.BodyPartType.unknown)
                {
                    return;
                }
                game.addModelOfBodyPartType_main(path, type);
            }
            else if (new FileInfo(path).Extension.Equals(".chr"))
            {
                openCharacter(path);
            }
        }

        public void openCharacter(string path)
        {
            game.openCharacter(path);
        }

        public void addModelOfBodyPartType_form(string path, CubeAnimator.BodyPartType type)
        {
            game.addModelOfBodyPartType_main(path, type);
        }

        public override void saveEvent(string path)
        {
            game.convertToCharacterEvent_main(path);
        }

        public override void saveAsEvent()
        {
        }

        public void removePart(Object bodyPart)
        {
            game.removePart((BodyPart)bodyPart);
        }

        public void replacePart(Object bodyPart,string path)
        {
            game.removePart((BodyPart)bodyPart, path);
        }

        public void changePartType(Object bodyPart, BodyPartType type)
        {
            game.changePartType((BodyPart)bodyPart,  type);
        }

        public void selectPartWithCharacterTreeClick(object tag)
        {
            BodyPart part = tag as BodyPart;
            if (part != null)
            {
                game.selectPartWithCharacterTreeClick(part);
            }
        }



    }
}