using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public abstract class Instance {

        public Vector2 Position;
        public Vector2 PositionPrevious {
            get;
            protected set;
        }

        public Instance(Vector2 position) {
            this.Position = position;
            this.PositionPrevious = this.Position;
        }

        public virtual void Update(GameTime time) {
            this.PositionPrevious = this.Position;
        }

        public abstract void Draw(GameTime time, RenderTarget2D surface);

    }
}
