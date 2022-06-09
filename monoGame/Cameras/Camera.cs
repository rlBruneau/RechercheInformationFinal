using Microsoft.Xna.Framework;
using monoGame.Actors.Movables;
using System;
using System.Collections.Generic;
using System.Text;

namespace monoGame.Cameras
{
    public class Camera
    {
        public Vector2 Offset { get; set; }
        private Vector2 CameraFollowPoint { get; set; }
        public Movable Target { get; set; }
        public float MinX, MinY, MaxX, MaxY;
        public Camera(Movable target, float minX, float minY, float maxX, float maxY)
        {

            Target = target;
            CameraFollowPoint = new Vector2(monoGameProjectManager.WindowWidth/2 - Target.Sprite.Width/2, monoGameProjectManager.WindowHeight/2 - Target.Sprite.Height/2);
            Offset = new Vector2(CameraFollowPoint.X, CameraFollowPoint.Y);
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }
        public void UpdateCam()
        {
            float x = Offset.X - Target.Speed.X;
            float y = Offset.Y - Target.Speed.Y;
            if (Target.Position.X - CameraFollowPoint.X < MinX) x = MinX;
            if ((MaxX - Target.Position.X) - CameraFollowPoint.X - Target.Sprite.Width < 0) x = monoGameProjectManager.WindowWidth - MaxX;
            if (Target.Position.Y - CameraFollowPoint.Y < MinY) y = MinY;
            if (MaxY - Target.Position.Y - CameraFollowPoint.Y - Target.Sprite.Height < 0) y = monoGameProjectManager.WindowHeight - MaxY;


            Offset = new Vector2(x, y);
        }
    }
}
