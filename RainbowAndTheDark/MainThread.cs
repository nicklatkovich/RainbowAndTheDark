using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RainbowAndTheDark {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch { get; protected set; }
        public KeyboardState Keyboard { get; protected set; }
        public KeyboardState KeyboardPrevious { get; protected set; } = SimpleUtils.GetKeyboardState( );

        public readonly UPoint MAP_SIZE = new UPoint(20, 10);

        Grid<UInt32> Map;
        UInt32 CellSize = 64;
        RenderTarget2D MapRender;
        Texture2D WallTexture;
        public SpriteFont FontArial;
        Player Player;

        public MainThread( ) {
            this.Graphics = new GraphicsDeviceManager(this);
            //this.Graphics.IsFullScreen = true;
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 640;
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize( ) {
            Map = SimpleUtils.CreateMapFromMaze(SimpleUtils.CreateMaze(MAP_SIZE));
            this.MapRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.X));
            this.Player = new Player(new Vector2(64, 64));

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: load content
            this.WallTexture = Content.Load<Texture2D>("Walls/Wall0");
            this.FontArial = Content.Load<SpriteFont>("Fonts/Arial");
            Player.Texture = Content.Load<Texture2D>("Instances/Characters/Player");

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

            this.Player.Update(time);

            KeyboardPrevious = Keyboard;
            base.Update(time);
        }

        protected override void Draw(GameTime time) {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: drawing code
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
            this.SpriteBatch.Draw(MapRender, new Rectangle(0, 0, MapRender.Width, MapRender.Height), Color.White);
            //this.SpriteBatch.DrawString(FontArial, time.ElapsedGameTime.TotalMilliseconds.ToString( ), Vector2.Zero, Color.Black);
            this.Player.Draw(time);
            this.SpriteBatch.End( );

            base.Draw(time);
        }
    }
}
