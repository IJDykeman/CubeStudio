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
using CubePainter;
namespace CubeAnimator
{
    class AnimationProgram
    {

        AnimationPlayer player;
        //public static string saveFileName = "paintings";

        BodyPart rootBodyPart;
        RequiredPartDescription rootDescription;
        bool placingPartCurrently=false;
        BodyPart currentlyPlacingPart;
        AnimatedCharacter character = null;
        RequiredPartDescription currentlyPlacingReq;
        public static int width;
        public static int height;
        List<Action> actionQueue;

        public enum AppState
        {
            buildingCharacter,
            admiringCharacter
        }
        public static AppState state = AppState.buildingCharacter;


        public AnimationProgram(GraphicsDevice ndevice)
        {

            player = new AnimationPlayer();
            actionQueue = new List<Action>();


            Compositer.construct(ndevice);

        }

        public void loadContent(ContentManager content)
        {
            Compositer.LoadContent(content);
            ColorPallete.loadContent();
            player.loadContent(content);

        }

        public void display()
        {
            Compositer.drawFinalImageFirst(player, state == AppState.admiringCharacter);
            if(state ==AppState.buildingCharacter && rootBodyPart != null){
                Compositer.drawPart(rootBodyPart);
            }
            else if(state == AppState.admiringCharacter)
            {
                Compositer.drawCharacter(character);

                
            }
            Compositer.drawFinalImageLast(player, rootBodyPart == null && character == null, character != null);
        }

        public void update()
        {
            
            if (state == AppState.buildingCharacter)
            {

            }
            else if(state == AppState.admiringCharacter)
            {
                character.update();
            }


            List<Action> playerActions = player.update(AnimationProgram.width, AnimationProgram.height, getRootPart()!= null && getRootPart().getSelectedPart()!=null);
            handleActionList(playerActions);


        }

        void handleActionList(List<Action> list)
        {
            foreach (Action test in list)
            {
                handleAction(test);
            }
            actionQueue.Clear();
        }

        void handleAction(Action action)
        {
            float moveAmount = .5f;
            switch (action.type)
            {
                case actionType.playerLeftClick:
                    PlayerLeftClickAction leftClick = (PlayerLeftClickAction) action;
                    if (currentlyPlacingPart != null)
                    {

                        BodyPartWithHitLoc? maybeHit = rootBodyPart.getNearestHit(leftClick.firstRef, leftClick.secondRef, Matrix.Identity, Quaternion.Identity);
                        if (maybeHit == null)
                        {
                            //Console.WriteLine("no hit");
                            break;
                        }
                        BodyPartWithHitLoc hit = (BodyPartWithHitLoc)maybeHit;
                        currentlyPlacingPart = new BodyPart(currentlyPlacingPart.model, hit.locInSpace + new Vector3(.5f, .5f, .5f), currentlyPlacingPart.type, currentlyPlacingPart.fileName);
                        //currentlyPlacingPart.model.loc = hit.locInSpace + new Vector3(.5f, .5f, .5f);
                        hit.part.children.Add(currentlyPlacingPart);
                        placingPartCurrently = false;
                        //Console.WriteLine("placed a piece");
                        currentlyPlacingPart = null;
                        updateMainWindowCharacterChart();
                    }
                    else
                    {
                        if (state == AppState.buildingCharacter)
                        {
                            playerClickToSelectPart(leftClick);
                        }
                    
                    }
                    break;
                case actionType.playerRightClick:
                    PlayerRightClickAction rightClick = (PlayerRightClickAction)action;

                    break;
                case actionType.playerAltClick:
                    PlayerAltClickAction altClick = (PlayerAltClickAction)action;

                    break;
                case actionType.playerShiftLeftClick:
                    PlayerShiftLeftClickAction shiftLeftClick = (PlayerShiftLeftClickAction)action;

                    break;
                case actionType.animateButtonClicked:
                    switchBetweenCharacterAndRoot();
                    break;
                case actionType.playerUndo:

 
                    break;
                case actionType.saveDocument:

                    
            
                    break;
                case actionType.openDocument:

                    break;
                case actionType.newDocument:

                    break;
                case actionType.ctrlC:
                    //convertToCharacter();
                    break;
                case actionType.scaleDownAction:
                    getRootPart().getSelectedPart().scale(-.1f);
                    break;
                case actionType.scaleUpButton:
                    getRootPart().getSelectedPart().scale(.1f);
                    break;
                
                case actionType.moveUpAction:
                    getRootPart().getSelectedPart().move(new Vector3(0, moveAmount, 0));
                    break;
                case actionType.moveDownAction:
                    getRootPart().getSelectedPart().move(new Vector3(0, -moveAmount, 0));
                    break;
                case actionType.moveLeftAction:
                    getRootPart().getSelectedPart().move(new Vector3(moveAmount, 0, 0));
                    break;
                case actionType.moveRightAction:
                    getRootPart().getSelectedPart().move(new Vector3(-moveAmount, 0, 0));
                    break;
                case actionType.moveForwardAction:
                    getRootPart().getSelectedPart().move(new Vector3(0, 0, moveAmount));
                    break;
                case actionType.moveBackAction:
                    getRootPart().getSelectedPart().move(new Vector3(0, 0, -moveAmount));
                    break;
                case actionType.rotateAction:
                    RotateAction rotAction = (RotateAction)action;
                    getRootPart().getSelectedPart().rotationOffset *= Quaternion.CreateFromYawPitchRoll(rotAction.rotation.X, rotAction.rotation.Y, rotAction.rotation.Z);
                    break;


                case actionType.quitProgram:
                    AnimatorMain.quitGame();
                    break;
                default:
                    throw new Exception("unhandled action");
                    

            }
        }

