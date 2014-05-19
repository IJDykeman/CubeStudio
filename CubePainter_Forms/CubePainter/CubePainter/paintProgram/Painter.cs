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
using CubeStudio;

namespace CubePainter
{
    public class Painter : CubeStudio.Player
    {
        public Vector3 loc;
        public Matrix viewMatrix;
        public bool displayGuideLines = false;

        public float leftrightRot = -2.328f;//0;//MathHelper.PiOver2;
        public float updownRot = -.179f;//-MathHelper.Pi / 10.0f;
        KeyboardState oldKeyboardState;
        public MouseState originalMouseState, oldMouseState;
        public Vector2 oldMouseLocation;


        const float rotationSpeed = 0.3f;
        const float moveSpeed = 20;
        public float mouseSensitivity = .009f;

        //float dragRotationSpeed=.01f;

        public byte selectedBlock = 15;

        Texture2D crossReticle;
        Texture2D colorsImage;
        Texture2D colorSwatchhighlight;
        Texture2D colorPickerIcon;
        Texture2D bucketIcon;
        Texture2D splash;

        SpriteFont UIfont;
        SpriteFont smallPrint;

        public Vector3? rectBuildLoc1 = null;

        enum PlayerState
        {
            //splash,
            working,
            paused
        }

        PlayerState state = PlayerState.working;

        bool colorPickerVisible;
        bool mirror = false;

        readonly int colorPickerBlockWidth = 28;
        readonly int colorPickerBlockHeight = 21;

        public Painter()
        {
            loc = new Vector3(-17, 4, -16);


            oldMouseState = Mouse.GetState();
            oldKeyboardState = Keyboard.GetState();
            oldMouseLocation = new Vector2();

        }

        public void loadContent(ContentManager content)
        {
            crossReticle = content.Load<Texture2D>("crossReticle");
            colorsImage = content.Load<Texture2D>("colorsImage");
            colorSwatchhighlight = content.Load<Texture2D>("ColorSwatchHighlightBox");
            colorPickerIcon = content.Load<Texture2D>("colorPickerIcon");
            splash = content.Load<Texture2D>("splash");
            bucketIcon = content.Load<Texture2D>("bucketIcon");
            UIfont = content.Load<SpriteFont>("SpriteFont1");
            smallPrint = content.Load<SpriteFont>("smallPrint");
        }

        public List<Action> update(int screenWidth, int screenHeight)
        {

            List<Action> result = new List<Action>();
            switch (state)
            {


                case PlayerState.working:
                    result.AddRange(processKeyboard());
                    result.AddRange(processMouse(.01f, screenWidth, screenHeight));
                    return result;

            }

            oldKeyboardState = Keyboard.GetState();
            return result;


        }

        public List<Action> processMouse(float amount, int screenWidth, int screenHeight)
        {
            if (MainWindow.singleton.hasDialogOpen())
            {
                return new List<Action>();
            }
            Vector2 currentMouseLocation = PainterMain.mouseLocationInXNASpace;



            List<Action> result = new List<Action>();
            MouseState currentMouseState = Mouse.GetState();



            KeyboardState keyState = Keyboard.GetState();
            if (currentMouseState != originalMouseState)
            {
                //if (currentMouseState.LeftButton == ButtonState.Pressed)
                //{
                //if (!(oldMouseState.RightButton == ButtonState.Pressed))
                //{
                float xDifference = 0;
                float yDifference = 0;
                if (currentMouseState.MiddleButton == ButtonState.Pressed || keyState.IsKeyDown(Keys.E))
                {
                    xDifference = currentMouseLocation.X - oldMouseLocation.X;
                    yDifference = currentMouseLocation.Y - oldMouseLocation.Y;
                }

                //}
                //Mouse.SetPosition(world.device.Viewport.Width / 2, world.device.Viewport.Height / 2);
                //Mouse.SetPosition((int)clickStartLoc.X,(int)clickStartLoc.Y);

                //}
                if (!Keyboard.GetState().IsKeyDown(Keys.Tab) && !colorPickerVisible)
                {
                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                    {
                        //   Mouse.SetPosition(screenWidth / 2, screenHeight / 2);
                    }
                    PainterMain.mouseIsVisible = false;
                    leftrightRot -= rotationSpeed * xDifference * amount;
                    updownRot -= rotationSpeed * yDifference * amount;
                    if (currentMouseState.LeftButton == ButtonState.Pressed && (oldMouseState.LeftButton != ButtonState.Pressed || keyState.IsKeyDown(Keys.LeftControl)))
                    {
                        if (keyState.IsKeyDown(Keys.LeftShift))
                        {
                            result.Add(new PlayerShiftLeftClickAction(playerAimNearPoint(), playerAimingAt()));

                        }
                        else if (keyState.IsKeyDown(Keys.LeftAlt))
                        {
                        }
                        else
                        {
                            result.Add(new PlayerLeftClickAction(playerAimNearPoint(), playerAimingAt(), mirror));
                        }

                    }
                    if (currentMouseState.RightButton == ButtonState.Pressed && (oldMouseState.RightButton != ButtonState.Pressed || keyState.IsKeyDown(Keys.LeftControl)))
                    {
                        if (keyState.IsKeyDown(Keys.LeftAlt))
                        {
                            result.Add(new PlayerAltClickAction(playerAimNearPoint(), playerAimingAt(), mirror));
                        }
                        else
                        {
                            result.Add(new PlayerRightClickAction(playerAimNearPoint(), playerAimingAt(), mirror));
                        }
                    }

                }
                else
                {
                    PainterMain.mouseIsVisible = true;
                    if (colorPickerVisible)
                    {
                        if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed)
                        {
                            Rectangle rect = new Rectangle((int)getColorPickerImageLoc(screenWidth, screenHeight).X,
                                (int)getColorPickerImageLoc(screenWidth, screenHeight).Y,
                                colorsImage.Width, colorsImage.Height);
                            if (rect.Contains(new Point((int)currentMouseLocation.X, (int)currentMouseLocation.Y)))
                            {
                                int xInPickerSpace = (int)currentMouseLocation.X - rect.X;
                                int yInPickerSpace = (int)currentMouseLocation.Y - rect.Y;

                                int arrayLoc = (yInPickerSpace / colorPickerBlockHeight) * 16 + xInPickerSpace / colorPickerBlockWidth;
                                selectedBlock = (byte)arrayLoc;

                            }
                        }
                    }




                }




            }


