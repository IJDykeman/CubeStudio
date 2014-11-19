using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace CubeStudio
{


        public struct VertexPostitionColorPaintNormal
        {
            public Vector3 Position;
            public Color Color;//R holds AO, G holds byte type
            public Color PaintColor;
            public Vector3 Normal;
            public Vector3 BiNormal;
            public Vector3 Tangent;
            public Vector2 textureCoordinate;
            public Vector3 InterPosition;



            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3+4, VertexElementFormat.Color, VertexElementUsage.Color, 1),
                new VertexElement(sizeof(float) * 3 + 8, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 3 * 2 + 8, VertexElementFormat.Vector3, VertexElementUsage.Normal, 1),
                new VertexElement(sizeof(float) * 3 * 3 + 8, VertexElementFormat.Vector3, VertexElementUsage.Normal, 2),
                new VertexElement(sizeof(float) * 3 * 4 + 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * (3 * 4 + 2 )+ 8, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 1)

            );
        }


    
}
