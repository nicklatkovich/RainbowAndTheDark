using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RainbowAndTheDark {
    public class Resources {

        public static Resources Get { get; protected set; }

        static Resources( ) {
            Get = new Resources( );
        }

        protected Resources( ) {

        }

        public Sprite this[SPRITE sprite] {
            get {
                return Sprites[sprite];
            }
        }

        public SpriteFont this[FONT font] {
            get {
                return Fonts[font];
            }
        }

        public enum SPRITE {
            Player,
            Spot,
            SmallSpot,
            Wall,
            Eues,
        }

        public enum FONT {
            Fuehrer64,
        }

        private static Vector2 Center = new Vector2(0.5f);
        private static Vector2 LeftTop = Vector2.Zero;

        private static Tuple<SPRITE, string, Vector2>[ ] SpriteSources = new Tuple<SPRITE, string, Vector2>[ ] {
            new Tuple<SPRITE, string, Vector2>(SPRITE.Player, "Instances/Characters/Player", Center),
            new Tuple<SPRITE, string, Vector2>(SPRITE.Spot, "Instances/Colors/Spot", Center),
            new Tuple<SPRITE, string, Vector2>(SPRITE.SmallSpot, "Instances/Colors/SmallSpot", Center),
            new Tuple<SPRITE, string, Vector2>(SPRITE.Wall, "Walls/Wall0", LeftTop),
            new Tuple<SPRITE, string, Vector2>(SPRITE.Eues, "Instances/Characters/Enemy0", Center),
        };

        private static Tuple<FONT, string>[ ] FontSources = new Tuple<FONT, string>[ ] {
            new Tuple<FONT, string>(FONT.Fuehrer64, "Fonts/Fuehrer64"),
        };

        private static Dictionary<SPRITE, Sprite> Sprites = new Dictionary<SPRITE, Sprite>( );
        private static Dictionary<FONT, SpriteFont> Fonts = new Dictionary<FONT, SpriteFont>( );

        public static Sprite GetSprite(SPRITE sprite) {
            return Sprites[sprite];
        }

        public static void LoadContent(ContentManager contentManager) {
            foreach (var a in SpriteSources) {
                Sprites.Add(a.Item1, new Sprite(contentManager.Load<Texture2D>(a.Item2), a.Item3));
            }
            foreach (var a in FontSources) {
                Fonts.Add(a.Item1, contentManager.Load<SpriteFont>(a.Item2));
            }
        }

    }
}
