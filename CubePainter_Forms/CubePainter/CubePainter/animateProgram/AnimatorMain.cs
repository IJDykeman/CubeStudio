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

namespace CubeAnimator
{
    /// <summary>
    /// This is the main type for your animationProgram
    /// </summary>
    public class AnimatorMain : Microsoft.Xna.Framework.Game
    {
        AnimationProgram animationProgram;
        private GraphicsDeviceManager graphics;
        private static AnimatorMain main;
        public static bool mouseIsVisible = true;
        public static Vector2 mouseLocationInXNASpace;

        public AnimatorMain()
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
            //Console.WriteLine(Microsoft.VisualBasic.Interaction.InputBox("Please select a file name to use", "Save As", "file name", 300, 300));

            Window.Title = "Cube Animator";
            
            

            base.Initialize();
        }

        protected void init(GraphicsDevice device)
        {
            mouseLocationInXNASpace = new Vector2();
            animationProgram = new AnimationProgram(device);

        }

        protected override void LoadContent()
        {
            animationProgram.loadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = mouseIsVisible;
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                this.Exit();
            animationProgram.update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            animationProgram.display();
            
            base.Draw(gameTime);
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
            AnimationProgram.width = nwidth;
            AnimationProgram.height = nheight;

        }

        public void setScreenLocation(int x, int y)
        {

            mouseLocationInXNASpace = new Vector2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            mouseLocationInXNASpace -= new Vector2(x + 10, y + 51);

        }

        public void start(GraphicsDevice device, ContentManager content)
        {
            Initialize();
            init(device);
            Content = content;
            LoadContent();
        }

        public void addModelOfBodyPartType_main(string path, CubeAnimator.BodyPartType type)
        {
            
             animationProgram.addModelOfBodyPartType(path, type);
        }

        public void convertToCharacterEvent_main(string path)
        {
            animationProgram.saveCharacterEvent(path);
        }

        public void openCharacter(string path)
        {
            animationProgram.openCharacter(path);
        }

        public void newEvent_main()
        {
            animationProgram.newEvent();
        }

        public void removePart(BodyPart part)
        {
            animationProgram.removePart(part);
        }

        public void removePart(BodyPart toReplace, string nPath)
        {
            animationProgram.replacePart(toReplace, nPath);
        }

        public void changePartType(BodyPart toReplace, BodyPartType type)
        {
            animationProgram.changePartType(toReplace, type);
        }

        public void selectPartWithCharacterTreeClick(BodyPart part)
        {
            animationProgram.selectPartWithCharacterTreeClick(part);
        }

    }


}
