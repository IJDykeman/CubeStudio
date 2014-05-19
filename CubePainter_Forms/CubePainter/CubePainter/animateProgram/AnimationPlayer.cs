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

namespace CubeAnimator
{
    public class AnimationPlayer : CubeStudio.Player
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
        Texture2D animateButton;
        Vector2 animateButtonLoc;
        Texture2D splash;
        Texture2D plus;
        Texture2D minus;
        Texture2D rotateZRight, rotateZLeft, rotateYLeft, rotateYRight, rotateXForward, rotateXBack;
        Texture2D up, down, left, right, forward, back;

        SpriteFont UIfont;

        enum PlayerState
        {
            //splash,
            working,
            paused
        }

        PlayerState state = PlayerState.working;

        bool colorPickerVisible;
        bool mirror = false;



        public AnimationPlayer()
        {
            loc = new Vector3(-17,4,-16);


            oldMouseState = Mouse.GetState();
            oldKeyboardState = Keyboard.GetState();
            oldMouseLocation = AnimatorMain.mouseLocationInXNASpace;
        }

        public void loadContent(ContentManager content)
        {
            crossReticle = content.Load<Texture2D>("crossReticle");
            animateButton = content.Load<Texture2D>("animateButton");
            splash = content.Load<Texture2D>("splash");
            plus = content.Load<Texture2D>("UI/plusButton");
            rotateZRight = content.Load<Texture2D>("UI/rotateZButtonRight");
            rotateYRight = content.Load<Texture2D>("UI/rotateYButtonRight");
            rotateXBack = content.Load<Texture2D>("UI/rotateXButtonUp");
            rotateZLeft = content.Load<Texture2D>("UI/rotateZButtonLeft");
            rotateYLeft = content.Load<Texture2D>("UI/rotateYButtonLeft");
            rotateXForward = content.Load<Texture2D>("UI/rotateXButtonDown");

            minus = content.Load<Texture2D>("UI/minusButton");

            up = content.Load<Texture2D>("UI/upButton");
            down = content.Load<Texture2D>("UI/downButton");
            left = content.Load<Texture2D>("UI/leftButton");
            right = content.Load<Texture2D>("UI/rightButton");
            forward = content.Load<Texture2D>("UI/forwardButton");
            back = content.Load<Texture2D>("UI/backButton");


            UIfont = content.Load<SpriteFont>("SpriteFont1");
        }

