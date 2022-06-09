using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monoGame.Actors;
using monoGame.Actors.Movables;
using monoGame.Actors.Movables.Players;
using monoGame.Cameras;
using monoGame.Sprites;
using monoGame.TileMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monoGame.States.GeneralStates
{
    public class GameState : StateBase
    {

        private static object lockObj = new object();
        private TileMap TestTileMap { get; set; }
        private static GameState instance = null;
        public static GameState Instance
        {
            get
            {
                lock (lockObj)
                {
                    if(instance == null)
                    {
                        instance = new GameState();
                    }
                    return instance;
                }
            }
        }

        private SpriteService SpriteService { get; set; } = SpriteService.Instance;
        private ActorBase Player { get; set; }
        private Camera Camera { get; set; }
        private List<ActorBase> Actors { get; set; } = new List<ActorBase>();
        private float NormalFriction { get; set; } = 0.8f;
        private float IceFriction { get; set; } = 0.06f;


        private GameState()
        {
            Player = new Mario(SpriteService.Sprites[Sprites.Sprites.MarioIdle], new Vector2(0, 0), true, IceFriction);
            Camera = new Camera((Movable)Player, 0f, 0f, 960 * 2, 640f);
            Actors.Add(Player);
            TestTileMap = new TileMap(SpriteService.Textures[Textures.MarioTiles], 16, 16, new Rectangle(0,0,8,8));
            TestTileMap.AddTilesRange(102, 50, 1, 1, false, false);
            TestTileMap.AddTilesRange(0, 16, 1, 1);
            TestTileMap.AddTilesRange(34, 16, 1, 1);
            TestTileMap.AddTilesRange(0, 196, 6, 1);
            foreach (Tile tile in TestTileMap.Tiles.Skip(1))
            {
                tile.IsSolid = true;
                tile.AppliedFriction = NormalFriction;
            }
            TestTileMap.Map = new int[,]
            {
                {  2,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  3,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  3,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  3,  3,  3,  3,  3,  3,  0,  0,  0,  0,  5,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},
                {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  0,  3,  4,  4,  4,  4,  5,  0,  1,  1,  1,  1,  1,  1,  1},
                {  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  1,  1,  1,  1},
                
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            TestTileMap.Draw(gameTime, spriteBatch, Camera);
            foreach(ActorBase actor in Actors)
            {
                actor.Draw(gameTime, spriteBatch, Camera);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ActorBase actor in Actors)
            {
                actor.Update(gameTime);
                actor.TestSolidTileCollision(TestTileMap);
                actor.IsColliding(actor);
                actor.ManageSpriteSpeedBased();
            }
            Camera.UpdateCam();
        }
    }
}
