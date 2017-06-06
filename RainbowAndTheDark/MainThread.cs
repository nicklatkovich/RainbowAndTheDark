using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RainbowAndTheDark {
    public class MainThread : Game {
        public GraphicsDeviceManager Graphics {
            get; protected set;
        }
        public SpriteBatch SpriteBatch {
            get; protected set;
        }

        public readonly UPoint MAP_SIZE = new UPoint(20, 10);

        public Grid<UInt32> Map;
        public bool GameIsOver = false;
        public UInt32 CellSize = 64;
        RenderTarget2D MapRender;
        RenderTarget2D ColorsRender;
        public SpriteFont FontArial;
        public Player Player {
            get; protected set;
        }
        Effect glslAddColor;
        Target Target;

        public List<Enemy> Enemies = new List<Enemy>( );
        public List<Instance> OtherInstances = new List<Instance>( );
        public uint EnemiesMaxCount = 4;

        public MainThread( ) {
            this.Graphics = new GraphicsDeviceManager(this);
            //this.Graphics.IsFullScreen = true;
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 640;
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
        }

        internal void Restart( ) {
            GameIsOver = false;
            foreach (var a in OtherInstances) {
                if (a is GameOverLabel) {
                    (a as GameOverLabel).Create = false;
                }
            }
            Tuple<Grid<uint>, UPoint> maze = Utils.CreateMaze(MAP_SIZE);
            Map = Utils.CreateMapFromMaze(maze.Item1);
            this.Player = new Player((maze.Item2.ToVector2( ) + new Vector2(0.5f)) * CellSize);
            this.Target = new Target( );
            this.Enemies.Clear( );
            GraphicsDevice.SetRenderTarget(MapRender);
            GraphicsDevice.Clear(Color.DarkGray);
            SpriteBatch.Begin( );
            for (UInt32 i = 0; i < this.Map.Width; i++) {
                for (UInt32 j = 0; j < this.Map.Height; j++) {
                    if (this.Map[i, j] > 0) {
                        this.SpriteBatch.DrawSprite(Resources.Get[Resources.SPRITE.Wall], new Vector2(i, j) * CellSize);
                    }
                }
            }
            SpriteBatch.End( );
            this.GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.SetRenderTarget(ColorsRender);
            GraphicsDevice.Clear(Color.Transparent);
            this.GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Initialize( ) {
            Tuple<Grid<uint>, UPoint> maze = Utils.CreateMaze(MAP_SIZE);
            Map = Utils.CreateMapFromMaze(maze.Item1);
            this.MapRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.Y));
            this.ColorsRender = new RenderTarget2D(this.GraphicsDevice, (Int32)(CellSize * MAP_SIZE.X), (Int32)(CellSize * MAP_SIZE.Y), false, SurfaceFormat.Color, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
            this.Player = new Player((maze.Item2.ToVector2( ) + new Vector2(0.5f)) * CellSize);
            this.Target = new Target( );

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            this.FontArial = Content.Load<SpriteFont>("Fonts/Arial");
            glslAddColor = Content.Load<Effect>("Shaders/AddColors");
            Resources.LoadContent(Content);

            GraphicsDevice.SetRenderTarget(ColorsRender);
            GraphicsDevice.Clear(Color.Transparent);

            GraphicsDevice.SetRenderTarget(MapRender);
            GraphicsDevice.Clear(Color.DarkGray);
            SpriteBatch.Begin( );
            for (UInt32 i = 0; i < this.Map.Width; i++) {
                for (UInt32 j = 0; j < this.Map.Height; j++) {
                    if (this.Map[i, j] > 0) {
                        this.SpriteBatch.DrawSprite(Resources.Get[Resources.SPRITE.Wall], new Vector2(i, j) * CellSize);
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
            Input.PreUpdate(time);

            this.Player.Update(time);
            this.Target.Update(time);
            foreach (var e in Enemies.ToArray( )) {
                e.Update(time);
            }
            foreach (var a in OtherInstances.ToArray( )) {
                a.Update(time);
            }

            base.Update(time);
        }

        protected override void Draw(GameTime time) {
            GraphicsDevice.SetRenderTarget(ColorsRender);
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
            if (Player.IsNeedToDrawSpot) {
                this.Player.DrawSpot(SpriteBatch, time);
            }
            this.Target.DrawSpot(SpriteBatch, time);
            foreach (var a in OtherInstances) {
                if (a is ISpottable) {
                    (a as ISpottable).DrawSpot(SpriteBatch, time);
                }
            }
            foreach (var e in Enemies) {
                e.DrawSpot(SpriteBatch, time);
            }
            try {
                SpriteBatch.End( );
            } catch (Exception e) {
                MessageBox.Show(e.Message);
                GraphicsDevice.SetRenderTarget(null);
                this.Exit( );
                return;
            }
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Gray);

            glslAddColor.Parameters["ColorTexture"].SetValue(ColorsRender);
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone, effect: glslAddColor);
            this.SpriteBatch.Draw(MapRender, new Rectangle(0, 0, MapRender.Width, MapRender.Height), Color.White);
            this.SpriteBatch.End( );
            this.SpriteBatch.Begin(rasterizerState: RasterizerState.CullNone);
            this.Player.Draw(SpriteBatch, time);
            foreach (var e in Enemies) {
                e.Draw(SpriteBatch, time);
            }
            foreach (var a in OtherInstances) {
                a.Draw(SpriteBatch, time);
            }
            this.SpriteBatch.End( );

            base.Draw(time);
        }
    }
}
