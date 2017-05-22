using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public class GameOverLabel : Instance {

        public float alpha = 0f;
        public bool Create = true;

        public GameOverLabel( ) : base(Vector2.Zero) {
        }

        public override void Update(GameTime time) {
            if (alpha < 1f && Create) {
                alpha += 0.05f;
            } else {
                alpha -= 0.05f;
                if (alpha <= 0) {
                    Program.Thread.OtherInstances.Remove(this);
                }
            }

            base.Update(time);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime time) {
            spriteBatch.DrawString(Resources.Get[Resources.FONT.Fuehrer64], "Game Over", Vector2.Zero, new Color(1f, 0f, 0f, alpha));

            base.Draw(spriteBatch, time);
        }

    }
}
