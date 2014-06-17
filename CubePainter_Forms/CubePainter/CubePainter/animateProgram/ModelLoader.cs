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
using CubePainter;
using System.Windows.Forms;

using CubeStudio;

namespace CubeAnimator
{
    static class ModelLoader
    {
        /*public static PaintedCubeSpace loadSpace(string partName, out string fileName)
        {
            PaintedCubeSpace bodypart=new PaintedCubeSpace();

            string folderPath = Microsoft.VisualBasic.Interaction.InputBox("Please select the file name of the "+partName, "Open", "", 300, 300);
            string dataPath = CubeStudio.MainWindow.mainFolderPath + folderPath + "/data.xml";
            Console.WriteLine(dataPath);
            if (!File.Exists(dataPath))
            {
                //Microsoft.VisualBasic.Interaction.InputBox("File name invalid.  Please select a file name to open", "Open", "file name", 300, 300);
                fileName = "";
                MessageBox.Show("Invalid load data");

                return null;

            }
            int newCubeSpaceWidth=0;
            int newCubeSpaceHeight=0;
            fileName = folderPath;
            using (StreamReader sr = File.OpenText(CubeStudio.MainWindow.mainFolderPath+ folderPath + "/config.txt"))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                    string[] array = s.Split(' ');
                    newCubeSpaceWidth = Convert.ToInt32(array[0]);
                    newCubeSpaceHeight = Convert.ToInt32(array[1]);
                }
            }
            FileInfo fileInfo = new FileInfo(dataPath);
            bodypart.spaceWidth = newCubeSpaceWidth;
            bodypart.spaceHeight = newCubeSpaceHeight;

            byte[, ,] obj = new byte[bodypart.spaceWidth, bodypart.spaceHeight, bodypart.spaceWidth];

            //Opens file "data.xml" and deserializes the object from it.
            Stream stream = File.Open(dataPath, FileMode.Open);


            BinaryFormatter formatter = new BinaryFormatter();

            //formatter = new BinaryFormatter();

            obj = (byte[,,])formatter.Deserialize(stream);
            //bodypart.decompressArrayAndSetArray(obj);
            bodypart.array = obj;
            bodypart.createModel(Compositer.device);
            stream.Close();
            return bodypart;
        
        }
        */

        public static PaintedCubeSpace loadSpaceFromName(string folderPath)
        {
            folderPath = FilePathManager.addNecesaryPathing(folderPath);
            PaintedCubeSpace paintedCubeSpace=new PaintedCubeSpace();

            
            string dataPath = folderPath;
            
            if (!File.Exists(dataPath))
            {
                //Microsoft.VisualBasic.Interaction.InputBox("File name invalid.  Please select a file name to open", "Open", "file name", 300, 300);
                folderPath = "";
                return null;

            }
            FileInfo fileInfo = new FileInfo(dataPath);

            long fileLength = fileInfo.Length;

            int newCubeSpaceWidth = (int)Math.Pow(fileLength, 1.0 / 3.0);
            int newCubeSpaceHeight = (int)Math.Pow(fileLength, 1.0 / 3.0);

            /*using (StreamReader sr = File.OpenText(folderPath + "/config.txt"))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                    string[] array = s.Split(' ');
                    newCubeSpaceWidth = Convert.ToInt32(array[0]);
                    newCubeSpaceHeight = Convert.ToInt32(array[1]);
                }
            }*/


            
            paintedCubeSpace.spaceWidth = newCubeSpaceWidth;
            paintedCubeSpace.spaceHeight = newCubeSpaceHeight;

            byte[, ,] obj = new byte[paintedCubeSpace.spaceWidth, paintedCubeSpace.spaceHeight, paintedCubeSpace.spaceWidth];

            //Opens file "data.xml" and deserializes the object from it.
            Stream stream = File.Open(dataPath, FileMode.Open);


            BinaryFormatter formatter = new BinaryFormatter();

            //formatter = new BinaryFormatter();

            obj = (byte[,,])formatter.Deserialize(stream);
            //bodypart.decompressArrayAndSetArray(obj);
            paintedCubeSpace.array = obj;
            paintedCubeSpace.createModel(Compositer.device);
            stream.Close();
            return paintedCubeSpace;
        
        
        }
    }
}
