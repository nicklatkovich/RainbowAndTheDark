using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RainbowAndTheDark {
    public class Player : Instance, ISpottable {

        Point HalfSize = new Point(16, 16);
        public Vector2 MaskHalfSize { get; protected set; } = new Vector2(10, 16);
        bool ToLeft = true;
        Vector2 MaxSpeed = new Vector2(5);
        public bool IsNeedToDrawSpot { get; protected set; } = true;
        bool SpotIsSmall;
        Vector2 DMove = Vector2.Zero;

        public float Hue = 0f;

        public Player(Vector2 position) : base(position) {
        }

        public override void Update(GameTime time) {
            SpotIsSmall = true;
            if (Program.Thread.GameIsOver) {
                if (Input.IsKeyPressed(Keys.Enter)) {
                    Program.Thread.Restart( );
                }
            } else {
                Vector2 xy = Position;
                if (Input.Keyboard.IsKeyDown(Keys.A)) {
                    xy.X -= MaxSpeed.X;
                }
                if (Input.Keyboard.IsKeyDown(Keys.D)) {
                    xy.X += MaxSpeed.X;
                }
                if (xy.X != Position.X) {
                    if (Utils.PlaceFree(xy, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize))) {
                        Position.X = xy.X;
                    } else {
                        xy.X = Position.X;
                    }
                }
                if (Input.Keyboard.IsKeyDown(Keys.W)) {
                    xy.Y -= MaxSpeed.Y;
                }
                if (Input.Keyboard.IsKeyDown(Keys.S)) {
                    xy.Y += MaxSpeed.Y;
                }
                if (xy.Y != Position.Y) {
                    if (Utils.PlaceFree(xy, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize))) {
                        Position.Y = xy.Y;
                    } else {
                        xy.Y = Position.Y;
                    }
                }
                if (Position.X < PositionPrevious.X) {
                    ToLeft = true;
                } else if (Position.X > PositionPrevious.X) {
                    ToLeft = false;
                }
            }
            IsNeedToDrawSpot = (PositionPrevious != Position);
            Hue += 0.02f;
            if (DMove == Vector2.Zero && Position - PositionPrevious != Vector2.Zero) {
                SpotIsSmall = false;
            }
            DMove = Position - PositionPrevious;

            base.Update(time);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime time) {
            if (Program.Thread.GameIsOver == false) {
                spriteBatch.DrawSprite(Resources.Get[Resources.SPRITE.Player], Position, scale: new Vector2(ToLeft ? 1 : -1, 1));
            }
        }

        public void DrawSpot(SpriteBatch spriteBatch, GameTime time) {
            if (Program.Thread.GameIsOver == false) {
                spriteBatch.DrawSprite(Resources.Get[SpotIsSmall ? Resources.SPRITE.SmallSpot : Resources.SPRITE.Spot], Position, Utils.ColorFromHSV(Hue, 0.8f, 1f), rotation: Utils.Random(Utils.TWO_PI));
            }
        }
    }
}