            if (updownRot > Math.PI / 2 - .01)
            {
                updownRot = (float)Math.PI / 2 - .01f;
            }
            else if (updownRot < -Math.PI / 2 + .01)
            {
                updownRot = -(float)Math.PI / 2 + .01f;
            }




            Vector3 moveVector = new Vector3(0, 0, 0);
            if (!keyState.IsKeyDown(Keys.LeftControl) && !keyState.IsKeyDown(Keys.LeftAlt))
            {
                if (keyState.IsKeyDown(Keys.W))
                    moveVector += new Vector3(0, 0, -1);
                if (keyState.IsKeyDown(Keys.S))
                    moveVector += new Vector3(0, 0, 1);
                if (keyState.IsKeyDown(Keys.D))
                    moveVector += new Vector3(1, 0, 0);
                if (keyState.IsKeyDown(Keys.A))
                    moveVector += new Vector3(-1, 0, 0);
                if (keyState.IsKeyDown(Keys.Q))
                    moveVector += new Vector3(0, 1, 0);
                if (keyState.IsKeyDown(Keys.Z))
                    moveVector += new Vector3(0, -1, 0);
            }

            if (currentMouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue)
            {
                moveVector += new Vector3(0, 0, -1);
            }

            // }
            //if (keyState.IsKeyDown(Keys.W))
            //    moveVector += new Vector3(0, 0, -1);
            //if (keyState.IsKeyDown(Keys.S))
            //    moveVector += new Vector3(0, 0, 1);
            // if (keyState.IsKeyDown(Keys.P))
            //world.testSpace.loc += new Vector3(0, -1, 0);

            if (moveVector.Length() > 0)
            {
                moveVector.Normalize();
            }

            moveVector *= .4f;
            if(keyState.IsKeyDown(Keys.LeftShift))
            {
                moveVector *= .1f;
            }

            Matrix cameraRotation = Matrix.CreateRotationY(leftrightRot);
            loc += Vector3.Transform(moveVector, cameraRotation);



            //Matrix cameraRotation =/* Matrix.CreateRotationX(updownRot) **/ Matrix.CreateRotationY(leftrightRot);
            //Vector3 rotatedVector = Vector3.Transform(moveVector, cameraRotation);


            oldMouseState = currentMouseState;
            oldMouseLocation = currentMouseLocation;
            return result;




        }

