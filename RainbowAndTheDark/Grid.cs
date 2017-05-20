using Microsoft.Xna.Framework;
using System;

namespace RainbowAndTheDark {
    public class Grid<T> {

        private T[][] _arr;
        public UPoint Size { get; protected set; }
        public UInt32 Width {
            get {
                return this.Size.X;
            }
        }
        public UInt32 Height {
            get {
                return this.Size.Y;
            }
        }

        public Grid(UPoint size, T defaultValue) {
            this._arr = new T[size.X][];
            for (UInt32 i = 0; i < size.X; i++) {
                this._arr[i] = new T[size.Y];
                for (UInt32 j = 0; j < size.Y; j++) {
                    this._arr[i][j] = defaultValue;
                }
            }
            this.Size = size;
        }

        public T this[UPoint pos] {
            get {
                return this[pos.X, pos.Y];
            }
            set {
                this[pos.X, pos.Y] = value;
            }
        }
        public T this[UInt32 X, UInt32 Y] {
            get {
                return this._arr[X][Y];
            }
            set {
                this._arr[X][Y] = value;
            }
        }

        public Grid(UInt32 width, UInt32 height, T defaultValue) : this(new UPoint(width, height), defaultValue) {

        }

    }
}
