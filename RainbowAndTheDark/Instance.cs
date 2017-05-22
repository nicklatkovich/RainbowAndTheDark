using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainbowAndTheDark {
    public abstract class Instance {

        public Vector2 Position;
        public bool IsFirstStep { get; private set; } = true;
        public bool IsFirstDraw { get; private set; } = true;
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
            IsFirstStep = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime time) {
            IsFirstDraw = false;
        }

    }
}
