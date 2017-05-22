using Microsoft.Xna.Framework;
using System;

namespace RainbowAndTheDark {
    public struct UPoint {

        public UInt32 X;
        public UInt32 Y;

        public UPoint(UInt32 x, UInt32 y) {
            this.X = x;
            this.Y = y;
        }

        public UPoint(uint xy) : this(xy, xy) {

        }

        public static UPoint Zero {
            get {
                return new UPoint(0u, 0u);
            }
        }

        public static Point operator + (UPoint p1, Point p2) {
            return new Point((int)p1.X + p2.X, (int)p1.Y + p2.Y);
        }

        public static Point operator - (UPoint p1, UPoint p2) {
            return new Point((int)(p1.X - p2.X), (int)(p1.Y - p2.Y));
        }

        public static explicit operator UPoint(Vector2 v) {
            return new UPoint((uint)v.X, (uint)v.Y);
        }

        public static explicit operator UPoint(Point p) {
            return new UPoint((uint)Math.Abs(p.X), (uint)Math.Abs(p.Y));
        }

        public Vector2 ToVector2( ) {
            return new Vector2(X, Y);
        }

    }
}
