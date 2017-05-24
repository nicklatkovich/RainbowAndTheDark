using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public class Enemy : Instance, ISpottable {

        public UPoint PositionOnMap;
        public UPoint TargetToMove;
        public Vector2 Speed;
        public uint Step = 0u;
        public uint StepsToMove = 32u;
        private uint PreviousDirection;

        public static Texture2D Eyes;

        public Enemy(UPoint pos) : base((pos.ToVector2( ) + new Vector2(0.5f)) * Program.Thread.CellSize) {
            this.TargetToMove = pos;
            PreviousDirection = (uint)(((float)Math.Atan2(
                Program.Thread.Player.Position.X - Position.X,
                Program.Thread.Player.Position.Y - Position.Y) + Math.PI / 4f) / Math.PI * 2f) % 4;
        }

        public override void Update(GameTime time) {
            if (Step > 0) {
                Position += Speed;
                Step--;
            } else {
                PositionOnMap = TargetToMove;
                Step = StepsToMove;
                Tuple<UPoint, uint>[ ] results = new Tuple<UPoint, uint>[4];
                uint results_size = 0;
                for (uint i = 0; i < 3; i++) {
                    uint d = i >= PreviousDirection ? i + 1 : i;
                    UPoint new_target = (UPoint)(PositionOnMap + Utils.Dd(d));
                    if (Program.Thread.Map[new_target] == 0) {
                        results[results_size++] = new Tuple<UPoint, uint>(new_target, d);
                    }
                }
                if (results_size == 0) {
                    results[results_size++] = new Tuple<UPoint, uint>((UPoint)(PositionOnMap + Utils.Dd(PreviousDirection)), PreviousDirection);
                }
                uint directionIndex = Utils.IRandom(results_size);
                TargetToMove = results[directionIndex].Item1;
                PreviousDirection = (results[directionIndex].Item2 + 2) % 4;
                Speed = (TargetToMove - PositionOnMap).ToVector2( ) * Program.Thread.CellSize / StepsToMove;
            }
            if (Math.Abs(Position.X - Program.Thread.Player.Position.X) < 32 &&
                Math.Abs(Position.Y - Program.Thread.Player.Position.Y) < 32) {
                if (Program.Thread.GameIsOver == false) {
                    Program.Thread.OtherInstances.Add(new GameOverLabel( ));
                }
                Program.Thread.GameIsOver = true;
                Program.Thread.Enemies.Clear( );
            }

            base.Update(time);
        }

        public void DrawSpot(SpriteBatch spriteBatch, GameTime time) {
            bool largeSpot = Utils.IRandom(32) == 0;
            spriteBatch.DrawSprite(Resources.Get[largeSpot ? Resources.SPRITE.Spot : Resources.SPRITE.SmallSpot], Position, Color.Black, rotation: Utils.Random(Utils.TWO_PI));
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime time) {
            spriteBatch.DrawSprite(Resources.Get[Resources.SPRITE.Eues], Position, scale: new Vector2(0.5f));
        }
    }
}
