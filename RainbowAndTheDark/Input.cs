using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RainbowAndTheDark {
    public static class Input {

        public static KeyboardState Keyboard { get; private set; }
        public static KeyboardState KeyboardPrevious { get; private set; }

        public static void PreUpdate(GameTime time) {
            KeyboardPrevious = Keyboard;
            Keyboard = Utils.GetKeyboardState( );
        }

        public static bool IsKeyPressed(Keys key) {
            return Keyboard.IsKeyDown(key) && KeyboardPrevious.IsKeyUp(key);
        }

    }
}
