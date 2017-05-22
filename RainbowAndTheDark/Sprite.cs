using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowAndTheDark {
    public class Sprite {

        public Texture2D Texture { get; protected set; }
        public Vector2 Offset { get; protected set; }
        public Vector2 Size { get; protected set; }
        public float Width { get { return Texture.Width; } }
        public float Height { get { return Texture.Height; } }

        public Sprite(Texture2D texture, Vector2 relativeOffset) {
            this.Texture = texture;
            this.Size = new Vector2(Texture.Width, Texture.Height);
            this.Offset = relativeOffset * Size;
        }

    }
}
