using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public class GameOverLabel : Instance, ISpottable {

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
            //spriteBatch.DrawString(Resources.Get[Resources.FONT.Fuehrer64], "Game Over", Vector2.Zero, new Color(1f, 0f, 0f, alpha));
            spriteBatch.DrawString(Resources.Get[Resources.FONT.Fuehrer64], "Game Over", new Vector2(Program.Thread.Graphics.PreferredBackBufferWidth / 2, Program.Thread.Graphics.PreferredBackBufferHeight / 2), new Color(0f, 0f, 0f, alpha), 0f, Resources.Get[Resources.FONT.Fuehrer64].MeasureString("Game Over") / 2f, 1f, SpriteEffects.None, 0);

            base.Draw(spriteBatch, time);
        }

        public void DrawSpot(SpriteBatch spriteBatch, GameTime time) {
            if (Program.Thread.GameIsOver) {
                Vector2 halfStringMeasure = Resources.Get[Resources.FONT.Fuehrer64].MeasureString("Game Over") / 2f;
                spriteBatch.DrawSprite(Resources.Get[Resources.SPRITE.Spot], new Vector2(
                    Utils.Random(-halfStringMeasure.X, halfStringMeasure.X) + Program.Thread.Graphics.PreferredBackBufferWidth / 2,
                    Utils.Random(-halfStringMeasure.Y, halfStringMeasure.Y) + Program.Thread.Graphics.PreferredBackBufferHeight / 2),
                    Utils.ColorFromHSV(Utils.Random(1), 0.8f, 1f), rotation: Utils.Random(Utils.TWO_PI));
            }
        }
    }
}
