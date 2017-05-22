﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RainbowAndTheDark {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch { get; protected set; }
        public KeyboardState Keyboard { get; protected set; }
        public KeyboardState KeyboardPrevious { get; protected set; } = SimpleUtils.GetKeyboardState( );

        public readonly UPoint MAP_SIZE = new UPoint(20, 10);

        public Grid<UInt32> Map;
        public UInt32 CellSize = 64;
        RenderTarget2D MapRender;
        RenderTarget2D ColorsRender;
        Texture2D WallTexture;
        public Texture2D SpotTexture { get; protected set; }
        public SpriteFont FontArial;
        public Player Player { get; protected set; }
        Effect glslAddColor;
        Target Target;

        public List<Enemy> Enemies = new List<Enemy>( );
        public uint EnemiesMaxCount = 4;

        public MainThread( ) {
            this.Graphics = new GraphicsDeviceManager(this);
            //this.Graphics.IsFullScreen = true;
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 640;
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize( ) {
            Tuple<Grid<uint>, UPoint> maze = SimpleUtils.CreateMaze(MAP_SIZE);
            Map = SimpleUtils.CreateMapFromMaze(maze.Item1);
            this.MapRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.X));
            this.ColorsRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.X), false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            this.Player = new Player((maze.Item2.ToVector2( ) + new Vector2(0.5f)) * CellSize);
            this.Target = new Target( );

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load content
            WallTexture = Content.Load<Texture2D>("Walls/Wall0");
            SpotTexture = Content.Load<Texture2D>("Instances/Colors/Spot");
            this.FontArial = Content.Load<SpriteFont>("Fonts/Arial");
            Player.Texture = Content.Load<Texture2D>("Instances/Characters/Player");
            glslAddColor = Content.Load<Effect>("Shaders/AddColors");

            GraphicsDevice.SetRenderTarget(ColorsRender);
            GraphicsDevice.Clear(Color.White);

            GraphicsDevice.SetRenderTarget(MapRender);
            GraphicsDevice.Clear(Color.DarkGray);
            SpriteBatch.Begin( );
            for (UInt32 i = 0; i < this.Map.Width; i++) {
                for (UInt32 j = 0; j < this.Map.Height; j++) {
                    if (this.Map[i, j] > 0) {
                        this.SpriteBatch.Draw(WallTexture, new Rectangle(
                            (Int32)(i * CellSize),
                            (Int32)(j * CellSize), (Int32)CellSize, (Int32)CellSize), Color.White);
                    }
                }
            }
            SpriteBatch.End( );
            this.GraphicsDevice.SetRenderTarget(null);
        }

        protected override void UnloadContent( ) {
            // TODO: Unload any non ContentManager content
        }

        protected override void Update(GameTime time) {
            Keyboard = SimpleUtils.GetKeyboardState( );
            if (Keyboard.IsKeyDown(Keys.Escape)) {
                Exit( );
            }

            this.Player.Update(time);
            this.Target.Update(time);
            foreach (var e in Enemies) {
                e.Update(time);
            }

            KeyboardPrevious = Keyboard;
            base.Update(time);
        }

        protected override void Draw(GameTime time) {
            GraphicsDevice.SetRenderTarget(ColorsRender);
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
            if (Player.IsNeedToDrawSpot) {
                this.Player.DrawSpot(time);
            }
            this.Target.DrawSpot(time);
            foreach (var e in Enemies) {
                e.DrawSpot(time);
            }
            this.SpriteBatch.End( );
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            // TODO: drawing code
            glslAddColor.Parameters["ColorTexture"].SetValue(ColorsRender);
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone, effect: glslAddColor);
            this.SpriteBatch.Draw(MapRender, new Rectangle(0, 0, MapRender.Width, MapRender.Height), Color.White);
            this.SpriteBatch.End( );
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
            this.Player.Draw(time);
            this.SpriteBatch.End( );

            base.Draw(time);
        }
    }
}
