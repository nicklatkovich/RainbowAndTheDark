﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RainbowAndTheDark {
    public class Player : Instance {

        public static Texture2D Texture;
        Point HalfSize = new Point(16, 16);
        Point MaskHalfSize = new Point(10, 16);
        bool ToLeft = true;
        Vector2 MaxSpeed = new Vector2(3);

        public Player(Vector2 position) : base(position) {
        }

        public override void Update(GameTime time) {
            if (Program.Thread.Keyboard.IsKeyDown(Keys.A)) {
                this.Position.X -= MaxSpeed.X;
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.D)) {
                this.Position.X += MaxSpeed.X;
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.W)) {
                this.Position.Y -= MaxSpeed.Y;
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.S)) {
                this.Position.Y += MaxSpeed.Y;
            }
            if (Position.X < PositionPrevious.X) {
                ToLeft = true;
            } else if (Position.X > PositionPrevious.X) {
                ToLeft = false;
            }

            base.Update(time);
        }

        public override void Draw(GameTime time) {

            Program.Thread.SpriteBatch.Draw(Texture, new Rectangle(Position.ToPoint( ) - HalfSize + (ToLeft ? new Point(0) : new Point(32, 0)), new Point((ToLeft ? 32 : -32), 32)), Color.White);

        }
    }
}