        void convertToCharacterAndSave(string path)
        {
            bool wasCharacter = false;
            if (character != null)
            {
                rootBodyPart = character.main;
                //character = null;w
                wasCharacter = true;
            }
            if (!wasCharacter) 
            {
                character = new AnimatedCharacter(rootBodyPart);
            }
            state = AppState.admiringCharacter;
            character.saveToFile(path);
            if (wasCharacter)
            {
                rootBodyPart = null;
            }

        }

        public void saveCharacterEvent(string path)
        {
            if (character == null && rootBodyPart == null)
            {
                MessageBox.Show("No character to save");
                return;
            }
            
            convertToCharacterAndSave(path);
            MainWindow.singleton.updateFileTreeView();
        }

        public void newEvent()
        {
            state = AppState.buildingCharacter;
            character = null;
            rootBodyPart = null;
            currentlyPlacingPart = null;
        }

        public void openCharacter(string path)
        {
            character = new AnimatedCharacter(path);
            state = AppState.admiringCharacter;
            updateMainWindowCharacterChart();
            switchBetweenCharacterAndRoot();
        }
        
        public void addModelOfBodyPartType(string path, CubeAnimator.BodyPartType type)
        {
            string bodyPartFileName = path.Split('/')[path.Split('/').Length-1];
            currentlyPlacingPart = new BodyPart(ModelLoader.loadSpaceFromName(path), new Vector3(), type, bodyPartFileName);
           // actionQueue.Add(new AddBodyPartAction(player.playerAimNearPoint(), player.playerAimingAt(), false, ModelLoader.loadSpaceFromName(path), type));
            //rootBodyPart = new BodyPart(ModelLoader.loadSpaceFromName(path), new Vector3(), type, "NOPE");
            if (rootBodyPart == null)
            {

                rootBodyPart = currentlyPlacingPart;
                currentlyPlacingPart = null;
                updateMainWindowCharacterChart();
            }

            
           //addModelOfBodyPartType(dataPath, type);
            //MessageBox.Show("The calculations are complete "+bodyPartFileName);


        }

