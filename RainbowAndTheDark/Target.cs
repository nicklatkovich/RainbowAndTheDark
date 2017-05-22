using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public class Target : Instance, ISpottable {

        Vector2 MaskHalfSize = new Vector2(16, 16);
        bool NeedDraw = false;
        bool CreateNewEnemy = false;
        float Hue = Utils.Random( );
        bool IsFistDrawSpot = true;

        public Target( ) : base(Vector2.Zero) {
            MoveToRandomPosition( );
        }

        public override void Update(GameTime time) {
            NeedDraw = true;
            if (Utils.PlaceFree(Position, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize)) == false) {
                MoveToRandomPosition( );
            } else {
                if (CreateNewEnemy) {
                    CreateNewEnemy = false;
                    if (Program.Thread.EnemiesMaxCount > Program.Thread.Enemies.Count) {
                        Program.Thread.Enemies.Add(new Enemy((UPoint)(Position.ToPoint( ) / new UPoint(Program.Thread.CellSize))));
                    }
                }
            }
            Vector2 DiffToPlayer = (Program.Thread.Player.Position - this.Position).Abs( );
            Vector2 MaskDiff = Program.Thread.Player.MaskHalfSize + this.MaskHalfSize;
            if (DiffToPlayer.X < MaskDiff.X && DiffToPlayer.Y < MaskDiff.Y) {
                MoveToRandomPosition( );
                IsFistDrawSpot = true;
                CreateNewEnemy = true;
            }
            Hue += 0.05f;

            base.Update(time);
        }

        public void MoveToRandomPosition( ) {
            Position = new Vector2(Utils.Random(Program.Thread.Map.Width - 2) + 1, Utils.Random(Program.Thread.Map.Height - 2) + 1) * Program.Thread.CellSize;
            NeedDraw = false;
        }

        public void DrawSpot(SpriteBatch spriteBatch, GameTime time) {
            if (NeedDraw) {
                spriteBatch.DrawSprite(Resources.Get[IsFistDrawSpot ? Resources.SPRITE.Spot : Resources.SPRITE.SmallSpot], Position, Utils.ColorFromHSV(Hue, 0.8f, 1f), rotation: Utils.Random(Utils.TWO_PI));
                IsFistDrawSpot = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime time) {
        }
    }
}
