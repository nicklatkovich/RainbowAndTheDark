﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RainbowAndTheDark {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        KeyboardState Keyboard;
        KeyboardState KeyboardPrevious = SimpleUtils.GetKeyboardState( );

        public readonly UPoint MAP_SIZE = new UPoint(20, 10);

        Grid<UInt32> Map;
        UInt32 CellSize = 64;
        RenderTarget2D MapRender;
        Texture2D WallTexture;

        public SpriteFont FontArial;

        public MainThread( ) {
            this.Graphics = new GraphicsDeviceManager(this);
            //this.Graphics.IsFullScreen = true;
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 640;
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";

            Map = new Grid<UInt32>(this.MAP_SIZE, 0);
        }

        protected override void Initialize( ) {
            for (UInt32 i = 0; i < Map.Width; i++) {
                for (UInt32 j = 0; j < Map.Height; j++) {
                    this.Map[i, j] = SimpleUtils.IRandom(2);
                }
            }
            this.MapRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.X));

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load content
            this.WallTexture = Content.Load<Texture2D>("Walls/Wall0");
            this.FontArial = Content.Load<SpriteFont>("Fonts/Arial");

            GraphicsDevice.SetRenderTarget(MapRender);
            GraphicsDevice.Clear(Color.Transparent);
            SpriteBatch.Begin( );
            for (UInt32 i = 0; i < this.Map.Width; i++) {
                for (UInt32 j = 0; j < this.Map.Height; j++) {
                    if (this.Map[i, j] > 0) {
                        this.SpriteBatch.Draw(this.WallTexture, new Rectangle(
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

            KeyboardPrevious = Keyboard;
            base.Update(time);
        }

        protected override void Draw(GameTime time) {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: drawing code
            this.SpriteBatch.Begin( );
            this.SpriteBatch.Draw(MapRender, new Rectangle(0, 0, MapRender.Width, MapRender.Height), Color.White);
            this.SpriteBatch.DrawString(FontArial, time.ElapsedGameTime.TotalMilliseconds.ToString( ), Vector2.Zero, Color.Black);
            this.SpriteBatch.End( );

            base.Draw(time);
        }
    }
}
