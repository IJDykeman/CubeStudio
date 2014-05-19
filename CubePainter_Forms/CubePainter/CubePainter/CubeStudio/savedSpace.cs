﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeStudio
{
    class SavedSpace
    {
        public byte[,,] array;
        public int width;
        public int height;
        public SavedSpace(byte[, ,] nArray, int nWidth, int nHeight)
        {
            array = nArray;
            width = nWidth;
            height = nHeight;
        }
    }
}
