﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeStudio
{
    struct IntVector3
    {
        int X;
        int Y;
        int Z;

        public IntVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is IntVector3)
            {
                return ((IntVector3)obj) == this;
            }
            return false;
        }

        public static bool operator == (IntVector3 value1, IntVector3 value2)
        {
            return (value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z);
        }

        public static bool operator !=(IntVector3 value1, IntVector3 value2)
        {
            return (value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z);
        }

        public static IntVector3 operator +(IntVector3 value1, IntVector3 value2)
        {
            return new IntVector3(value1.X + value2.X, value1.Y + value2.Y, value1.Z + value2.Z);
        }


    }
}
