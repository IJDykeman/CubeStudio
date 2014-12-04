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
using CubePainter;



namespace CubeStudio
{
    public class Block
    {

        bool coveredUp, coveredDown, coveredForward, coveredBack, coveredLeft, coveredRight, covered;

        public Color topColor;
        public Color sideColor;
        public Vector2 texturePos;
        public Vector2 topTexturePos;
        //public Texture2D texture;
        public float cubeWidth = 1;
        public float cubeHeight = 1;
        public float bottomDrop = 0;
        public float cubeEdgePadding = 0;

        Color paint;

        Vector3 XlYlZl, XhYhZh, XhYhZl, XlYhZh, XlYhZl, XlYlZh, XhYlZl, XhYlZh;

        VertexPostitionColorPaintNormal Zero = new VertexPostitionColorPaintNormal();

        VertexPostitionColorPaintNormal One = new VertexPostitionColorPaintNormal();
        VertexPostitionColorPaintNormal Two = new VertexPostitionColorPaintNormal();
        VertexPostitionColorPaintNormal Three = new VertexPostitionColorPaintNormal();

        static readonly float imageFudge = .002f;
        static readonly float imageWidth = 1.0f / 16.0f - imageFudge;
        static Vector2[] texCoordsByCorner;

        public Block(Color ncolor)
        {
            Byte nType = 1;
            paint = ncolor;
            Zero.PaintColor = paint;
            One.PaintColor = paint;
            Two.PaintColor = paint;
            Three.PaintColor = paint;
            Zero.extraData.G = nType;
            One.extraData.G = nType;
            Two.extraData.G = nType;
            Three.extraData.G = nType;

            texCoordsByCorner = new Vector2[8];

            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh] = new Vector2(1, 1);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl] = new Vector2(0, 1);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh] = new Vector2(1, 0);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl] = new Vector2(0, 0);

            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh] = new Vector2(0, 0);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl] = new Vector2(1, 0);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh] = new Vector2(0, 1);
            texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl] = new Vector2(1, 1);
        }


        public bool[] updateOnGeometryModification(int x, int y, int z, PaintedCubeSpace space)
        {

            coveredUp = true;
            coveredDown = true;
            coveredForward = true;
            coveredBack = true;
            coveredLeft = true;
            coveredRight = true;
            covered = false;
            //if (z+1<100) {
            if (z + 1 < space.spaceWidth)
            {
                if (space.isTransparentAt(x, y, z + 1))
                {

                    coveredBack = false;
                }
            }
            else
            {
                    coveredBack = false;
                
            }

            // -Z "back" face
            //if (z-1>=0) {
            if (z - 1 >= 0)
            {
                if (space.isTransparentAt(x, y, z - 1))
                {
                    coveredForward = false;
                }
            }
            else
            {
                    coveredForward = false;

            }

            // +Y "bottom" face
            if (y + 1 <= space.spaceHeight - 1)
            {
                if (space.isTransparentAt(x, y + 1, z))
                {
                    coveredUp = false;
                }
            }
            else
            {
                coveredUp = false;
            }

            //  -Y "top" face

            if (y - 1 >= 0)
            {

                if (space.isTransparentAt(x, y - 1, z))
                {
                    coveredDown = false;
                }
                
                
            }
            else
            {
                coveredDown = false;
            }
            if (y == 0)
            {
                coveredDown = false;
            }

            // +X "right" face

            if (x + 1 <= space.spaceWidth - 1)
            {
                if (space.isTransparentAt(x + 1, y, z))
                {
                    coveredLeft = false;
                }
            }
            else
            {

                    coveredLeft = false;
                
            }
            // 
            // -X "left" face
            if (x - 1 >= 0)
            {
                if (space.isTransparentAt(x - 1, y, z))
                {

                    coveredRight = false;
                }
            }
            else
            {

                    coveredRight = false;
                
            }

            if (coveredUp && coveredDown && coveredForward && coveredBack && coveredLeft && coveredRight)
            {
                covered = true;
            }


            bool[] resultCFBLRUD = new bool[7];
            resultCFBLRUD[0] = covered;
            resultCFBLRUD[1] = coveredForward;
            resultCFBLRUD[2] = coveredBack;
            resultCFBLRUD[3] = coveredLeft;
            resultCFBLRUD[4] = coveredRight;
            resultCFBLRUD[5] = coveredUp;
            resultCFBLRUD[6] = coveredDown;
            return resultCFBLRUD;

        }

        public int draw(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, bool[] coveredArray, byte[][] AOarray)
        {

            XlYhZl = new Vector3(0 + loc.X + cubeEdgePadding, cubeHeight + loc.Y, 0f + loc.Z + cubeEdgePadding);

            XlYhZh = new Vector3(0 + loc.X + cubeEdgePadding, cubeHeight + loc.Y, cubeWidth + loc.Z - cubeEdgePadding);

            XlYlZl = new Vector3(0f + loc.X + cubeEdgePadding, 0f + loc.Y - bottomDrop, 0f + loc.Z + cubeEdgePadding);

            XlYlZh = new Vector3(0f + loc.X + cubeEdgePadding, 0f + loc.Y - bottomDrop, cubeWidth + loc.Z - cubeEdgePadding);


            XhYlZl = new Vector3(cubeWidth + loc.X - cubeEdgePadding, 0f + loc.Y - bottomDrop, 0f + loc.Z + cubeEdgePadding);

            XhYhZl = new Vector3(cubeWidth + loc.X - cubeEdgePadding, cubeHeight + loc.Y, 0f + loc.Z + cubeEdgePadding);

            XhYlZh = new Vector3(cubeWidth + loc.X - cubeEdgePadding, 0f + loc.Y - bottomDrop, cubeWidth + loc.Z - cubeEdgePadding);

            XhYhZh = new Vector3(cubeWidth + loc.X - cubeEdgePadding, cubeHeight + loc.Y, cubeWidth + loc.Z - cubeEdgePadding);


            covered = coveredArray[0];
            coveredForward = coveredArray[1];
            coveredBack = coveredArray[2];
            coveredLeft = coveredArray[3];
            coveredRight = coveredArray[4];
            coveredUp = coveredArray[5];
            coveredDown = coveredArray[6];



            if (!covered)
            {


                if (!coveredForward)
                {

                    drawFront(space, indexList, vertexList, loc, AOarray[2]);
                    space.numFaces++;

                }
                if (!coveredBack)
                {

                    drawBack(space, indexList, vertexList, loc, AOarray[2]);
                    space.numFaces++;

                }
                if (!coveredRight)
                {
                    drawRight(space, indexList, vertexList, loc, AOarray[0]);
                    space.numFaces++;


                }
                if (!coveredLeft)
                {
                    drawLeft(space, indexList, vertexList, loc, AOarray[0]);
                    space.numFaces++;

                }
                if (!coveredDown)
                {
                    drawBottom(space, indexList, vertexList, loc, AOarray[1]);
                    space.numFaces++;

                }
                if (!coveredUp)
                {
                    drawTop(space, indexList, vertexList, loc, AOarray[1]);
                    space.numFaces++;

                }

            }
            return 0;
        }

        public int getNumFacesExposed(int x, int y, int z, PaintedCubeSpace space)
        {
            updateOnGeometryModification(x, y, z, space);

            int result = 0;
            if (!covered)
            {


                if (!coveredForward)
                {


                    result++;
                }
                if (!coveredBack)
                {

                    result++;
                }
                if (!coveredRight)
                {

                    result++;

                }
                if (!coveredLeft)
                {

                    result++;
                }
                if (!coveredDown)
                {

                    result++;
                }
                if (!coveredUp)
                {

                    result++;
                }

            }
            return result;
        }

        public void assign_Normal_Binormal_TanToOneTwoThreeFour(Vector3 normal, Vector3 binormal, Vector3 tangent)
        {
            One.Normal = normal;
            One.BiNormal = binormal;
            One.Tangent = tangent;

            Two.Normal = normal;
            Two.BiNormal = binormal;
            Two.Tangent = tangent;

            Three.Normal = normal;
            Three.BiNormal = binormal;
            Three.Tangent = tangent;

            Zero.Normal = normal;
            Zero.BiNormal = binormal;
            Zero.Tangent = tangent;


        }


        public void drawFront(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)//normal along z
        {

            assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(0, 0, -1), new Vector3(0, 1, 0), new Vector3(1, 0, 0));

            Vector2 frontTexturePos = texturePos;




            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;



            Zero.Position = XlYhZl;//new Vector3(0 + loc.X, cubeHeight + loc.Y, 0f + loc.Z);
            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];

            One.Position = XlYlZl;//new Vector3(0f + loc.X, 0f + loc.Y, 0f + loc.Z);
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];

            Two.Position = XhYlZl;//new Vector3(cubeWidth + loc.X, 0f + loc.Y, 0f + loc.Z);
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];

            Three.Position = XhYhZl;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, 0f + loc.Z);
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];


            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;




        }

        public void drawBack(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {
            assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector3(-1, 0, 0));

            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 2);
            space.indexCount += 6;


            Zero.Position = XlYhZh;

            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];



            One.Position = XlYlZh;
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];


            Two.Position = XhYlZh;
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];


            Three.Position = XhYhZh;
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;




        }

        public void drawRight(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


          assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));


            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;


            Zero.Position = XlYlZh;
            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];


            One.Position = XlYhZl;
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];


            Two.Position = XlYhZh;
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];


            Three.Position = XlYlZl;
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }

        public void drawLeft(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


           assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));

            Vector2 frontTexturePos = texturePos;

            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            space.indexCount += 6;


            Zero.Position = XhYlZh;
            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];


            One.Position = XhYhZl;
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];


            Two.Position = XhYhZh;
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];


            Three.Position = XhYlZl;
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];


            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;

        }

        public void drawBottom(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {

            assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(0, -1, 0), new Vector3(1, 0, 0), new Vector3(0,0, 1));

            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            space.indexCount += 6;


            //Top and bottom texture coords use different corners' texcoords.  This is good.
            Zero.Position = XlYlZl;
            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];



            One.Position = XhYlZh;
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];


            Two.Position = XlYlZh;
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];



            Three.Position = XhYlZl;
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];


            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }

        public void drawTop(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


           assign_Normal_Binormal_TanToOneTwoThreeFour(new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1));







            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;



            Zero.Position = XlYhZl;
            Zero.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];
            Zero.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];


            One.Position = XhYhZh;;
            One.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];
            One.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];

            Two.Position = XlYhZh;
            Two.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];
            Two.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];


            Three.Position = XhYhZl;
            Three.extraData.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];
            Three.textureCoordinate = texCoordsByCorner[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];


            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }

    }
}
