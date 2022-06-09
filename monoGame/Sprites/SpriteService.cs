using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Actors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace monoGame.Sprites
{
    public class SpriteService
    {
        private static object lockObj = new object();
        private static SpriteService instance = null;
        public static SpriteService Instance
        {
            get
            {
                lock (lockObj)
                {
                    if(instance == null)
                    {
                        instance = new SpriteService();
                    }
                    return instance;
                }
            }
        }

        private SpriteService() { }
        private ContentManager Content { get; set; }
        public Dictionary<Textures, Texture2D> Textures { get; set; } = new Dictionary<Textures, Texture2D>();
        public Dictionary<Sprites, Sprite> Sprites { get; private set; } = new Dictionary<Sprites, Sprite>();

        public void LoadInitialTexture(ContentManager content, Textures[] textures)
        {
            Content = content;
            foreach(Textures tex in textures)
            {
                Textures.Add(tex, LoadTexture(tex));
            }
        }

        public Texture2D LoadTexture(Textures texture)
        {
            return Content.Load<Texture2D>(texture.ToString());
        }

        public void LoadGeneralSprite()
        {
           CreateSprite(monoGame.Sprites.Sprites.MarioRun, Textures[monoGame.Sprites.Textures.MarioLuiji], 20, 8, 16, 16, 2, 3, 100f);
           CreateSprite(monoGame.Sprites.Sprites.MarioIdle, Textures[monoGame.Sprites.Textures.MarioLuiji], 0, 8, 16, 16, 0, 1, 0);
           CreateSprite(monoGame.Sprites.Sprites.MarioJump, Textures[monoGame.Sprites.Textures.MarioLuiji], 96, 8, 16, 16, 0, 1, 0);
        }

        public Sprite CreateSprite(Sprites spriteEnum, Texture2D texture, int xPos, int yPos, int width, int height, int offset, int nbFrame, float generalSpeed)
        {
            Sprite sprite = Sprite.CreateSprite(texture, xPos, yPos, width, height, offset, nbFrame, generalSpeed, this);
            Sprites.Add(spriteEnum, sprite);
            return sprite;
        }
    }
}
