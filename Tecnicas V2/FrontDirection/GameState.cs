using FrontDirection.GraphicsSupport;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDirection
{
   
    class GameState
    {
        // Rocket support
        Vector2 mRocketInitDirection = Vector2.UnitY; // This does not change
        TexturedPrimitive mRocket;
        // Support the flying net
        TexturedPrimitive mNet;
        bool mNetInFlight = false;
        Vector2 mNetVelocity = Vector2.Zero;
        float mNetSpeed = 0.5f;
        // Insect support
        TexturedPrimitive mInsect;
        bool mInsectPreset = true;
        // Simple game status
        int mNumInsectShot;
        int mNumMissed;


        public GameState()
        {
            // Create and set up the primitives
            mRocket = new TexturedPrimitive("Rocket", new Vector2(5, 5), new Vector2(3, 10));
            // Initially the rocket is pointing in the positive y direction
            mRocketInitDirection = Vector2.UnitY;
            mNet = new TexturedPrimitive("Net", new Vector2(0, 0), new Vector2(2, 5));
            mNetInFlight = false; // until user press "A", rocket is not in flight
            mNetVelocity = Vector2.Zero;
            mNetSpeed = 1f;
            // Initialize a new insect
            mInsect = new TexturedPrimitive("Insect", Vector2.Zero, new Vector2(5, 5));
            mInsectPreset = false;
            // Initialize game status
            mNumInsectShot = 0;
            mNumMissed = 0;
        }

        /// <summary>
        /// Update the game state
        /// </summary>
        public void UpdateGame()
        {
            mRocket.RotateAngleInRadian += MathHelper.ToRadians(InputWrapper.ThumbSticks.Right.X); //roda o player
            mRocket.Position += InputWrapper.ThumbSticks.Left; //mexe o player

            //  dispara a rede
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
            {
                mNetInFlight = true;
                mNet.RotateAngleInRadian = mRocket.RotateAngleInRadian;
                mNet.Position = mRocket.Position;
                mNetVelocity = ShowVector.RotateVectorByAngle(
                mRocketInitDirection,
                mNet.RotateAngleInRadian) * mNetSpeed;
            }

            if (!mInsectPreset)
            {
                float x = 15f + ((float)Game1.sRan.NextDouble() * 30f);
                float y = 15f + ((float)Game1.sRan.NextDouble() * 30f);
                mInsect.Position = new Vector2(x, y);
                mInsectPreset = true;
            }

            if (mNetInFlight)
            {
                mNet.Position += mNetVelocity;
                if (mNet.PrimitivesTouches(mInsect))
                {
                    mInsectPreset = false;
                    mNetInFlight = false;
                    mNumInsectShot++;
                }

                if ((Camera.CollidedWithCameraWindow(mNet) !=
                Camera.CameraWindowCollisionStatus.InsideWindow))
                {
                    mNetInFlight = false;
                    mNumMissed++;
                }
            }


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        public void DrawGame()
        {
            mRocket.Draw();
            if (mNetInFlight)
                mNet.Draw();
            if (mInsectPreset)
                mInsect.Draw();
            // Print out text message to echo status
            FontSupport.PrintStatus(
            "Num insects netted = " + mNumInsectShot +
            " Num missed = " + mNumMissed, null);
        }
    }
}