        public List<Action> processKeyboard()
        {
            if (MainWindow.singleton.hasDialogOpen())
            {
                return new List<Action>();
            }
            List<Action> result = new List<Action>();

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Space))
            {
                colorPickerVisible = true;

            }
            else
            {
                colorPickerVisible = false;
            }
            if (justHit(Keys.RightShift))
            {
                if (rectBuildLoc1 == null)
                {
                    rectBuildLoc1 = new Vector3(loc.X, loc.Y, loc.Z);
                }
                else
                {
                    result.Add(new BuildRectAction((Vector3)rectBuildLoc1,loc));
                    rectBuildLoc1 = null;
                }

            }

            if (keyState.IsKeyDown(Keys.LeftControl))
            {

                if (justHit(Keys.N))
                {
                    result.Add(new PlayerNewDocumentAction());
                }
                if (justHit(Keys.Z))
                {
                    result.Add(new PlayerUndoAction());
                }
                if (justHit(Keys.M))
                {
                    result.Add(new FlipSpaceAction());
                }
            }
            else if(keyState.IsKeyDown(Keys.LeftAlt)){
                if (justHit(Keys.W))
                {
                    result.Add(new MoveWorkAction(new Vector3(0, 0, 1)));
                }
                if (justHit(Keys.S))
                {
                    result.Add(new MoveWorkAction(new Vector3(0, 0, -1)));
                }
                if (justHit(Keys.A))
                {
                    result.Add(new MoveWorkAction(new Vector3(1, 0, 0)));
                }
                if (justHit(Keys.D))
                {
                    result.Add(new MoveWorkAction(new Vector3(-1, 0, 0)));
                }
                if (justHit(Keys.Q))
                {
                    result.Add(new MoveWorkAction(new Vector3(0, 1, 0)));
                }
                if (justHit(Keys.Z))
                {
                    result.Add(new MoveWorkAction(new Vector3(0, -1, 0)));
                }
            
            }
            else
            {

                if (justHit(Keys.M))
                {
                    mirror = !mirror;
                    //result.Add(new PlayerNeedsUndoSave());
                }
            }

            displayGuideLines = keyState.IsKeyDown(Keys.LeftShift);


            oldKeyboardState = keyState;
            return result;
        }

        public Vector3 playerAimingAt()
        {

            Vector3 far = new Vector3(PainterMain.mouseLocationInXNASpace.X, PainterMain.mouseLocationInXNASpace.Y, 1f);

            far = Compositer.device.Viewport.Unproject(far, Compositer.projectionMatrix, Compositer.viewMatrix, Matrix.Identity);
            return far;
        }

        public Vector3 playerAimNearPoint()
        {
            Vector3 near = new Vector3(PainterMain.mouseLocationInXNASpace.X, PainterMain.mouseLocationInXNASpace.Y, 0f);

            near = Compositer.device.Viewport.Unproject(near, Compositer.projectionMatrix, Compositer.viewMatrix, Matrix.Identity);
            return near;
        }

        public void display2D(SpriteBatch spriteBatch, int screenWidth, int screenHeight, int mouseX, int mouseY)
        {


            if (oldKeyboardState.IsKeyDown(Keys.LeftShift))
            {


                spriteBatch.Draw(colorPickerIcon, new Vector2(mouseX - colorPickerIcon.Width,
                    mouseY - colorPickerIcon.Width), Color.White);
            }
            if (oldKeyboardState.IsKeyDown(Keys.LeftAlt))
            {
                spriteBatch.Draw(bucketIcon, new Vector2(mouseX - bucketIcon.Width,
                    mouseY - bucketIcon.Width), Color.White);
            }
            else
            {
                //spriteBatch.Draw(crossReticle, new Vector2(screenWidth / 2 - crossReticle.Width / 2,
               //     screenHeight / 2 - crossReticle.Width / 2), Color.White);
            }
            displayColorPickerSystem(spriteBatch, screenHeight, screenWidth);
            spriteBatch.DrawString(UIfont, loc.ToString(), new Vector2(20, screenHeight - 30), Color.White);
            string uiExplanation = "[M]irror is currently " + (mirror ? "on" : "off") + "\n\nCube Studio by Isaac Dykeman, all rights reserved. \nPre alpha. Do not distribute.";
            spriteBatch.DrawString(UIfont, uiExplanation, new Vector2(20, 10), Color.White);
            //spriteBatch.DrawString(UIfont, "current model size is "+, new Vector2(screenWidth-100, screenHeight-29), Color.White);

        }

        public void displayColorPickerSystem(SpriteBatch spriteBatch, int screenHeight, int screenWidth)
        {
            Vector2 colorPickerLoc = getColorPickerImageLoc(screenWidth, screenHeight);
            if (colorPickerVisible)
            {
                spriteBatch.Draw(colorsImage, colorPickerLoc, Color.White);
                Vector2 currentColorLoc = new Vector2(selectedBlock % 16, selectedBlock / 16);
                currentColorLoc.X *= colorPickerBlockWidth;
                currentColorLoc.Y *= colorPickerBlockHeight;
                spriteBatch.Draw(colorSwatchhighlight, currentColorLoc - new Vector2(1, 1) + colorPickerLoc, Color.White);
            }
        }

        Vector2 getColorPickerImageLoc(int screenWidth, int screenHeight)
        {
            return new Vector2(screenWidth / 2 - colorsImage.Width / 2, screenHeight / 2 - colorsImage.Height / 2);
        }

        public bool justHit(Keys test)
        {
            bool result = false;
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(test))
            {
                if (!oldKeyboardState.IsKeyDown(test))
                {
                    result = true;
                }
            }
            return result;
        }

    }
}
