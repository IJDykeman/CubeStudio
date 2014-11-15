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
    public static class Compositer
    {
        static RenderTarget2D mainTarget;
        //static Texture2D finalBlurred;
        public static GraphicsDevice device;
        
        static SpriteBatch spriteBatch;
        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;
        public static Effect effect;


        public static void construct(GraphicsDevice mdevice)
        {
            device = mdevice;


        }

        public static void LoadContent(ContentManager content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(device);
            effect = content.Load<Effect>("effects");
           


            // TODO: use this.Content to load your animationProgram content here
        }

        static void Initialize()
        {

            
        }

        public static void drawFinalImage( Painter player, PaintedCubeSpace space, int width, int height, int mouseX, int mouseY)
        {
            //width /= 2;
            
              mainTarget = new RenderTarget2D(device, width, height,false, device.DisplayMode.Format, DepthFormat.Depth24, 
                  4, RenderTargetUsage.DiscardContents);

            //device.SetRenderTarget(mainTarget);
            device.Clear(Color.LightGray);
            device.DepthStencilState = new DepthStencilState();

            effect.CurrentTechnique = effect.Techniques["Colored"];
            UpdateViewMatrix(player.loc, player.updownRot, player.leftrightRot, width, height);
            //  blurer = new GaussianBlurHandler(main.Content.Load<Effect>("GaussianBlur"), device,
            //graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            


            drawChunk(space);

            if (player.displayGuideLines)
            {
                drawLine(new Vector3(0, -100, 0), new Vector3(0, 100, 0));
                drawLine(new Vector3(-100, 0, 0), new Vector3(100, 0, 0));
                drawLine(new Vector3(0, 0, -100), new Vector3(0, 0, 100));
            }
            spriteBatch.Begin();
            player.display2D(spriteBatch, PaintProgram.width,PaintProgram.height, mouseX, mouseY);

            spriteBatch.End();

            device.SetRenderTarget(null);
            spriteBatch.Begin();
            
            spriteBatch.Draw((Texture2D)mainTarget, new Rectangle(0,0,width,height), Color.White);
            spriteBatch.End();
            
            mainTarget.Dispose();
        }



        public static void drawChunk(PaintedCubeSpace paintedCubeSpace)
        {

            device.DepthStencilState = new DepthStencilState()
            {
                DepthBufferEnable = true
            };
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xAmbient"].SetValue(.6f);
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);

            effect.Parameters["xEnableLighting"].SetValue(true);

            Vector3 lightDirection = new Vector3(-.3f, .5f, -1f);

            lightDirection.Normalize();
            lightDirection *= (float).3f;
            effect.Parameters["xLightDirection"].SetValue(lightDirection);


            // Matrix sunRotation = Matrix.CreateRotationX(MathHelper.ToRadians(updateCount)) * Matrix.CreateRotationZ(MathHelper.ToRadians(updateCount));

            paintedCubeSpace.draw(device, effect);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);


            
        }

        private static void SetUpCamera(int width, int height)
        {
            //temp += 0.11f;
            //viewMatrix = Matrix.CreateLookAt(player.loc, new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            float viewDistance = (float)300;
            
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2.3f, device.Viewport.AspectRatio, 0.09f, viewDistance); //used to be 1 and 300 for the last two arguments
        }


        public static void UpdateViewMatrix(Vector3 loc, float updownRot, float leftrightRot, int width, int height)
        {


            SetUpCamera(width,height);
            if (updownRot > MathHelper.ToRadians(89))
            {
                updownRot = MathHelper.ToRadians(89);
            }
            else if (updownRot < MathHelper.ToRadians(-87))
            {
                updownRot = MathHelper.ToRadians(-87);
            }


            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);

            Vector3 cameraFinalTarget = loc + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);



            viewMatrix = Matrix.CreateLookAt(loc, new Vector3(0,0,0), new Vector3(0,1,0));

        }

        public static void drawLine(Vector3 loc1, Vector3 loc2)
        {
            effect.Parameters["xAmbient"].SetValue(0);
            effect.CurrentTechnique = effect.Techniques["ColoredNoShading"];
            List<Vector3> locations= new List<Vector3>(2);
            locations.Add(loc1);
            locations.Add(loc2);
            Color color = Color.Blue;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {

                pass.Apply();
                var data = new List<VertexPositionColor>(locations.Count * 2);
                for (int i = 1; i < locations.Count; i++)
                {





                    data.Add(new VertexPositionColor(locations[i], color));
                    data.Add(new VertexPositionColor(locations[i - 1], color));
                }
               device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, data.ToArray(), 0, data.Count / 2);
            }
        }


    }
}