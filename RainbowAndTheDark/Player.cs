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
        Vector2 MaskHalfSize = new Vector2(10, 16);
        bool ToLeft = true;
        Vector2 MaxSpeed = new Vector2(3);
        public bool IsNeedToDrawSpot { get; protected set; } = true;
        public float Hue = 0f;


        public Player(Vector2 position) : base(position) {
        }

        public override void Update(GameTime time) {
            Vector2 xy = Position;
            if (Program.Thread.Keyboard.IsKeyDown(Keys.A)) {
                xy.X -= MaxSpeed.X;
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.D)) {
                xy.X += MaxSpeed.X;
            }
            if (xy.X != Position.X) {
                if (SimpleUtils.PlaceFree(xy, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize))) {
                    Position.X = xy.X;
                } else {
                    xy.X = Position.X;
                }
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.W)) {
                xy.Y -= MaxSpeed.Y;
            }
            if (Program.Thread.Keyboard.IsKeyDown(Keys.S)) {
                xy.Y += MaxSpeed.Y;
            }
            if (xy.Y != Position.Y) {
                if (SimpleUtils.PlaceFree(xy, MaskHalfSize, Program.Thread.Map, new UPoint(Program.Thread.CellSize))) {
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
            IsNeedToDrawSpot = (PositionPrevious != Position);
            Hue += 8;

            base.Update(time);
        }

        public override void Draw(GameTime time) {
            Program.Thread.SpriteBatch.Draw(Texture, Position, origin: new Vector2(Texture.Width / 2f, Texture.Height / 2f), scale: new Vector2(ToLeft ? 1 : -1, 1), color: Color.White);
        }

        public void DrawSpot(GameTime time) {
            Program.Thread.SpriteBatch.Draw(Program.Thread.SpotTexture, Position, origin: new Vector2(Program.Thread.SpotTexture.Width / 2f, Program.Thread.SpotTexture.Height / 2f), scale: new Vector2(1f), color: SimpleUtils.ColorFromHSV(Hue, 0.8f, 1f), rotation: SimpleUtils.Random((float)Math.PI * 2));
        }
    }
}
