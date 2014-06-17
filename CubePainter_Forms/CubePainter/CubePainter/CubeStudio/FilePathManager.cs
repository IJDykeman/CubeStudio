using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeStudio
{
    class FilePathManager
    {
        static string cubeStudioRootPath = @"C:\Users\Public\CubeStudioCreations\";

        public static string getRootPath()
        {
            return cubeStudioRootPath;
        }


        public static string addNecesaryPathing(string path)
        {
            if (path.Contains(getRootPath()))
            {
                return path;
            }
            else
            {
                return getRootPath() + path;
            }
        }
    }
}
