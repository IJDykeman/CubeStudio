using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CubePainter;
using System.Windows.Forms;

namespace CubeStudio
{
    public class ButtonInterface
    {
        public List<Button> buttons;

        public ButtonInterface(List<Button>nList)
        {
            buttons = nList;
        }


        public List<CubeAnimator.Action> click(Vector2 click)
        {
            List<CubeAnimator.Action> result = new List<CubeAnimator.Action>();
            foreach (Button button in buttons)
            {
                result.AddRange(button.click(click));
            }
            return result;
        }

        public static ButtonInterface getAnimatorInterface(int width, int height, Texture2D plusTex, Texture2D minusTex, Texture2D rotateZTexRight, Texture2D rotateZTexLeft, Texture2D rotateYTexRight, Texture2D rotateYTexLeft, Texture2D rotateXTexForward, Texture2D rotateXTexBack, Texture2D forwardTex,
            Texture2D backTex, Texture2D upTex,Texture2D downTex,Texture2D leftTex,Texture2D rightTex)
        {
            List<Button> buttonList = new List<Button>();
            buttonList.Add(new Button(new CubeAnimator.MoveDownAction(),downTex,new Vector2(width-downTex.Width*2-13,height-150)));
            buttonList.Add(new Button(new CubeAnimator.MoveBackAction(), backTex, new Vector2(width - downTex.Width * 2 - 13, height - 150-backTex.Height-3)));
            buttonList.Add(new Button(new CubeAnimator.MoveForwardAction(), forwardTex, new Vector2(width - downTex.Width * 2 - 13, height - 150 - backTex.Height*2 - 3*2)));
            buttonList.Add(new Button(new CubeAnimator.MoveUpAction(), upTex, new Vector2(width - downTex.Width * 2 - 13, height - 150 - backTex.Height * 3 - 3*3)));
            buttonList.Add(new Button(new CubeAnimator.MoveLeftAction(), leftTex, new Vector2(width - downTex.Width * 3 - 13-3, height - 150 - backTex.Height * 1.5f -1)));
            buttonList.Add(new Button(new CubeAnimator.MoveRightAction(), rightTex, new Vector2(width - downTex.Width * 1 - 10, height - 150 - backTex.Height * 1.5f - 1)));

            buttonList.Add(new Button(new CubeAnimator.ScaleUpAction(), plusTex, new Vector2(width - plusTex.Width*2 - 13, height - 270 - 28)));
            buttonList.Add(new Button(new CubeAnimator.ScaleDownAction(), minusTex, new Vector2(width - plusTex.Width*2 - 13, height - 270)));

            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(0,0,.1f)), rotateZTexRight, new Vector2(width - plusTex.Width * 2 - 13, height - 270 - 28*4)));
            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(-.1f, 0, 0)), rotateYTexLeft, new Vector2(width - plusTex.Width * 2 - 13, height - 270 - 28 * 3)));
            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(0, .1f, 0)), rotateXTexBack, new Vector2(width - plusTex.Width * 2 - 13, height - 270 - 28 * 5)));

            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(0, 0, -.1f)), rotateZTexLeft, new Vector2(width - plusTex.Width * 3 - 13, height - 270 - 28 * 4)));
            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(.1f, 0, 0)), rotateYTexRight, new Vector2(width - plusTex.Width * 3 - 13, height - 270 - 28 * 3)));
            buttonList.Add(new Button(new CubeAnimator.RotateAction(new Vector3(0, -.1f, 0)), rotateXTexForward, new Vector2(width - plusTex.Width * 3 - 13, height - 270 - 28 * 5)));

            //buttonList.Add(new Button(new CubeAnimator,plusTex,new Vector2(width-plusTex.Width-8,height-80+28+28)));


            ButtonInterface newInterface = new ButtonInterface(buttonList);
            return newInterface;
        }


        public void display(SpriteBatch spriteBatch)
        {
            foreach(Button button in buttons){
                spriteBatch.Draw(button.texture, button.getRectangle(), Color.White);
            }
        }
    }
}
