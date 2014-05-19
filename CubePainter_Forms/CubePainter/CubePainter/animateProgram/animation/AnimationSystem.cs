using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CubeAnimator
{
    public enum AnimationType
    {
        none,
        standing,
        running,
        falling,
        toolInLeftHand,
        toolInRightHand,
        walking,
        armsOut,
        stabLeftArm,
        stabRightArm,
        hammerHitRaisedLeftArm,
        hammerHitLoweredLeftArm,
        torsoLeftShoulderForward


    }

    public abstract class AnimationSystem
    {
        public Quaternion rotation;
        public AnimationType currentAnimation;

        public MovementTarget currentTarget;


        public abstract void handleOrder(List<AnimationType> typem, AnimationSystem parent);
        public Quaternion getRotation()
        {
            return rotation;
        }

        public void lerpRotation()
        {


            //if (float.IsNaN())
            //{
             //   rotation = currentTarget.goal;

            //}
            float angleBetween = AnimationFunctions.angleBetweenQuaternions(rotation, currentTarget.goal);

            if (MathHelper.ToDegrees(angleBetween) < 10)
            {
                rotation = Quaternion.Lerp(rotation, currentTarget.goal, .2f);
                return;
            }

            float amountToUse = currentTarget.speed / angleBetween;

            float speedLimit = .4f;
            if(amountToUse>speedLimit)
            {
                amountToUse=speedLimit;
            }

            rotation = Quaternion.Lerp(rotation, currentTarget.goal, amountToUse);

            if (float.IsNaN(rotation.W))
            {
                rotation = currentTarget.goal;
            }
        }


        public void returnToDefaultPosition()
        {
            rotation = Quaternion.Identity;
        }

    }

    public class noAnimation : AnimationSystem
    {
        public noAnimation()
        {
            rotation = Quaternion.Identity;
        }
        public override void handleOrder(List<AnimationType> type, AnimationSystem parent)
        {

        }

    }


 


    /*public class leftLegAnimation : AnimationSystem
    {
        Quaternion raised;
        Quaternion lowered;
        Quaternion forward;

        float walkArmSwingAmount;
        float walkArmSwingSpeed;
        Quaternion swungForward;
        Quaternion swungBackward;

        int animationAge = 0;
        AnimationType currentAnimation;

        MovementTarget currentTarget;
        bool inverted;

        public float lowerArmSwing;

        public leftLegAnimation(bool ninverted)
        {
            inverted = ninverted;
            int invert = 1;
            if (inverted)
            {
                invert = -1;
            }
            currentTarget = new MovementTarget(Quaternion.Identity, 1);
            rotation = Quaternion.Identity;
            currentAnimation = AnimationType.none;
            Console.WriteLine("making a left leg");



            walkArmSwingAmount = MathHelper.ToRadians(90);
            walkArmSwingSpeed = .02f;
            swungForward = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0), walkArmSwingAmount, MathHelper.ToRadians(1));
            swungBackward = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(0),walkArmSwingAmount, MathHelper.ToRadians(1));
            if (inverted)
            {

                //swungForward = AnimationFunctions.mirror(swungForward);
                //swungBackward = AnimationFunctions.mirror(swungBackward);
            }
        }

        public override void handleOrder(AnimationType type, AnimationSystem parent)
        {
            switch (type)
            {

                case AnimationType.running:
                    if (currentAnimation != AnimationType.running)
                    {

                        currentTarget = new MovementTarget((inverted ? swungForward : swungBackward), walkArmSwingSpeed);
                    }

                    if (currentTarget.goal == swungBackward && AnimationFunctions.angleBetweenQuaternions(rotation, swungBackward) < .02)
                    {
                        currentTarget = new MovementTarget(swungForward, walkArmSwingSpeed);
                    }
                    if (currentTarget.goal == swungForward && AnimationFunctions.angleBetweenQuaternions(rotation, swungForward) < .02)
                    {
                        currentTarget = new MovementTarget(swungBackward, walkArmSwingSpeed);
                    }
                    //currentTarget = new MovementTarget(swungBackward, walkArmSwingSpeed);
                    
                    break;
            }
            currentAnimation = type;

            rotation = Quaternion.Lerp(rotation, currentTarget.goal, currentTarget.speed / AnimationFunctions.angleBetweenQuaternions(rotation, currentTarget.goal));


        }
    }*/

    static class AnimationSystemFactory
    {
        public static AnimationSystem getSystemFromType(BodyPartType type)
        {
            

            switch (type)
            {
                case BodyPartType.leftArm:
                    return new leftArmAnimation(false);
                case BodyPartType.lowerLeftArm:
                   return new LowerLeftArmAnimation(false);
                case BodyPartType.rightArm:
                    return new leftArmAnimation(true);
                case BodyPartType.lowerRightArm:
                    return new LowerLeftArmAnimation(true);
                case BodyPartType.rightLeg:
                    return new leftLegAnimation(true);
                case BodyPartType.lowerRightLeg:
                    return new LowerLeftLegAnimation(true);
                case BodyPartType.leftLeg:
                    return new leftLegAnimation(false);
                case BodyPartType.lowerLeftLeg:
                    return new LowerLeftLegAnimation(false);
                case BodyPartType.torso:
                    return new TorsoAnimation();
                case BodyPartType.head:
                    return new headAnimation();
                case BodyPartType.wheel:
                    return new WheelAnimation();
                case BodyPartType.rigid:
                    return new noAnimation();
                default:
                    throw new Exception("unhandled body type");
            }
        }
    }

    
}




