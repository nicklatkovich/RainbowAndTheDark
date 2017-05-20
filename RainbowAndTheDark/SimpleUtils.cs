using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowAndTheDark {
    public static class SimpleUtils {

        private static Random _rand = new Random( );

        public static KeyboardState GetKeyboardState( ) {
            return Keyboard.GetState( );
        }

        public static UInt32 IRandom(UInt32 maxValue) {
            return (UInt32)_rand.Next((Int32)maxValue);
        }

    }
}