        public List<Action> update(int screenWidth, int screenHeight, bool isAPartSelected)
        {
            if (isAPartSelected)
            {
                if (buttonInterface == null)
                {
                    buttonInterface = ButtonInterface.getAnimatorInterface(screenWidth, screenHeight, plus,
                        minus, rotateZRight, rotateZLeft, rotateYRight, rotateYLeft, rotateXForward, rotateXBack, forward, back, up, down, left, right);
                }
            }
            else
            {
                buttonInterface = null;
            }

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

            Vector2 currentMouseLocation = AnimatorMain.mouseLocationInXNASpace;


            List<Action> result = new List<Action>();
            MouseState currentMouseState = Mouse.GetState();



            KeyboardState keyState = Keyboard.GetState();
            if (currentMouseState != originalMouseState)
            {

                float xDifference = currentMouseLocation.X - oldMouseLocation.X;
                float yDifference = currentMouseLocation.Y - oldMouseLocation.Y;

                if (!Keyboard.GetState().IsKeyDown(Keys.Tab) )
                {
                    if (currentMouseState.MiddleButton == ButtonState.Pressed || keyState.IsKeyDown(Keys.E))
                    {
                        leftrightRot -= rotationSpeed * xDifference * amount;
                        updownRot -= rotationSpeed * yDifference * amount;
                    }
                    if (currentMouseState.LeftButton == ButtonState.Pressed && (oldMouseState.LeftButton != ButtonState.Pressed) && clicked(animateButton,animateButtonLoc,currentMouseLocation))
                    {
                        result.Add(new AnimateButtonClicked());
                    }
                    else  if (currentMouseState.LeftButton == ButtonState.Pressed && (oldMouseState.LeftButton != ButtonState.Pressed || keyState.IsKeyDown(Keys.LeftControl)))
                    {
                        List<Action> interfaceActions = new List<Action>();

                        if (buttonInterface != null)
                        {
                            interfaceActions = buttonInterface.click(currentMouseLocation);
                        }
                        result.AddRange(interfaceActions);
                        if (interfaceActions.Count == 0)
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

                    }
                    if (currentMouseState.RightButton == ButtonState.Pressed && (oldMouseState.RightButton != ButtonState.Pressed || keyState.IsKeyDown(Keys.LeftControl)))
                    {
                        if (keyState.IsKeyDown(Keys.LeftAlt))
                        {
                            Console.WriteLine("alt");
                            result.Add(new PlayerAltClickAction(loc, playerAimingAt(), mirror));
                        }
                        else
                        {
                            result.Add(new PlayerRightClickAction(loc, playerAimingAt(), mirror));
                        }
                    }

                }
                else
                {
                    AnimatorMain.mouseIsVisible = true;
                    
                    
                    


                
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
            if (!keyState.IsKeyDown(Keys.LeftControl))
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

            Matrix cameraRotation = Matrix.CreateRotationY(leftrightRot);
            loc += Vector3.Transform(moveVector, cameraRotation);



            //Matrix cameraRotation =/* Matrix.CreateRotationX(updownRot) **/ Matrix.CreateRotationY(leftrightRot);
            //Vector3 rotatedVector = Vector3.Transform(moveVector, cameraRotation);


            oldMouseState = Mouse.GetState();
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

            if(keyState.IsKeyDown(Keys.Space))
            {
                colorPickerVisible = true;

            }
            else
            {
                colorPickerVisible = false;
            }
            if (keyState.IsKeyDown(Keys.LeftControl))
            {
                if (justHit(Keys.S))
                {
                    result.Add(new PlayerSaveDocumentAction());

                }
                if (justHit(Keys.O))
                {
                    result.Add(new PlayerOpenDocumentAction());
                }
                if (justHit(Keys.N))
                {
                    result.Add(new PlayerNewDocumentAction());
                }
                if (justHit(Keys.Z))
                {
                    result.Add(new PlayerUndoAction());
                }
                if (justHit(Keys.C))
                {
                    result.Add(new PlayerCtrlC());
                }
            }

            if (justHit(Keys.M))
            {
                mirror = !mirror;
                result.Add(new PlayerNeedsUndoSave());
            }

            displayGuideLines = keyState.IsKeyDown(Keys.LeftShift);


            oldKeyboardState = keyState;
            return result;
        }

        public Vector3 playerAimingAt()
        {
            Vector3 far = new Vector3(AnimatorMain.mouseLocationInXNASpace.X, AnimatorMain.mouseLocationInXNASpace.Y, 1f);

            far = Compositer.device.Viewport.Unproject(far, Compositer.projectionMatrix, Compositer.viewMatrix, Matrix.Identity);
            return far;
        }

        public Vector3 playerAimNearPoint()
        {
            Vector3 near = new Vector3(AnimatorMain.mouseLocationInXNASpace.X, AnimatorMain.mouseLocationInXNASpace.Y, 0f);

            near = Compositer.device.Viewport.Unproject(near, Compositer.projectionMatrix, Compositer.viewMatrix, Matrix.Identity);
            return near;
        }

        public void display2D(SpriteBatch spriteBatch, int screenWidth, int screenHeight, bool explainUIToUser, bool isAnimating)
        {

            string explanation = "Right click a .vox file to the left or load one\nusing the file menu to begin creating a character";
            Vector2 stringOffset = UIfont.MeasureString(explanation);
            if (explainUIToUser)
            {
                spriteBatch.DrawString(UIfont, explanation, new Vector2(screenWidth / 2 - stringOffset.X / 2, screenHeight / 2 - stringOffset.Y / 2), Color.White);
            }

            animateButtonLoc = new Vector2(screenWidth - animateButton.Width - 20, screenHeight - animateButton.Height - 10);
            spriteBatch.Draw(animateButton, animateButtonLoc, (isAnimating? Color.White:new Color(100,100,100,100)));
               // spriteBatch.Draw(crossReticle, new Vector2(screenWidth / 2 - crossReticle.Width / 2,
               //     screenHeight / 2 - crossReticle.Width / 2), Color.White);
            
           //spriteBatch.DrawString(UIfont, loc.ToString(), new Vector2(20,  screenHeight-30), Color.White);
            if (buttonInterface != null)
            {
                buttonInterface.display(spriteBatch);
            }

        }

        bool clicked(Texture2D buttonTex, Vector2 buttonLoc, Vector2 mouseLoc)
        {
            return (new Rectangle((int)buttonLoc.X, (int)buttonLoc.Y, buttonTex.Width, buttonTex.Height).
                Contains(new Point((int)mouseLoc.X, (int)mouseLoc.Y))); 
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
