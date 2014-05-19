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
    public class Button
    {
        public Texture2D texture;
        CubeAnimator.Action action;
        Vector2 location;

        public Button(CubeAnimator.Action naction, Texture2D ntexture, Vector2 nLoc)
        {
            action = naction;
            texture = ntexture;
            location = nLoc;
        }

        public List<CubeAnimator.Action> click(Vector2 clickLoc)
        {
            List<CubeAnimator.Action> result = new List<CubeAnimator.Action>();
            if(getRectangle().Contains(new Point((int)clickLoc.X,(int)clickLoc.Y)))
            {
                result.Add(action);
            }
            return result;
        }

        public Rectangle getRectangle()
        {
            return new Rectangle((int)location.X, (int)location.Y, texture.Width, texture.Height);
        }

        

    }
}
