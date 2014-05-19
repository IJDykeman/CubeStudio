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

        public Block(Color ncolor)
        {
            paint = ncolor;
            Zero.PaintColor = paint;
            One.PaintColor = paint;
            Two.PaintColor = paint;
            Three.PaintColor = paint;
        }



        /*
        public bool[] updateOnGeometryModification(int x, int y, int z, PaintedCubeSpace toReplace)
        {

            coveredUp = true;
            coveredDown = true;
            coveredForward = true;
            coveredBack = true;
            coveredLeft = true;
            coveredRight = true;
            covered = false;
            //if (z+1<100) {
            if (z + 1 < toReplace.spaceWidth)
            {
                if (toReplace.isTransparentAt(x, y, z + 1))
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
                if (toReplace.isTransparentAt(x, y, z - 1))
                {
                    coveredForward = false;
                }
            }
            else
            {

                    coveredForward = false;
                

            }

            // +Y "bottom" face
            if (y + 1 <= toReplace.spaceHeight - 1)
            {


                    coveredUp = false;
                
            }
            else
            {
                coveredUp = false;
            }

            //  -Y "top" face

            if (y - 1 >= 0)
            {

                    coveredDown = false;
                
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

            if (x + 1 <= toReplace.spaceWidth - 1)
            {

                    coveredLeft = false;
                
            }
            else
            {
                coveredLeft = false;
                
            }
            // 
            // -X "left" face
            //if (x-1>=0) {
            if (x - 1 >= 0)
            {


                    coveredRight = false;
                
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
        */

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
                /////if (toReplace.spaceChunkWorld.worldIsTransparentAt(new Vector3(x + (int)toReplace.loc.X, y + (int)toReplace.loc.Y-1, z + (int)toReplace.loc.Z)))
                /////{
                coveredDown = false;
                ////}
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
            //if (x-1>=0) {
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
            //Zero.Color.R = new Color(Game1.world.rand.Next(255), Game1.world.rand.Next(255), Game1.world.rand.Next(255));
            //One.Color = new Color(Game1.world.rand.Next(255), Game1.world.rand.Next(255), Game1.world.rand.Next(255));
            //Two.Color = new Color(Game1.world.rand.Next(255), Game1.world.rand.Next(255), Game1.world.rand.Next(255));
            // Three.Color = new Color(Game1.world.rand.Next(255), Game1.world.rand.Next(255), Game1.world.rand.Next(255));

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
            /*drawFront(toReplace);
            drawBack(toReplace);
            drawTop(toReplace);
            drawBottom(toReplace);
            drawLeft(toReplace);
            drawRight(toReplace);
            toReplace.numFaces += 6;*/
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

        /*public  void drawWithBox(Game1 world)
        {
            Box test = new Box(loc, 1, 1, 1, new Vector3(), new Vector2());
            test.draw(world);
        }*/

        public void drawFront(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)//normal along z
        {



            Vector3 thisNormal = new Vector3(0, 0, -1f);
            Vector2 frontTexturePos = texturePos;




            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;



            Zero.Position = XlYhZl;//new Vector3(0 + loc.X, cubeHeight + loc.Y, 0f + loc.Z);
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];

            One.Position = XlYlZl;//new Vector3(0f + loc.X, 0f + loc.Y, 0f + loc.Z);
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];

            Two.Position = XhYlZl;//new Vector3(cubeWidth + loc.X, 0f + loc.Y, 0f + loc.Z);
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];

            Three.Position = XhYhZl;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, 0f + loc.Z);
            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];


            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;




        }

        public void drawBack(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


            Vector3 thisNormal = new Vector3(3f, -.5f, 1f);
            thisNormal.Normalize();
            Vector2 frontTexturePos = texturePos;






            //vertexList[toReplace.count + 4].TextureCoordinate = new Vector2(imageWidth + frontTexturePos.X, 0 + frontTexturePos.Y);
            //vertexList[toReplace.count + 5].TextureCoordinate = new Vector2(0 + frontTexturePos.X, imageWidth + frontTexturePos.Y);

            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 2);
            space.indexCount += 6;


            Zero.Position = XlYhZh;//new Vector3(0 + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = sideColor;
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];
            //vertexList[toReplace.count].TextureCoordinate = new Vector2(0 + frontTexturePos.X, 0 + frontTexturePos.Y);



            One.Position = XlYlZh;//new Vector3(0f + loc.X, 0f + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = sideColor;
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];
            //vertexList[toReplace.count].TextureCoordinate = new Vector2(frontTexturePos.X, 0 + frontTexturePos.Y + imageWidth);


            Two.Position = XhYlZh;//new Vector3(cubeWidth + loc.X, 0f + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = sideColor;
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];
            // vertexList[toReplace.count].TextureCoordinate = new Vector2(imageWidth + frontTexturePos.X, imageWidth + frontTexturePos.Y);


            Three.Position = XhYhZh;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = sideColor;
            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];
            // vertexList[toReplace.count].TextureCoordinate = new Vector2(imageWidth + frontTexturePos.X, 0 + frontTexturePos.Y);



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;




        }

        public void drawRight(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


            Vector3 thisNormal = new Vector3(-.6f, -.5f, 1f);
            thisNormal.Normalize();
            Vector2 frontTexturePos = texturePos;





            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;


            Zero.Position = XlYlZh;//new Vector3(0f + loc.X, 0f + loc.Y, cubeWidth + loc.Z);//
            //vertexList[toReplace.count].Color = sideColor;
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];



            One.Position = XlYhZl;//new Vector3(0 + loc.X, cubeHeight + loc.Y, 0f + loc.Z);//
            //vertexList[toReplace.count].Color = sideColor;
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];



            Two.Position = XlYhZh;//new Vector3(0f + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);//
            //vertexList[toReplace.count].Color = sideColor;
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];



            Three.Position = XlYlZl;//new Vector3(0f + loc.X, 0f + loc.Y, 0f + loc.Z);//
            //vertexList[toReplace.count].Color = sideColor;
            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];




            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }

        public void drawLeft(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


            Vector3 thisNormal = new Vector3(1, 0, 0);


            thisNormal.Normalize();
            Vector2 frontTexturePos = texturePos;





            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            space.indexCount += 6;


            Zero.Position = XhYlZh;//new Vector3(cubeWidth + loc.X, 0f + loc.Y, cubeWidth + loc.Z);//
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];



            One.Position = XhYhZl;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, 0f + loc.Z);//
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];



            Two.Position = XhYhZh;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);//
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];



            Three.Position = XhYlZl;//new Vector3(cubeWidth + loc.X, 0f + loc.Y, 0f + loc.Z);//
            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;

        }

        public void drawBottom(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {

            Vector3 thisNormal = new Vector3(0, 1, 0);
            //setupVertices();






            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 1);
            space.indexCount += 6;



            Zero.Position = XlYlZl;//new Vector3(0f + loc.X, 0 + loc.Y, 0f + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZl];




            One.Position = XhYlZh;//new Vector3(cubeWidth + loc.X, 0 + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZh];



            Two.Position = XlYlZh;//new Vector3(0f + loc.X, 0 + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYlZh];




            Three.Position = XhYlZl;//new Vector3(cubeWidth + loc.X, 0 + loc.Y, 0f + loc.Z);

            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYlZl];



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }



        public void drawTop(PaintedCubeSpace space, List<int> indexList, List<VertexPostitionColorPaintNormal> vertexList, Vector3 loc, byte[] AOarray)
        {


            Vector3 thisNormal = new Vector3(0, 1, 0);
            //setupVertices();






            indexList.Add(space.vertexCount + 0);
            indexList.Add(space.vertexCount + 3);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 1);
            indexList.Add(space.vertexCount + 2);
            indexList.Add(space.vertexCount + 0);
            space.indexCount += 6;



            Zero.Position = XlYhZl;//new Vector3(0f + loc.X, cubeHeight + loc.Y, 0f + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            Zero.Normal = thisNormal;
            Zero.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZl];



            One.Position = XhYhZh;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            One.Normal = thisNormal;
            One.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZh];


            Two.Position = XlYhZh;//new Vector3(0f + loc.X, cubeHeight + loc.Y, cubeWidth + loc.Z);
            //vertexList[toReplace.count].Color = topColor;
            Two.Normal = thisNormal;
            Two.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XlYhZh];



            Three.Position = XhYhZl;//new Vector3(cubeWidth + loc.X, cubeHeight + loc.Y, 0f + loc.Z);

            Three.Normal = thisNormal;
            Three.Color.R = AOarray[(int)PaintedCubeSpace.cornerToAOArrayLoc.XhYhZl];



            vertexList.Add(Zero);

            vertexList.Add(One);
            vertexList.Add(Two);

            vertexList.Add(Three);
            space.vertexCount += 4;


        }

    }
}
