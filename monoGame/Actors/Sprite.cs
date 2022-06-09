using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Actors
{
    public class Sprite
    {
        public int Width { get; }
        public int Height { get; }
        public Texture2D Texture { get; private set; }
        public Rectangle[] SpriteFrames { get; private set; }
        public List<Rectangle> CollisionRectangle { get; set; }
        public float AnimationSpeed { get; set; }
        private Sprite(Texture2D texture)
        :this(texture,0,0,0,0,0,0,1) {}
        private Sprite(Texture2D texture, int xInitialPos, int yInitialPos, int width, int height,int offset, int nbFrames, float animationSpeed, bool isVertivalStrip = false)
        {
            Width = width;
            Height = height;
            Texture = texture;
            AnimationSpeed = animationSpeed;
            if (nbFrames > 0)
            {
                SpriteFrames = new Rectangle[nbFrames];
                for (int i = 0; i < nbFrames; i++)
                {
                    int x, y;
                    if(isVertivalStrip)
                    {
                        x = xInitialPos;
                        y = yInitialPos + (i * height) + (offset * 1);
                    }
                    else
                    {
                        x = xInitialPos + (i * width) + (offset * i);
                        y = yInitialPos;
                    }
                    SpriteFrames[i] = new Rectangle(x, y, width, height);
                    //SpriteFrames[i] = new Rectangle(isVertivalStrip ? xInitialPos : xInitialPos + (i * width) + offset, isVertivalStrip ? yInitialPos : yInitialPos + (i * height) + offset, width, height);
                }
            }
            CollisionRectangle = new List<Rectangle>() { new Rectangle(0, 0, width, height) };
        }

        public static Sprite CreateSprite(Texture2D texture, int xInitialPos, int yInitialPos, int width, int height, int offset, int nbFrames, float animationSpeed, SpriteService validator = null, bool isVertivalStrip = false)
        {
            return validator?.GetType().Name == "SpriteService" ? new Sprite(texture,xInitialPos,yInitialPos,width,height,offset,nbFrames,animationSpeed,isVertivalStrip) : null;
        }

        public static Sprite CreateSprite(Texture2D texture, SpriteService validator = null)
        {
            return validator?.GetType().Name == "SpriteService" ? new Sprite(texture) : null;
        }
    }
}
