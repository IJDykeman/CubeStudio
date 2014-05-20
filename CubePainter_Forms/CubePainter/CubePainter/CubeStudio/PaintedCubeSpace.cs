using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
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
using CubeStudio;

//The version of PaintedCubeSpace that once existed in this solution
//  is now in the back stored in backupBucket


using CubePainter; //dependent on Block class

namespace CubeStudio
{
    public class PaintedCubeSpace
    {
        public int spaceWidth;
        public int spaceHeight;
        public int numFaces = 0;
        public VertexPostitionColorPaintNormal[] vertices;
        public short[] indices;
        public IndexBuffer indexBuffer;
        public VertexBuffer vertexBuffer;

        public static readonly byte AIR=0;

        public Quaternion rotation;
        public float scale = .99f;

        public bool readyToBeDisplayed = false;

        public bool serializedAfterLastChange = false; //must be set to false when the chunk is generated or changed


        //float widthOfCubes = 1;

        public Vector3 loc;


        public byte[, ,] array;


        public int vertexCount = 0;
        public int indexCount = 0;
        int numVertices;



        ///public CubeSpacePathHandler pathHandler;//TEMP

        public BoundingBox boundingBox;



        public PaintedCubeSpace()
        {

            resetSpace();

        }

        void resetSpace()
        {
            array = new byte[1, 1, 1];
            Random rand = new Random();


            loc = new Vector3();

            spaceHeight = 1;
            spaceWidth = 1;
            boundingBox = new BoundingBox(loc, loc + new Vector3(spaceWidth, spaceHeight, spaceWidth));
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int y = 0; y < spaceHeight; y++)
                {
                    for (int z = 0; z < spaceWidth; z++)
                    {
                        if (y > 0)
                        {
                            array[x, y, z] = 0;
                        }
                        else
                        {
                            array[x, y, z] = 5;
                        }
                        array[x, y, z] = 5;
                    }
                }
            }
        }

        public void draw(GraphicsDevice device, Effect effect)
        {
            
            effect.Parameters["xWorld"].SetValue(getMatrix());
            VertexBuffer thisVertexBuffer = vertexBuffer;
            IndexBuffer thisIndexBuffer = indexBuffer;
            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //NO OLD BAD WRONG//world.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / (3), VertexPositionNormalTexture.VertexDeclaration);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                    vertexBuffer.VertexCount, 0,
                    indexBuffer.IndexCount / 3);

            }
            //effect.Parameters["xWorld"].SetValue(Matrix.Identity);
        }

        public void draw(GraphicsDevice device, Effect effect, Matrix superMatrix, Quaternion rotation, bool highLighted)
        {
            VertexBuffer thisVertexBuffer;
            IndexBuffer thisIndexBuffer;
            RasterizerState rasterizerState;

            if (highLighted)
            {
                effect.Parameters["xAmbient"].SetValue(7.6f);
                rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                device.RasterizerState = rasterizerState;
                effect.Parameters["xWorld"].SetValue(getMatrix(superMatrix, rotation));
                thisVertexBuffer = vertexBuffer;
                thisIndexBuffer = indexBuffer;
                device.Indices = indexBuffer;
                device.SetVertexBuffer(vertexBuffer);
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    //NO OLD BAD WRONG//world.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / (3), VertexPositionNormalTexture.VertexDeclaration);
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                        vertexBuffer.VertexCount, 0,
                        indexBuffer.IndexCount / 3);

                }
            }
            rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.Solid;
            device.RasterizerState = rasterizerState;

            effect.Parameters["xAmbient"].SetValue(.6f);
            effect.Parameters["xWorld"].SetValue(getMatrix(superMatrix, rotation));
            thisVertexBuffer = vertexBuffer;
            thisIndexBuffer = indexBuffer;
            device.Indices = indexBuffer;
            device.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //NO OLD BAD WRONG//world.device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / (3), VertexPositionNormalTexture.VertexDeclaration);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                    vertexBuffer.VertexCount, 0,
                    indexBuffer.IndexCount / 3);

            }
            //effect.Parameters["xWorld"].SetValue(Matrix.Identity);
        }


        public bool isTransparentAt(int x, int y, int z)
        {
            if (withinSpace(new Vector3(x, y, z)))
            {
                return array[x, y, z] == AIR;
            }
            return false;
        }
        
        public bool canBeDrawn()
        {
            if (!readyToBeDisplayed && vertexBuffer != null && indexBuffer != null)
            {
                return false;
            }
            return true;
        }

        public void createModel(GraphicsDevice device)
        {

            //readyToBeDisplayed = false; // When the meshingThread comes through and sees this has been set to false, it executes this function
            //even though it's alreasy being executed in the main thread( IF this is a priority chunk(being set by the player))


            numVertices = 0;
            //vertices = null;
            //indices = null;
            //indexBuffer = null;
            //vertexBuffer = null; // setting these to null seemed to be causing threading problems.  The meshes are set to null in this thread
            //  then they are drawn while null.
            List<VertexPostitionColorPaintNormal> tempVertexList = new List<VertexPostitionColorPaintNormal>(1024);
            List<int> tempIntList = new List<int>(100);


            int numIndices = 0;
            byte tempNumFaces = 0;
            bool[] coveredArray;
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int z = 0; z < spaceWidth; z++)
                {
                    for (int y = 0; y < spaceHeight; y++)
                    {
                        if (array[x, y, z] != 0)
                        {
                            coveredArray = getTypeAt(x, y, z).updateOnGeometryModification(x, y, z, this);

                            if (!coveredArray[0]) //if block is not covered
                            {
                                for (int i = 1; i < 7; i++)
                                {
                                    if (coveredArray[i])
                                    {
                                        tempNumFaces++;
                                    }
                                }
                            }
                            numVertices += tempNumFaces * 4;
                            numIndices += tempNumFaces * 6;

                            tempNumFaces = 0;

                            getTypeAt(x, y, z).draw(this, tempIntList, tempVertexList, new Vector3(x, y, z), coveredArray,
                                cornersCoveredAlong(x, y, z, coveredArray[0]));
                        }


                    }
                }
            }

            copyFromTempIntAndVertexListIntoArrays(tempIntList, tempVertexList);
            indexCount = 0;
            vertexCount = 0;
            if (vertices.Length == 0)
            {
                return;
            }
            copyToBuffers(device);

            readyToBeDisplayed = true;



        }

        public void copyFromTempIntAndVertexListIntoArrays(List<int> tempIntList, List<VertexPostitionColorPaintNormal> tempVertexList)
        {
            vertices = new VertexPostitionColorPaintNormal[tempVertexList.Count];
            indices = new short[tempIntList.Count];

            for (int i = 0; i < tempIntList.Count; i++)
            {
                indices[i] = (short)tempIntList[i];
            }


            for (int i = 0; i < tempVertexList.Count; i++)
            {
                vertices[i] = tempVertexList[i];

            }
        }

        public enum AxisDirection : byte
        {
            pX, nX, pY, nY, pZ, nZ
        }

        public void createModelEXPERIMENTAL(GraphicsDevice device)
        {

            //readyToBeDisplayed = false; // When the meshingThread comes through and sees this has been set to false, it executes this function
            //even though it's alreasy being executed in the main thread( IF this is a priority chunk(being set by the player))

            // x and z imposed faces pass
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int z = 0; z < spaceWidth; z++)
                {

                    //panels are labeled by organized direction
                    CubeSurfacePanel? pXPanel= null;
                    CubeSurfacePanel? nXPanel = null;
                    CubeSurfacePanel? pZPanel = null;
                    CubeSurfacePanel? nZPanel = null;

                    for (int y = 0; y < spaceHeight; y++)
                    {
                        if (!withinSpace(x+1,y,z) || isTransparentAt(x+1, y, z)) //TODO: optimize away within check
                        {
                            if (pXPanel == null)
                            {
                                pXPanel = new CubeSurfacePanel(new IntVector3(x, y, z), new IntVector3 (0,0,0), array[x,y,z], AxisDirection.pX);

                            }
                        }
                    }
                }
            }




            numVertices = 0;
            //vertices = null;
            //indices = null;
            //indexBuffer = null;
            //vertexBuffer = null; // setting these to null seemed to be causing threading problems.  The meshes are set to null in this thread
            //  then they are drawn while null.
            List<VertexPostitionColorPaintNormal> tempVertexList = new List<VertexPostitionColorPaintNormal>(1024);
            List<int> intList = new List<int>(1536);


            int numIndices = 0;
            byte tempNumFaces = 0;
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int z = 0; z < spaceWidth; z++)
                {
                    for (int y = 0; y < spaceHeight; y++)
                    {
                        if (array[x, y, z] != 0)
                        {
                            bool[] coveredArray = getTypeAt(x, y, z).updateOnGeometryModification(x, y, z, this);

                            if (!coveredArray[0]) //if block is not covered
                            {
                                for (int i = 1; i < 7; i++)
                                {
                                    if (coveredArray[i])
                                    {
                                        tempNumFaces++;
                                    }
                                }
                            }
                            numVertices += tempNumFaces * 4;
                            numIndices += tempNumFaces * 6;

                            tempNumFaces = 0;

                            getTypeAt(x, y, z).draw(this, intList, tempVertexList, new Vector3(x, y, z), coveredArray,
                                cornersCoveredAlong(x, y, z, coveredArray[0]));
                        }


                    }
                }
            }
            vertices = new VertexPostitionColorPaintNormal[tempVertexList.Count];
            indices = new short[intList.Count];

            for (int i = 0; i < intList.Count; i++)
            {
                indices[i] = (short)intList[i];
            }


            for (int i = 0; i < tempVertexList.Count; i++)
            {
                vertices[i] = tempVertexList[i];

            }
            outputSCII_STL(vertices, intList);
            indexCount = 0;
            vertexCount = 0;
            if (vertices.Length == 0)
            {
                resetSpace();
                createModel(device);
                return;
            }
            copyToBuffers(device);

            //pathHandler.updateAfterChunkChange();//TEMP
            readyToBeDisplayed = true;



        }

        public void outputSCII_STL(VertexPostitionColorPaintNormal[] vertices, List<int> indices)
        {
            string path = "C:/Users/Public/CubeStudio/output";
            if (new FileInfo(path).Extension != ".stl")
            {
                path += ".stl";
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("solid cube_corner");
                int toTwo = 0;
                foreach (int i in indices)
                {
                    if (toTwo == 0)
                    {
                        sw.WriteLine("  facet normal " + vertices[i].Normal.X + " " + vertices[i].Normal.Y + " " + -vertices[i].Normal.Z);
                        sw.WriteLine("    outer loop");
                    }
                    toTwo++;
                    sw.WriteLine("      vertex " + vertices[i].Position.X + " " + vertices[i].Position.Y + " " + -vertices[i].Position.Z);
                    if (toTwo == 3)
                    {
                        sw.WriteLine("    endloop");
                        sw.WriteLine("  endfacet");
                        toTwo = 0;
                    }
                }
                sw.WriteLine("endsolid");
            }
        }

        public Block getTypeAt(int x, int y, int z)
        {
            return new Block(ColorPallete.colorArray[array[x, y, z]]);
        }


        public void serializeChunk(string savePath)
        {


            //Opens a file and serializes the object into it in binary format.

            if (new FileInfo(savePath).Extension != ".vox")
            {
                savePath += ".vox";
            }

            Stream stream = File.Open(savePath, FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, array);
            stream.Close();
            serializedAfterLastChange = true;


            MainWindow.singleton.updateFileTreeView();

        }

        public byte[] compressedArray()
        {

            List<byte> numberList = new List<byte>();   // numBlocks, blockType, numBLocks, blockType

            for (int z = 0; z < spaceWidth; z++)
            {
                for (int x = 0; x < spaceWidth; x++)
                {
                    byte numCubesOfSameTypeSoFar = 0;

                    byte currentType = array[x, 0, z];

                    for (int y = 0; y < spaceHeight; y++)
                    {
                        if (array[x, y, z] == (byte)currentType)
                        {

                            numCubesOfSameTypeSoFar++;
                        }
                        if (y + 1 >= spaceHeight || array[x, y, z] != (byte)currentType || numCubesOfSameTypeSoFar > 250) //put the current numbers into the array  
                        {
                            numberList.Add((byte)(numCubesOfSameTypeSoFar));
                            numberList.Add((byte)currentType);
                            numCubesOfSameTypeSoFar = 1; //0?
                            currentType = array[x, y, z];
                        }
                    }

                }

            }
            byte[] result = numberList.ToArray();



            return result;
        }

        public void decompressArrayAndSetArray(byte[] input)
        {
            int numBlocksDone = 0;
            int currentSpotInArray = 0;
            for (int z = 0; z < spaceWidth; z++)
            {
                for (int x = 0; x < spaceWidth; x++)
                {
                    for (int y = 0; y < spaceHeight; y++)
                    {
                        numBlocksDone++;
                        array[x, y, z] = input[currentSpotInArray + 1];
                        input[currentSpotInArray]--;
                        if (input[currentSpotInArray] == 0)
                        {
                            currentSpotInArray += 2;
                        }
                    }
                }
            }
        }

        public void comeIntoView(GraphicsDevice device)
        {
            createModel(device);
            readyToBeDisplayed = true;
        }

        public void flipSpace()
        {
            byte[, ,] newArray = new byte[spaceWidth, spaceHeight, spaceWidth];
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int y = 0; y < spaceHeight; y++)
                {
                    for (int z = 0; z < spaceWidth; z++)
                    {
                        Vector3 flipped = flippedLoc(x, y, z);

                        newArray[(int)flipped.X, (int)flipped.Y, (int)flipped.Z] = array[x,y,z];
                        
                    }
                }
            }

            array = newArray;
        }

        public void moveWork(Vector3 move)
        {
            //expands whole array
            byte[, ,] newArray = new byte[spaceWidth + 2, spaceHeight + 2, spaceWidth + 2];
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int y = 0; y < spaceHeight; y++)
                {
                    for (int z = 0; z < spaceWidth; z++)
                    {
                        if (withinSpace(move + new Vector3(x, y, z)))
                        {
                            newArray[x + 1 + (int)move.X, y + 1 + (int)move.Y, z + 1 + (int)move.Z] = array[x, y, z];
                        }
                    }
                }
            }
            spaceWidth += 2;
            spaceHeight += 2;
            array = newArray;


        }

        private void copyToBuffers(GraphicsDevice device)
        {
            vertexBuffer = new VertexBuffer(device, VertexPostitionColorPaintNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPostitionColorPaintNormal>(vertices);
            indexBuffer = new IndexBuffer(device, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public void buildRect(Vector3 loc1, Vector3 loc2, byte type)
        {
            float temp;
            if (loc1.X > loc2.X)
            {
                temp = loc1.X;
                loc1.X = loc2.X;
                loc2.X = temp;
            }
            if (loc1.Y > loc2.Y)
            {
                temp = loc1.Y;
                loc1.Y = loc2.Y;
                loc2.Y = temp;
            }
            if (loc1.Z > loc2.Z)
            {
                temp = loc1.Z;
                loc1.Z = loc2.Z;
                loc2.Z = temp;
            }
            for (int x = (int)loc1.X; x < (int)loc2.X; x++)
            {
                for (int y = (int)loc1.Y; y < (int)loc2.Y; y++)
                {
                    for (int z = (int)loc1.Z; z < (int)loc2.Z; z++)
                    {
                        Vector3 locToPut = Vector3.Transform(new Vector3(x, y, z), putPointIntoSpaceContext());
                        while (!withinSpace(locToPut))
                        {
                            expandArray();
                        }
                        if (withinSpace(locToPut))
                        {
                            array[(int)locToPut.X, (int)locToPut.Y, (int)locToPut.Z] = type;
                        }
                        else
                        {
                        }
                    }
                }

            }
        }

        public bool destroyBlockFromWorldSpace(Vector3 firstRef, Vector3 secondRef, bool mirror)
        {
            Vector3? blockLocMaybe = rayTileHitsViaModerna(true, float.NaN, array, Vector3.Transform(firstRef, 
                putPointIntoSpaceContext()), Vector3.Transform(secondRef, putPointIntoSpaceContext()));

            if (!blockLocMaybe.HasValue)
            {
                return false;
            }
            Vector3 blockLoc = (Vector3)blockLocMaybe;

            

            array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z] = 0;
            if (mirror)
            {
                mirrorBlockPlace((int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z, 0);
            }
            return true;
            
        }

        public byte? getBlockFromWorldSpace(Vector3 firstRef, Vector3 secondRef)
        {
            Vector3? blockLocMaybe = rayTileHitsViaModerna(true, float.NaN, array, Vector3.Transform(firstRef,
                putPointIntoSpaceContext()), Vector3.Transform(secondRef, putPointIntoSpaceContext()));

            if (!blockLocMaybe.HasValue)
            {
                return null;
            }
            Vector3 blockLoc = (Vector3)blockLocMaybe;



            return array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z];

        }


        public bool placeBlockFromWorldSpace(Vector3 firstRef, Vector3 secondRef, byte toPlace, bool mirror)
        {
            Vector3? blockLocMaybe = rayTileHitsViaModerna(false, float.NaN, array, Vector3.Transform(firstRef,
                putPointIntoSpaceContext()), Vector3.Transform(secondRef, putPointIntoSpaceContext()));

            if (!blockLocMaybe.HasValue)
            {
                return false;
            }
            Vector3 blockLoc = (Vector3)blockLocMaybe;
            if (!withinSpace(blockLoc))
            {
                expandArray();
                placeBlockFromWorldSpace(firstRef, secondRef, toPlace, mirror);
                return true;
            }
            array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z] = toPlace;
            if (mirror)
            {
                mirrorBlockPlace((int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z, toPlace);
            }
            return true;
        }

        void mirrorBlockPlace(int x, int y, int z, byte toPlace)
        {
            float middle =((float)spaceWidth / 2.0f);
            if (x < middle)
            {
                array[spaceWidth-x-1, y, z] = toPlace;
            }
            if (x >= middle)
            {
                array[ (int)(middle-(x-middle+1)) , y, z] = toPlace;
            }
        }

        Vector3 flippedLoc(int x, int y, int z)
        {
            float middle = ((float)spaceWidth / 2.0f);
            if (x < middle)
            {
                return new Vector3(spaceWidth - x - 1, y, z);
            }
            else
            {
                return new Vector3((int)(middle - (x - middle + 1)), y, z);
            }
        }

        Vector3 mirrorVec(Vector3 test)
        {
            Vector3 result = new Vector3(test.X,test.Y,test.Z);
            float middle = ((float)spaceWidth / 2.0f);
            if (test.X < middle)
            {
                result.X = spaceWidth - test.X - 1;
            }
            if (test.X >= middle)
            {
                result.X = (int)(middle - (test.X - middle + 1));
            }
            return result;
        }

        public Matrix getMatrix()
        {
            loc = getOffset();
            Vector3 offset = getOffset();

            return Matrix.CreateTranslation(-offset)   * Matrix.CreateTranslation(loc)
                * Matrix.CreateTranslation(offset);
        }

        public Matrix getMatrix(Matrix superMatrix, Quaternion rotation)
        {
            Vector3 offset = getOffset();


            return (Matrix.CreateTranslation(offset) * Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(-offset)
                * Matrix.CreateTranslation(offset) * Matrix.CreateTranslation(loc)) * superMatrix;
        }

        Vector3 getOffset()
        {
            return new Vector3(-(float)spaceWidth / 2.0f,
               -(float)spaceHeight / 2.0f, -(float)spaceWidth / 2.0f);
        }

        Matrix putPointIntoSpaceContext(Matrix superMatrix, Quaternion rotation)
        {
            return Matrix.Invert(getMatrix(superMatrix, rotation));
        }

        Matrix putPointIntoSpaceContext()
        {
            return Matrix.Invert(getMatrix());
        }

        public bool withinSpace(Vector3 test)  // does not account for space loc
        {
            if (
             test.X < spaceWidth && test.X >= 0
                && test.Z < spaceWidth && test.Z >= 0
                && test.Y < spaceHeight && test.Y >= 0)
            {
                return true;
            }
            return false;
        }

        public bool withinSpace(int x, int y, int z)  // does not account for space loc
        {
            if (
             x < spaceWidth && x >= 0
                && z < spaceWidth && z >= 0
                && y < spaceHeight && y >= 0)
            {
                return true;
            }
            return false;
        }

        public Vector3? rayTileHitsViaModerna(bool wantNearestHit, float range, byte[, ,] array, Vector3 firstRef, Vector3 secondRef)
        {

            List<Vector3> intersected;
            intersected = new List<Vector3>();

            int xMin, xMax, yMin, yMax, zMin, zMax;
            if (!System.Single.IsNaN(range))
            {
                xMin = (int)(firstRef.X - range);
                xMax = (int)(firstRef.X + range);
                yMin = (int)(firstRef.Y - range);
                yMax = (int)(firstRef.Y + range);
                zMin = (int)(firstRef.Z - range);
                zMax = (int)(firstRef.Z + range);

            }
            else //it's Nan
            {
                xMin = 0;
                xMax = spaceWidth;
                yMin = 0;
                yMax = spaceHeight;
                zMin = 0;
                zMax = spaceWidth;
            }

            for (int a = xMin; a < xMax; a += 10)
            {
                for (int b = yMin; b < yMax; b += 10)
                {
                    for (int c = zMin; c < zMax; c += 10)
                    {
                        if (withinSpace(new Vector3(a, b, c)))
                        {


                            Vector3 wat = new Vector3(0, 0, 0);
                            if (GeometryFunctions.CheckLineBox(new Vector3(a, b, c), new Vector3(a + 10, b + 10, c + 10), secondRef, firstRef, ref wat))
                            {
                                for (int x = a; x < a + 10; x++)
                                {
                                    for (int y = b; y < b + 10; y++)
                                    {
                                        for (int z = c; z < c + 10; z++)
                                        {
                                            if (withinSpace(new Vector3(x, y, z)))
                                            {
                                                Vector3 lul = new Vector3(0, 0, 0);
                                                if (GeometryFunctions.CheckLineBox(new Vector3(x, y, z), new Vector3(x + 1, y + 1, z + 1), secondRef, firstRef, ref lul))
                                                {
                                                    intersected.Add(new Vector3(x, y, z));
                                                    
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            float minDist = 99999999999;
            List<float> distances = new List<float>();
            Vector3 closest = new Vector3();
            Vector3 secondClosest = new Vector3();
            bool foundOne = false;
            for (int i = 0; i < intersected.Count; i++)
            {

                Vector3 test = intersected[i];
                if(array[(int)test.X, (int)test.Y, (int)test.Z] != 0)//if (getTypeAt((int)test.X, (int)test.Y, (int)test.Z).transLoS == false)
                {
                    Vector3 dist = firstRef - test;
                    float length = (float)Math.Sqrt((firstRef.X - test.X) * (firstRef.X - test.X) + (firstRef.Y - test.Y) * (firstRef.Y - test.Y) + (firstRef.Z - test.Z) * (firstRef.Z - test.Z));
                    distances.Add(length);

                    if ((float)length < (float)minDist)
                    {
                        minDist = length;
                        if (foundOne)
                        {

                        }
                        foundOne = true;
                        closest = test;
                    }


                }
            }


           // Vector3 intersectionPoint = new Vector3();

            BoundingBox box = new BoundingBox(new Vector3(closest.X, closest.Y, closest.Z), new Vector3(closest.X + 1, closest.Y + 1, closest.Z + 1));
            Ray ray = new Ray(firstRef, Vector3.Normalize(secondRef - firstRef));
            
            float? distanceToHitMaybe = box.Intersects(ray);
            if (!distanceToHitMaybe.HasValue)
            {
                return null;
            }
            float distanceToHit = (float)distanceToHitMaybe;
            
           // Vector3 intersectionPoint = ray.Direction * distanceToHit + firstRef;

            //if (GeometryFunctions.CheckLineBox(new Vector3(closest.X, closest.Y, closest.Z), new Vector3(closest.X + 1, closest.Y + 1, closest.Z + 1), secondRef, firstRef, ref intersectionPoint))
            //{
           // }

            //float distaceToHit = (firstRef - intersectionPoint).Length();
            float backup = .0001f;
            Vector3 slightlyCloser = ray.Direction * (distanceToHit-backup) + firstRef;

            //Boolean ableToPlace = false;

            //secondClosest = array[(int)slightlyCloser.X, (int)slightlyCloser.Y, (int)slightlyCloser.Z];
            
            secondClosest = slightlyCloser;
            if (foundOne)
            {
                if (withinSpace(closest))
                {

                }

            }

            if (wantNearestHit)
            {
                return closest;
            }
            else
            {
                return secondClosest;
            }

        }

        public Vector3? getNearestHitInSpaceContext(Vector3 firstRef, Vector3 secondRef, Matrix superMatrix, Quaternion rotation)
        {
            return rayTileHitsViaModerna(true, float.NaN, array, Vector3.Transform(firstRef,
                 putPointIntoSpaceContext(superMatrix, rotation)), Vector3.Transform(secondRef, putPointIntoSpaceContext(superMatrix, rotation)));
        }

        public enum cornerToAOArrayLoc
        {
            XlYhZh = 0,
            XlYhZl = 1,
            XlYlZh = 2,
            XlYlZl = 3,
            XhYhZh = 4,
            XhYhZl = 5,
            XhYlZh = 6,
            XhYlZl = 7

        }

        static byte lowAOvalue = 60;
        static byte highAOvalue = 60;

        public byte[][] cornersCoveredAlong(int x, int y, int z, bool blockIsCoverd)
        {
            //these are in array order
            byte[] resultX = new byte[8];
            byte[] resultY = new byte[8];
            byte[] resultZ = new byte[8];

            if (blockIsCoverd)
            {
                return new byte[][] { resultX, resultY, resultZ };
            }


            //byte XlYhZh = 0;//0
            //byte XlYhZl = 0;//1
            //byte XlYlZh = 0;//2
            //byte XlYlZl = 0;//3
            //byte XhYhZh = 0;//4
            //byte XhYhZl = 0;//5
            //byte XhYlZh = 0;//6
            //byte XhYlZl = 0;//7


            //low Y
            if (isOpaqueAtFromWithinChunkContext(x, y - 1, z - 1))
            {
                //XlYlZl++;
                //XhYlZl++;
                resultZ[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y - 1, z))
            {
                //XlYlZl++;
                //XlYlZh++;
                resultX[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x, y - 1, z + 1))
            {
                //XlYlZh++;
                //XhYlZh++;
                resultY[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;

            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y - 1, z))
            {
                //XhYlZh++;
                //XhYlZl++;
                resultY[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
            }
            //mid Y
            if (isOpaqueAtFromWithinChunkContext(x - 1, y, z - 1))
            {
                //XlYlZl++;
                //XlYhZl++;
                resultX[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYlZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y, z + 1))
            {
                //XlYlZh++;
                //XlYhZh++;
                resultX[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYlZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y, z + 1))
            {
                //XhYhZh++;
                //XhYlZh++;
                resultX[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZh] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y, z - 1))
            {
                //XhYhZl++;
                //XhYlZl++;
                resultX[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZl] += highAOvalue;
            }
            //high Y
            if (isOpaqueAtFromWithinChunkContext(x, y + 1, z - 1))
            {
                //XlYhZl++;
                //XhYhZl++;
                resultY[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y + 1, z))
            {
                //XlYhZl++;
                //XlYhZh++;
                resultX[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYhZl] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
            }


            if (isOpaqueAtFromWithinChunkContext(x, y + 1, z + 1))
            {
                //XlYhZh++;
                //XhYhZh++;
                resultY[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZh] += highAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;

            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y + 1, z))
            {
                //XhYhZh++;
                //XhYhZl++;
                resultY[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYhZh] += highAOvalue;
                resultX[(int)cornerToAOArrayLoc.XhYhZl] += highAOvalue;
            }

            //===== corners!

            //high Y corners

            if (isOpaqueAtFromWithinChunkContext(x + 1, y + 1, z + 1))
            {
                resultX[(int)cornerToAOArrayLoc.XhYhZh] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYhZh] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZh] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y + 1, z + 1))
            {
                resultX[(int)cornerToAOArrayLoc.XlYhZh] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYhZh] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZh] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y + 1, z - 1))
            {
                resultX[(int)cornerToAOArrayLoc.XhYhZl] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYhZl] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYhZl] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y + 1, z - 1))
            {
                resultX[(int)cornerToAOArrayLoc.XlYhZl] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYhZl] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYhZl] += lowAOvalue;
            }

            // low Y corners
            if (isOpaqueAtFromWithinChunkContext(x + 1, y - 1, z + 1))
            {
                resultX[(int)cornerToAOArrayLoc.XhYlZh] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYlZh] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZh] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y - 1, z + 1))
            {
                resultX[(int)cornerToAOArrayLoc.XlYlZh] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYlZh] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYlZh] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x + 1, y - 1, z - 1))
            {
                resultX[(int)cornerToAOArrayLoc.XhYlZl] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XhYlZl] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XhYlZl] += lowAOvalue;
            }
            if (isOpaqueAtFromWithinChunkContext(x - 1, y - 1, z - 1))
            {
                resultX[(int)cornerToAOArrayLoc.XlYlZl] += lowAOvalue;
                resultY[(int)cornerToAOArrayLoc.XlYlZl] += lowAOvalue;
                resultZ[(int)cornerToAOArrayLoc.XlYlZl] += lowAOvalue;
            }

            return new byte[][] { resultX, resultY, resultZ };
        }

        public bool isOpaqueAtFromWithinChunkContext(int x, int y, int z)
        {
            if (withinSpace(new Vector3(x, y, z)))
            {

                return !isTransparentAt(x, y, z);
            }
            else if (y >= spaceHeight || y < 0)
            {
                return false;
            }
            return false;
        }

        void expandArray()
        {
            byte[,,] newArray = new byte[spaceWidth+2,spaceHeight+2,spaceWidth+2];
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int y = 0; y < spaceWidth; y++)
                {
                    for (int z = 0; z < spaceWidth; z++)
                    {
                        newArray[x + 1, y + 1, z + 1] = array[x, y, z];
                    }
                }
            }
            spaceWidth+=2;
            spaceHeight += 2;
            array = newArray;
        }

        public void replaceArrayWith(byte[, ,] newArray)
        {
            array = newArray;
            spaceWidth = newArray.GetLength(0);
            spaceHeight = newArray.GetLength(1);
        }

        void fill(Vector3 startLoc, byte colorFrom, byte colorTo)
        {
            if (array[(int)startLoc.X, (int)startLoc.Y, (int)startLoc.Z] == colorTo)
            {
                return;
            }
            List<Vector3> openNodes = getAdjacentBlocks((int)startLoc.X, (int)startLoc.Y, (int)startLoc.Z);
            HashSet<Vector3> visitedNodes = new HashSet<Vector3>();
            array[(int)startLoc.X, (int)startLoc.Y, (int)startLoc.Z] = colorTo;
            removeAllNotOfColor(colorFrom, openNodes);
            foreach (Vector3 visted in openNodes)
            {
                visitedNodes.Add(visted);
            }
            while (openNodes.Count > 0)
            {

                List<Vector3> newOpenNodes = new List<Vector3>();
                foreach (Vector3 from in openNodes) 
                {
                    array[(int)from.X, (int)from.Y, (int)from.Z] = colorTo;
                    List<Vector3> potentialNextSteps = getAdjacentBlocks((int)from.X, (int)from.Y, (int)from.Z);
                    List<Vector3> nextSteps = new List<Vector3>();
                    foreach (Vector3 test in potentialNextSteps)
                    {
                        if(!visitedNodes.Contains(test))
                        {
                            nextSteps.Add(test);
                            visitedNodes.Add(test);
                        }
                    }
                    removeAllNotOfColor(colorFrom, nextSteps);
                    newOpenNodes.AddRange(nextSteps);
                    
                }
                openNodes = newOpenNodes;
            }
        }

        public byte fillFromWorldContext(Vector3 firstRef, Vector3 secondRef, byte color, bool mirror)
        {

            Vector3? blockLocMaybe = rayTileHitsViaModerna(true, float.NaN, array, Vector3.Transform(firstRef,
                putPointIntoSpaceContext()), Vector3.Transform(secondRef, putPointIntoSpaceContext()));

            if (!blockLocMaybe.HasValue)
            {
                return 0;
            }
            Vector3 blockLoc = (Vector3)blockLocMaybe;
            //if (!withinSpace(blockLoc))
            //{
            //expandArray();
            fill(blockLoc, array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z], color);
            if (mirror)
            {
                fill(mirrorVec(blockLoc), array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z], color);
            }

            //return;
            //}
            return array[(int)blockLoc.X, (int)blockLoc.Y, (int)blockLoc.Z];
        }

        void removeAllNotOfColor(byte color, List<Vector3> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {

                if (array[(int)list[i].X, (int)list[i].Y, (int)list[i].Z] != color)
                {
                    list.RemoveAt(i);

                }
            }
        }

        List<Vector3> getAdjacentBlocks(int x, int y, int z)
        {
            List<Vector3> result = new List<Vector3>();
            List<Vector3> directions = new List<Vector3>();
            directions.Add(new Vector3(1, 0, 0));
            directions.Add(new Vector3(-1, 0, 0));
            directions.Add(new Vector3(0, 1, 0));
            directions.Add(new Vector3(0, -1, 0));
            directions.Add(new Vector3(0, 0, 1));
            directions.Add(new Vector3(0, 0, -1));

            foreach (Vector3 test in directions)
            {
                if(withinSpace( new Vector3(x,y,z)+test))
                {
                   
                    result.Add(new Vector3(x, y, z) + test);
                }
            }

            return result;
        }

        public void upgradeColors()
        {
            for (int x = 0; x < spaceWidth; x++)
            {
                for (int y = 0; y < spaceWidth; y++)
                {
                    for (int z = 0; z < spaceWidth; z++)
                    {
                        array[x, y, z] = ColorPallete.getNewByteFromOldByte(array[x, y, z]);
                    }
                }
            }
        }
    }
}
