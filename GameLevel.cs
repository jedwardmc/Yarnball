using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GPT_Assignment
{
    public class GameLevel
    {
        /*** 
         *** 
         *** Stores all the important components of the current
         *** computer game, such as the load content, timer and
         *** draw methods, for each level in the current computer
         *** game.         
         ***         
         ***/

        SpriteBatch currSB;
        int currLevel; 

        public GameLevel()
        {
        }

        public void SetSB(SpriteBatch sb)
        {
            // set the sprite batch for this game level class
            currSB = sb; 
        }

        public SpriteBatch GetSB()
        {
            // get the current sprite batch 
            return currSB; 
        }

        public void SetLevel(int l)
        {
            // set the level for the game 
            currLevel = l; 
        }

        public int GetLevel() { 
            // get the current level for the game 
            return currLevel;  
        }




    }
}
