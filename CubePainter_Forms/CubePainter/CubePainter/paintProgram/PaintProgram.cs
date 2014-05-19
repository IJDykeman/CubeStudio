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
using System.Windows.Forms;

using CubeStudio;

namespace CubePainter
{
    class PaintProgram
    {

        Painter player;
        //public static string saveFileName = "paintings";
        PaintedCubeSpace paintedCubeSpace;
        List<SavedSpace> savedSpaces;
        int timeSinceLastSaveableAction = 100;//set to negative one when nothing has been done yet
        public static int width;
        public static int height;
        List<Action> UIActionQueue;

        public PaintProgram(GraphicsDevice device)
        {

            player = new Painter();
            paintedCubeSpace = new PaintedCubeSpace();
            savedSpaces = new List<SavedSpace>();

            Compositer.construct(device);

        }

        public void loadContent(ContentManager content)
        {
            Compositer.LoadContent(content);
            ColorPallete.loadContent();
            paintedCubeSpace.createModel(Compositer.device);
            paintedCubeSpace.readyToBeDisplayed = true;
            player.loadContent(content);
            UIActionQueue = new List<Action>();
            
        }


        public void display()
        {
            Compositer.drawFinalImage(player, paintedCubeSpace, width, height, (int)PainterMain.mouseLocationInXNASpace.X, (int)PainterMain.mouseLocationInXNASpace.Y);
        }

        public void update()
        {

            try
            {
                List<Action> playerActions = player.update(width, height);
                playerActions.AddRange(UIActionQueue);

                handleActionList(playerActions);
                if (timeSinceLastSaveableAction >= 0)
                {
                    timeSinceLastSaveableAction++;
                }
                if (timeSinceLastSaveableAction > 3)
                {
                    saveToStack();
                    Console.WriteLine("stack size is " + savedSpaces.Count);

                }
                UIActionQueue.Clear();
            }
            catch (Exception e)
            {
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e);
                
                Console.WriteLine(trace.ToString());
            }


        }

        void saveToStack()
        {
            Console.WriteLine("saving");
            if (savedSpaces.Count>0 && savedSpaces[0].array.GetHashCode() == paintedCubeSpace.array.GetHashCode())
            {
             //   return;
            }
            savedSpaces.Insert(0, new SavedSpace(new byte[paintedCubeSpace.spaceWidth, paintedCubeSpace.spaceHeight, paintedCubeSpace.spaceWidth]
                , paintedCubeSpace.spaceWidth, paintedCubeSpace.spaceHeight));
            for (int x = 0; x < paintedCubeSpace.spaceWidth; x++)
            {
                for (int y = 0; y < paintedCubeSpace.spaceHeight; y++)
                {
                    for (int z = 0; z < paintedCubeSpace.spaceWidth; z++)
                    {
                        savedSpaces[0].array[x, y, z] = paintedCubeSpace.array[x, y, z];
                    }
                }
            }
            timeSinceLastSaveableAction = -1;
        }

        void handleActionList(List<Action> list)
        {
            foreach (Action test in list)
            {
                handleAction(test);
            }
        }

