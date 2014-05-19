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

using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CubeAnimator{
    public class AnimatedCharacter
    {
        public BodyPart main;
        List<PositionForTime> positionQueue;

        public AnimatedCharacter(BodyPart nMainPart)
        {
            positionQueue = new List<PositionForTime>();
            main = nMainPart;
            main.setAnimations();
        }

        public AnimatedCharacter(string filePath)
        {
            positionQueue = new List<PositionForTime>();
            loadFromFile(filePath);
            main.setAnimations();
        }

        public void update()
        {
            updatePositionQueue();

            List<AnimationType> order = new List<AnimationType>();
            //order.Add(AnimationType.running);

            if (positionQueue.Count > 0)
            {
                order.AddRange(positionQueue[0].type);
            }

            if(Keyboard.GetState().IsKeyDown(Keys.P)){
                order.Add(AnimationType.walking);
            
            }
            else{
                order.Add(AnimationType.standing);
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                order.Add(AnimationType.armsOut);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                order.Add(AnimationType.stabLeftArm);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                positionQueue = getHammerAnimation();
            }

            //order.Add(AnimationType.toolInLeftHand);

            main.orderAnimation(order, new noAnimation());
        }

        public void saveToFile(string path)
        {
            
            //string path = "C:/Users/Public/" + folderName;





            if(new FileInfo(path).Extension != ".chr")
            {
                path += ".chr";
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            List<string> fileContents = main.saveToFolder(path, new List<string>());

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string write in fileContents)
                {
                    sw.WriteLine(write);
                }

            }

        }

        void updatePositionQueue()
        {
            if(positionQueue.Count==0)
            {
                return;
            }

            positionQueue[0].countDown--;
            if (positionQueue[0].countDown == 0)
            {
                positionQueue.RemoveAt(0);
            }
        }

        public void loadFromFile(string path)
        {
            string[] file = System.IO.File.ReadAllLines(path);
            main = new BodyPart();
            main.loadFromFile(file, 0);
        }



        static List<PositionForTime> getHammerAnimation()
        {
            List<PositionForTime> result = new List<PositionForTime>();

            result.Add(new PositionForTime(10,AnimationType.hammerHitRaisedLeftArm));

            List<AnimationType> downSwing = new List<AnimationType>();
            downSwing.Add(AnimationType.hammerHitLoweredLeftArm);
            //downSwing.Add(AnimationType.torsoLeftShoulderForward);
            result.Add(new PositionForTime(20,downSwing));
            return result;
        }
        
    }
}
