using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RainbowAndTheDark {
    public class Enemy : Instance {

        public UPoint PositionOnMap;
        public UPoint TargetToMove;
        public Vector2 Speed;
        public uint Step = 0u;
        public uint StepsToMove = 32u;

        public Enemy(UPoint pos) : base((pos.ToVector2( ) + new Vector2(0.5f)) * Program.Thread.CellSize) {
            this.TargetToMove = pos;
        }

        public override void Update(GameTime time) {
            if (Step > 0) {
                Position += Speed;
                Step--;
            } else {
                PositionOnMap = TargetToMove;
                Step = StepsToMove;
                UPoint[ ] results = new UPoint[4];
                uint results_size = 0;
                for (uint i = 0; i < 4; i++) {
                    UPoint new_target = (UPoint)(PositionOnMap + SimpleUtils.Dd(i));
                    if (Program.Thread.Map[new_target] == 0) {
                        results[results_size++] = new_target;
                    }
                }
                TargetToMove = results[SimpleUtils.IRandom(results_size)];
                Speed = (TargetToMove - PositionOnMap).ToVector2( ) * Program.Thread.CellSize / StepsToMove;
            }

            base.Update(time);
        }

        public void DrawSpot(GameTime time) {
            Program.Thread.SpriteBatch.Draw(Program.Thread.SpotTexture, Position, origin: new Vector2(Program.Thread.SpotTexture.Width / 2f, Program.Thread.SpotTexture.Height / 2f), scale: new Vector2(1f), color: Color.Black, rotation: SimpleUtils.Random((float)Math.PI * 2));
        }


        public override void Draw(GameTime time) {
        }
    }
}
