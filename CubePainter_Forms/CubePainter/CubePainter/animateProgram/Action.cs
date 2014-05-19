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


using CubeStudio;

namespace CubeAnimator
{
    public enum actionType
    {
        addBodyPart,
        animateButtonClicked,
        playerLeftClick,
        playerRightClick,
        playerShiftLeftClick,
        playerAltClick,
        playerUndo,
        playerNeedsUndoSave,
        saveDocument,
        openDocument,
        newDocument,
        quitProgram,
        scaleUpButton,
        scaleDownAction,
        moveForwardAction,
        moveBackAction,
        moveUpAction,
        moveDownAction,
        moveRightAction,
        moveLeftAction,
        rotateAction,


        ctrlC
    }

    public abstract class Action
    {
        public actionType type;
        
    }

    class AddBodyPartAction : Action
    {

        public Vector3 firstRef;
        public Vector3 secondRef;
        public bool mirror;
        public bool successful = false;
        public PaintedCubeSpace space;
        public BodyPartType bodyPartType;

        public AddBodyPartAction(Vector3 nFirst, Vector3 nSecond, bool nMirror, PaintedCubeSpace nSpace, BodyPartType nBodyPartType)
        {
            type = actionType.addBodyPart;
            firstRef = nFirst;
            secondRef = nSecond;
            mirror = nMirror;
            space = nSpace;
            bodyPartType = nBodyPartType;
        }

    }


    class PlayerLeftClickAction : Action
    {
        
        public Vector3 firstRef;
        public Vector3 secondRef;
        public bool mirror;
        public bool successful = false;

        public PlayerLeftClickAction(Vector3 nFirst, Vector3 nSecond, bool nMirror)
        {
            type = actionType.playerLeftClick;
            firstRef = nFirst;
            secondRef = nSecond;
            mirror = nMirror;
        }

    }

    class PlayerRightClickAction : Action
    {

        public Vector3 firstRef;
        public Vector3 secondRef;
        public bool mirror;
        public bool successful = false;

        public PlayerRightClickAction(Vector3 nFirst, Vector3 nSecond, bool nMirror)
        {
            type = actionType.playerRightClick;
            firstRef = nFirst;
            secondRef = nSecond;
            mirror = nMirror;
        }

    }

    class PlayerAltClickAction : Action
    {

        public Vector3 firstRef;
        public Vector3 secondRef;
        public bool mirror;
        public byte originalColor;

        public PlayerAltClickAction(Vector3 nFirst, Vector3 nSecond, bool nMirror)
        {
            type = actionType.playerAltClick;
            firstRef = nFirst;
            secondRef = nSecond;
            mirror = nMirror;
        }

    }

    class PlayerShiftLeftClickAction : Action
    {

        public Vector3 firstRef;
        public Vector3 secondRef;

        public PlayerShiftLeftClickAction(Vector3 nFirst, Vector3 nSecond)
        {
            type = actionType.playerShiftLeftClick;
            firstRef = nFirst;
            secondRef = nSecond;
        }
    }

    class PlayerUndoAction : Action
    {



        public PlayerUndoAction()
        {
            type = actionType.playerUndo;
        }
    }

    class AnimateButtonClicked : Action
    {
        public AnimateButtonClicked()
        {
            type = actionType.animateButtonClicked;
        }
    }
    
    class PlayerNeedsUndoSave : Action
    {



        public PlayerNeedsUndoSave()
        {
            type = actionType.playerNeedsUndoSave;
        }
    }
    class PlayerSaveDocumentAction : Action
    {

        public PlayerSaveDocumentAction()
        {
            type = actionType.saveDocument;
        }
    }
    class PlayerOpenDocumentAction : Action
    {

        public PlayerOpenDocumentAction()
        {
            type = actionType.openDocument;
        }
    }

    class PlayerNewDocumentAction : Action
    {

        public PlayerNewDocumentAction()
        {
            type = actionType.newDocument;
        }
    }
    class PlayerQuitProgram : Action
    {

        public PlayerQuitProgram()
        {
            type = actionType.quitProgram;
        }
    }
    class PlayerCtrlC : Action
    {

        public PlayerCtrlC()
        {
            type = actionType.ctrlC;
        }
    }

    class ScaleUpAction : Action
    {

        public ScaleUpAction()
        {
            type = actionType.scaleUpButton;
        }
    }

    class ScaleDownAction : Action
    {

        public ScaleDownAction()
        {
            type = actionType.scaleDownAction;
        }
    }


    class MoveForwardAction : Action
    {

        public MoveForwardAction()
        {
            type = actionType.moveForwardAction;
        }
    }
    class MoveBackAction : Action
    {

        public MoveBackAction()
        {
            type = actionType.moveBackAction;
        }
    }
        class MoveUpAction : Action
    {

        public MoveUpAction()
        {
            type = actionType.moveUpAction;
        }
    }
    class MoveDownAction : Action
    {

        public MoveDownAction()
        {
            type = actionType.moveDownAction;
        }
    }
    class MoveLeftAction : Action
    {

        public MoveLeftAction()
        {
            type = actionType.moveLeftAction;
        }
    }
    class MoveRightAction : Action
    {

        public MoveRightAction()
        {
            type = actionType.moveRightAction;
        }
    }

    class RotateAction : Action
    {
        public Vector3 rotation;
        public RotateAction(Vector3 nRotation)
        {
            rotation = nRotation;
            type = actionType.rotateAction;
        }
    }

    


}