        public void updateMainWindowCharacterChart()
        {
            MainWindow window = MainWindow.singleton;
            window.characterTreeView.Nodes.Clear();

            if (rootBodyPart != null)
            {
                window.characterTreeView.Nodes.Add(rootBodyPart.getTreeNode());
            }
            else if (character != null)
            {
                window.characterTreeView.Nodes.Add(character.main.getTreeNode());

            }

        }

        public void removePart(BodyPart bodyPart)
        {
            if (state == AppState.buildingCharacter)
            {
                if (bodyPart == rootBodyPart)
                {
                    rootBodyPart = null;
                }
                if (rootBodyPart != null)
                {
                    rootBodyPart.removePart(bodyPart);
                }
            }
            else if (state == AppState.admiringCharacter)
            {
                state = AppState.buildingCharacter;
                rootBodyPart = character.main;
                character = null;
                removePart(bodyPart);
            }
            
            
            updateMainWindowCharacterChart();
        }


        public void replacePart(BodyPart toReplace, string path)
        {
            PaintedCubeSpace newSpace = ModelLoader.loadSpaceFromName(path);
            if (state == AppState.buildingCharacter)
            {

                if (rootBodyPart != null)
                {
                    rootBodyPart.replacePart(toReplace, newSpace);
                    rootBodyPart.remesh(Compositer.device);
                }
            }
            else if (state == AppState.admiringCharacter)
            {
                state = AppState.buildingCharacter;
                rootBodyPart = character.main;
                character = null;
                rootBodyPart.replacePart(toReplace, newSpace);
                rootBodyPart.remesh(Compositer.device);
            }


            updateMainWindowCharacterChart();
        }


        public void changePartType(BodyPart toReplace, BodyPartType typeToMake)
        {
            
            
            if (state == AppState.buildingCharacter)
            {
                    rootBodyPart.changePartType(toReplace, typeToMake);
                    //rootBodyPart.remesh(Compositer.device);
                
            }
            else if (state == AppState.admiringCharacter)
            {
                state = AppState.buildingCharacter;
                rootBodyPart = character.main;
                character = null;
                rootBodyPart.changePartType(toReplace, typeToMake);
                //rootBodyPart.remesh(Compositer.device);
            }


            updateMainWindowCharacterChart();
        }


        void switchBetweenCharacterAndRoot()
        {
            if (rootBodyPart == null && character == null)
            {
                return;
            }
            if (character != null)
            {
                rootBodyPart = character.main;
                character = null;
                state = AppState.buildingCharacter;
                rootBodyPart.clearTransformations();
            }
            else
            {
                character = new AnimatedCharacter(rootBodyPart);
                rootBodyPart = null;
                state = AppState.admiringCharacter;
                character.main.highlight(null);
            }
        }

        void playerClickToSelectPart(PlayerLeftClickAction action)
        {
            BodyPart root=null;
            if (character != null)
            {
                root = character.main;
            }
            else if (rootBodyPart != null)
            {
                root = rootBodyPart;
            }
            if (root == null)
            {
                return;
            }
            BodyPartWithHitLoc? hitMaybe = root.getNearestHit(action.firstRef, action.secondRef, Matrix.Identity, Quaternion.Identity);

            if (hitMaybe != null)
            {
                BodyPartWithHitLoc hit = (BodyPartWithHitLoc)hitMaybe;
                root.highlight(hit.part);
            }
            else
            {
                root.highlight(null);
            }
            //root.highlight(root.getNearestHit(action.firstRef, action.secondRef, Matrix.Identity, Quaternion.Identity));
        }

        BodyPart getRootPart()
        {
            if (character != null)
            {
                return character.main;
            }
            return rootBodyPart;
        }

        public void selectPartWithCharacterTreeClick(BodyPart part)
        {
            BodyPart root = getRootPart();
            root.highlight(null);
            root.highlight(part);
        }

    }
}

