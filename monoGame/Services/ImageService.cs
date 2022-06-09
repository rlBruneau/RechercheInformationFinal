using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Actors;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Services
{
    public class ImageService
    {
        private static object lockObj = new object();
        private static ImageService instance = null;
        public static ImageService Instance
        {
            get
            {
                lock (lockObj)
                {
                    if(instance == null)
                    {
                        instance = new ImageService();
                    }
                    return instance;
                }
            }
        }

        public ContentManager Content { get; set; } = null;

        private ImageService() { }

        private Texture2D CreateTexture(string textureNamePath)
        {
            return Content.Load<Texture2D>(textureNamePath);
        }

    }
}
