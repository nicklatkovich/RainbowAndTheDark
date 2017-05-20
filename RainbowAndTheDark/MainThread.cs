using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RainbowAndTheDark {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        KeyboardState Keyboard;
        KeyboardState KeyboardPrevious = SimpleUtils.GetKeyboardState( );

        Grid<UInt32> Map = new Grid<UInt32>(40u, 20u, 0);
        RenderTarget2D MapRender;
        Texture2D WallTexture;

        public MainThread( ) {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 640;
            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize( ) {
            for (UInt32 i = 0; i < Map.Width; i++) {
                for (UInt32 j = 0; j < Map.Height; j++) {
                    Map[i, j] = SimpleUtils.IRandom(2);
                }
            }
            this.MapRender = new RenderTarget2D(this.GraphicsDevice, (Int32)Map.Width * 32, (Int32)Map.Height * 32);

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load content
            this.WallTexture = Content.Load<Texture2D>("Walls/Wall0");
            this.GraphicsDevice.SetRenderTarget(this.MapRender);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.SpriteBatch.Begin( );
            for (UInt32 i = 0; i < Map.Width; i++) {
                for (UInt32 j = 0; j < Map.Height; j++) {
                    if (Map[i, j] > 0) {
                        this.SpriteBatch.Draw(this.WallTexture, new Rectangle((Int32)i * 32, (Int32)j * 32, 32, 32), Color.White);
                    }
                }
            }
            this.SpriteBatch.End( );
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

            // TODO: update logic

            KeyboardPrevious = Keyboard;
            base.Update(time);
        }

        protected override void Draw(GameTime time) {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: drawing code
            this.SpriteBatch.Begin( );
            this.SpriteBatch.Draw(MapRender, Vector2.Zero, Color.White);
            this.SpriteBatch.End( );

            base.Draw(time);
        }
    }
}
