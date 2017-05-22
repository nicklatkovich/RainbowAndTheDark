using System;
using Microsoft.Xna.Framework;

namespace RainbowAndTheDark {
    public class Target : Instance {

        Vector2 MaskHalfSize = new Vector2(16, 16);
        bool NeedDraw = false;
        bool CreateEnemy = false;

        public Target( ) : base(Vector2.Zero) {
            MoveToRandomPosition( );
        }

        public override void Update(GameTime time) {
            NeedDraw = true;
            if (SimpleUtils.PlaceFree(Position, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize)) == false) {
                MoveToRandomPosition( );
            }
            Vector2 DiffToPlayer = (Program.Thread.Player.Position - this.Position).Abs( );
            Vector2 MaskDiff = Program.Thread.Player.MaskHalfSize + this.MaskHalfSize;
            if (DiffToPlayer.X < MaskDiff.X && DiffToPlayer.Y < MaskDiff.Y) {
                MoveToRandomPosition( );
            }

            base.Update(time);
        }

        public void DrawSpot(GameTime time) {
            if (NeedDraw) {
                Program.Thread.SpriteBatch.Draw(Program.Thread.SpotTexture, Position, origin: new Vector2(Program.Thread.SpotTexture.Width / 2f, Program.Thread.SpotTexture.Height / 2f), scale: new Vector2(1f), color: SimpleUtils.ColorFromHSV(SimpleUtils.Random(256), 0.8f, 1f), rotation: SimpleUtils.Random((float)Math.PI * 2));
                if (CreateEnemy && Program.Thread.Enemies.Count < Program.Thread.EnemiesMaxCount) {
                    Program.Thread.Enemies.Add(new Enemy((UPoint)(this.Position / Program.Thread.CellSize)));
                    CreateEnemy = false;
                }
            }
        }

        public override void Draw(GameTime time) {
        }

        public void MoveToRandomPosition( ) {
            Position = new Vector2(SimpleUtils.Random(Program.Thread.Map.Width - 2) + 1, SimpleUtils.Random(Program.Thread.Map.Height - 2) + 1) * Program.Thread.CellSize;
            NeedDraw = false;
            CreateEnemy = true;
        }
    }
}
