using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.VisualBasic;
using System.Threading;

namespace CubePainter
{
    /// <summary>
    /// This is the main type for your animationProgram
    /// </summary>
    public class PainterMain : Microsoft.Xna.Framework.Game
    {
        PaintProgram paintProgram;
        private GraphicsDeviceManager graphics;
        private static PainterMain main;
        public static bool mouseIsVisible = true;
        public static Vector2 mouseLocationInXNASpace;

        public PainterMain()
        {

            main = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public static void quitGame()
        {
            main.Exit();
        }

        protected override void Initialize()
        {
            Mouse.WindowHandle = Window.Handle;
            Window.Title = "Cube Painter";



            base.Initialize();
        }

        protected void init(GraphicsDevice device)
        {

            paintProgram = new PaintProgram(device);

        }

        protected override void LoadContent()
        {

            paintProgram.loadContent(Content);

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = mouseIsVisible;
            IsMouseVisible = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                this.Exit();
            paintProgram.update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            paintProgram.display();

            base.Draw(gameTime);
        }

        public void start(GraphicsDevice device, ContentManager content)
        {
            Initialize();
            init(device);
            Content = content;
            LoadContent();
        }


        public void drawPub()
        {
            
            Random rand = new Random();
            Update(new GameTime(TimeSpan.FromSeconds(1f), TimeSpan.FromSeconds(1f)));
            Draw(new GameTime(TimeSpan.FromSeconds(1f), TimeSpan.FromSeconds(1f)));
            //Compositer.device.Clear(new Color(rand.Next(255), rand.Next(255), rand.Next(255)));
        }

        public void resize(int nwidth, int nheight)
        {
            PaintProgram.width = nwidth;
            PaintProgram.height = nheight;
        }

        public void setScreenLocation(int x, int y)
        {

            mouseLocationInXNASpace = new Vector2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            mouseLocationInXNASpace -= new Vector2(x + 10, y + 51);

        }

        public void addAction(Action toAdd)
        {
            paintProgram.addUIAction(toAdd);
            
        }
    }
}
