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

namespace CubePainter
{
    public enum actionType
    {
        playerLeftClick,
        playerRightClick,
        playerShiftLeftClick,
        playerAltClick,
        playerUndo,
        playerNeedsUndoSave,
        flipSpace,
        moveSpace,
        buildRect,
        saveDocument,
        openDocument,
        newDocument,
        quitProgram
    }

    public abstract class Action
    {
        public actionType type;
        
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
    class PlayerNeedsUndoSave : Action
    {
        public PlayerNeedsUndoSave()
        {
            type = actionType.playerNeedsUndoSave;
        }
    }

    class FlipSpaceAction : Action
    {
        public FlipSpaceAction()
        {
            type = actionType.flipSpace;
        }
    }

    class MoveWorkAction : Action
    {
        public Vector3 move;
        public MoveWorkAction(Vector3 nMove)
        {
            move = nMove;
            type = actionType.moveSpace;
        }
    }


    class PlayerSaveDocumentAction : Action
    {
        public string path;
        public PlayerSaveDocumentAction(string nPath)
        {
            path = nPath;
            type = actionType.saveDocument;
        }
    }

    class BuildRectAction : Action
    {
        public Vector3 loc1;
        public Vector3 loc2;
        public BuildRectAction(Vector3 nloc1, Vector3 nloc2)
        {
            loc1 = nloc1;
            loc2 = nloc2;
            type = actionType.buildRect;
        }
    }

    class PlayerOpenDocumentAction : Action
    {
        public string path;
        public PlayerOpenDocumentAction(string nPath)
        {
            path = nPath;
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

}
