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

        public static UPoint Zero {
            get {
                return new UPoint(0u, 0u);
            }
        }

    }
}