        void handleAction(Action action)
        {
            
            switch (action.type)
            {
                case actionType.playerLeftClick:
                    PlayerLeftClickAction leftClick = (PlayerLeftClickAction) action;
                    leftClick.successful = paintedCubeSpace.destroyBlockFromWorldSpace(leftClick.firstRef, leftClick.secondRef,leftClick.mirror);
                    paintedCubeSpace.createModel(Compositer.device);
                    registerSaveableAction();
                    break;
                case actionType.playerRightClick:
                    PlayerRightClickAction rightClick = (PlayerRightClickAction)action;
                    rightClick.successful = paintedCubeSpace.placeBlockFromWorldSpace(rightClick.firstRef, rightClick.secondRef, player.selectedBlock, rightClick.mirror);
                    paintedCubeSpace.createModel(Compositer.device);
                    registerSaveableAction();
                    break;
                case actionType.playerAltClick:
                    PlayerAltClickAction altClick = (PlayerAltClickAction)action;
                    altClick.originalColor = paintedCubeSpace.fillFromWorldContext(altClick.firstRef, altClick.secondRef, player.selectedBlock, altClick.mirror);
                    paintedCubeSpace.createModel(Compositer.device);
                    registerSaveableAction();
                    break;
                case actionType.playerShiftLeftClick:
                    PlayerShiftLeftClickAction shiftLeftClick = (PlayerShiftLeftClickAction)action;
                    byte? test = paintedCubeSpace.getBlockFromWorldSpace(shiftLeftClick.firstRef, shiftLeftClick.secondRef);
                    if (test.HasValue)
                    {
                        player.selectedBlock = (byte)test;
                    }
                    break;
                case actionType.playerUndo:
                    undo();
 
                    break;
                case actionType.moveSpace:
                    MoveWorkAction move = (MoveWorkAction)action;
                    paintedCubeSpace.moveWork(move.move);
                    paintedCubeSpace.createModel(Compositer.device);


                    break;
                case actionType.flipSpace:
                    paintedCubeSpace.flipSpace();
                    paintedCubeSpace.createModel(Compositer.device);


                    break;
                case actionType.buildRect:
                    BuildRectAction build = (BuildRectAction)action;
                    paintedCubeSpace.buildRect(build.loc1, build.loc2, player.selectedBlock);
                    paintedCubeSpace.createModel(Compositer.device);
                    registerSaveableAction();


                    break;
                case actionType.saveDocument:
                    Console.WriteLine("save event main");
                    PlayerSaveDocumentAction saveAction = (PlayerSaveDocumentAction)action;
                    paintedCubeSpace.serializeChunk(saveAction.path);
                    
            
                    break;
                case actionType.openDocument:
                    PlayerOpenDocumentAction loadAction = (PlayerOpenDocumentAction)action;
                    loadCubeSpace(loadAction.path);
                    break;
                case actionType.newDocument:
                    createNewCubeSpace();
                    break;
                case actionType.playerNeedsUndoSave:
                    saveToStack();
                    break;
                case actionType.quitProgram:
                    PainterMain.quitGame();
                    break;
                default:
                    throw new Exception("unhandled action");
                    

            }
        }

        void loadCubeSpace(string path)
        {
            try
            {
                Console.WriteLine(path);
                if (!File.Exists(path))
                {
                    //Microsoft.VisualBasic.Interaction.InputBox("File name invalid.  Please select a file name to open", "Open", "file name", 300, 300);
                    return;

                }

                FileInfo fileInfo = new FileInfo(path);



                long fileLength = fileInfo.Length;

                int newCubeSpaceWidth = (int)Math.Pow(fileLength, 1.0 / 3.0);
                int newCubeSpaceHeight = (int)Math.Pow(fileLength, 1.0 / 3.0);


                paintedCubeSpace.spaceWidth = newCubeSpaceWidth;
                paintedCubeSpace.spaceHeight = newCubeSpaceHeight;

                byte[, ,] obj = new byte[paintedCubeSpace.spaceWidth, paintedCubeSpace.spaceHeight, paintedCubeSpace.spaceWidth];

                //Opens file "data.xml" and deserializes the object from it.
                Stream stream = File.Open(path, FileMode.Open);


                BinaryFormatter formatter = new BinaryFormatter();

                //formatter = new BinaryFormatter();

                obj = (byte[, ,])formatter.Deserialize(stream);
                //bodypart.decompressArrayAndSetArray(obj);
                paintedCubeSpace.array = obj;
                paintedCubeSpace.createModel(Compositer.device);
                stream.Close();
            }
            catch
            {
                MessageBox.Show("Invalid model folder");
            }


        }

        void createNewCubeSpace()
        {
            paintedCubeSpace = new PaintedCubeSpace();
            paintedCubeSpace.createModel(Compositer.device);

        }

        void undo()
        {
            if (savedSpaces.Count <2)
            {
                Console.WriteLine("nowhere to back ro");
                return;
            }
            Console.WriteLine("ctrlsz and stack size is " + savedSpaces.Count);
            SavedSpace toLoad = savedSpaces[0];
            paintedCubeSpace.spaceWidth = toLoad.width;
            paintedCubeSpace.spaceHeight = toLoad.height;
            paintedCubeSpace.array = toLoad.array;
            paintedCubeSpace.createModel(Compositer.device);
            savedSpaces.RemoveAt(0);
            Console.WriteLine("afyer ctrlsz and stack size is now " + savedSpaces.Count);
        }

        void registerSaveableAction()
        {
            timeSinceLastSaveableAction = 0;
        }

        public void addUIAction(Action toAdd)
        {
            UIActionQueue.Add(toAdd);

        }
    }
}

