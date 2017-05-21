using System;

namespace RainbowAndTheDark {
#if WINDOWS || LINUX
    public static class Program {

        public static MainThread Thread;

        [STAThread]
        static void Main( ) {
            using (Thread = new MainThread( ))
                Thread.Run( );
        }
    }
#endif
}
