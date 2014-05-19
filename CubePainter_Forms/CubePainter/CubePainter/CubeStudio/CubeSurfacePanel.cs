using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace CubeStudio
{
    struct CubeSurfacePanel
    {
        IntVector3 loc1;
        IntVector3 loc2;
        byte type;
        PaintedCubeSpace.AxisDirection direction;

        public CubeSurfacePanel(IntVector3 nLoc1, IntVector3 nLoc2, byte ntype, PaintedCubeSpace.AxisDirection nDirection)
        {
            loc1 = nLoc1;
            loc2 = nLoc2;
            type = ntype;
            direction = nDirection;
        }
        
    }
}
