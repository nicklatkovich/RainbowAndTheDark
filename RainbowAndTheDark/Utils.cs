using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowAndTheDark {
    public static class Utils {

        private static Random _rand = new Random( );

        public static KeyboardState GetKeyboardState( ) {
            return Keyboard.GetState( );
        }

        public static UInt32 IRandom(UInt32 maxValue) {
            return (UInt32)_rand.Next((Int32)maxValue);
        }

        public static float Random(float maxValue = 1f) {
            return (float)_rand.NextDouble( ) * maxValue;
        }

        public static readonly Int32[ ] Dx = new Int32[ ] { 1, 0, -1, 0 };
        public static readonly Int32[ ] Dy = new Int32[ ] { 0, -1, 0, 1 };

        public static Point Dd(UInt32 dir) {
            return new Point(Dx[dir % 4], Dy[dir % 4]);
        }

        public static uint ShortestWayBFS(UPoint from, UPoint to, Grid<uint> map) {
            Grid<uint> temp = new Grid<uint>(map.Size, uint.MaxValue);
            temp[from] = 0;
            Queue<UPoint> q = new Queue<UPoint>( );
            q.Enqueue(from);
            while (q.Count > 0 && temp[to] == uint.MaxValue) {
                UPoint p = q.Dequeue( );
                uint l = temp[p] + 1;
                for (uint d = 0; d < 4; d++) {
                    UPoint near = (UPoint)(p + Dd(d));
                    if (map[near] == 0 && temp[near] > l) {
                        temp[near] = l;
                        q.Enqueue(near);
                    }
                }
            }
            return temp[to];
        }

        public static Grid<uint> CreateMapFromMaze(Grid<uint> maze) {
            Grid<uint> result = maze.Copy( );
            while (true) {
                bool exit = true;
                uint maxDistance = uint.MinValue;
                UPoint wallToDelete = default(UPoint);
                for (uint i = 1; i < result.Width - 1; i++) {
                    for (uint j = 1; j < result.Height - 1; j++) {
                        if (result[i, j] == 1) {
                            UPoint[ ] near = new UPoint[4];
                            uint near_size = 0;
                            for (uint d = 0; d < 4; d++) {
                                UPoint new_pos = new UPoint((uint)(i + Dx[d]), (uint)(j + Dy[d]));
                                if (result[new_pos] == 0) {
                                    near[near_size++] = new_pos;
                                }
                            }
                            uint min_dist = uint.MaxValue;
                            for (uint l = 0; l < near_size; l++) {
                                for (uint k = l + 1; k < near_size; k++) {
                                    uint dist = ShortestWayBFS(near[l], near[k], result);
                                    if (min_dist > dist) {
                                        min_dist = dist;
                                    }
                                }
                            }
                            if (min_dist != uint.MaxValue && min_dist > 2 && (exit || (maxDistance < min_dist))) {
                                maxDistance = min_dist;
                                wallToDelete = new UPoint(i, j);
                                exit = false;
                            }
                        }
                    }
                }
                if (exit) {
                    break;
                } else {
                    result[wallToDelete] = 0;
                }
            }
            return result;
        }

        public static Tuple<Grid<uint>, UPoint> CreateMaze(UPoint size) {
            Grid<uint> result = new Grid<uint>(size, 3);
            result.SetRegion(new UPoint(1, 1), new UPoint(
                result.Width - 1,
                result.Height - 1), 0);
            UPoint start_pos = new UPoint(
                IRandom(result.Width - 2) + 1,
                IRandom(result.Height - 2) + 1);
            result[start_pos] = 1;
            UPoint[ ] queue = new UPoint[(result.Width - 2) * (result.Height - 2)];
            queue[0] = start_pos;
            UInt32 queue_size = 1;
            while (queue_size > 0) {
                UInt32 queue_pos = IRandom(queue_size);
                UPoint pos = queue[queue_pos];
                queue_size--;
                queue[queue_pos] = queue[queue_size];
                if (result[pos] == 1) {
                    result[pos] = 2;
                    for (uint i = 0; i < 4; i++) {
                        Point d = Dd(i);
                        UPoint new_pos = (UPoint)(pos + d);
                        switch (result[new_pos]) {
                            case 1:
                                result[new_pos] = 3;
                                break;
                            case 0:
                                result[new_pos] = 1;
                                queue[queue_size++] = new_pos;
                                break;
                        }
                    }
                }
            }
            for (uint i = 0; i < result.Width; i++) {
                for (uint j = 0; j < result.Height; j++) {
                    if (result[i, j] == 2) {
                        result[i, j] = 0;
                    } else {
                        result[i, j] = 1;
                    }
                }
            }
            return new Tuple<Grid<uint>, UPoint>(result, start_pos);
        }

        public static Color ColorFromHSV(float hue, float saturation, float value) {
            float hue6 = hue * 6f;
            int hi = Convert.ToInt32(Math.Floor(hue6)) % 6;
            double f = hue6 - Math.Floor(hue6);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi) {
                case 0:
                    return new Color(v, t, p);
                case 1:
                    return new Color(q, v, p);
                case 2:
                    return new Color(p, v, t);
                case 3: return new Color(p, q, v);
                case 4: return new Color(t, p, v);
                default: return new Color(v, p, q);
            }
        }

        public static bool PlaceFree(Vector2 pos, Vector2 maskHalfSize, Grid<uint> map, UPoint cellSize) {
            for (uint
                i = (uint)(pos.X - maskHalfSize.X) / cellSize.X,
                iTo = (uint)(pos.X + maskHalfSize.X) / cellSize.X; i <= iTo; i++) {
                for (uint
                    j = (uint)(pos.Y - maskHalfSize.Y) / cellSize.Y,
                    jTo = (uint)(pos.Y + maskHalfSize.Y) / cellSize.Y; j <= jTo; j++) {
                    if (map[i, j] > 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static Vector2 Abs(this Vector2 v) {
            return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        }

        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color, Vector2 scale, float rotation = 0f) {
            spriteBatch.Draw(sprite.Texture, position, origin: sprite.Offset, rotation: rotation, color: color, scale: scale);
        }
        
        public static void DrawSprite(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color? color = null, Vector2? scale = null, float rotation = 0f) {
            spriteBatch.DrawSprite(sprite, position, color == null ? Color.White : color.Value, scale == null ? new Vector2(1f) : scale.Value, rotation);
        }

        public static readonly float TWO_PI = (float)(Math.PI * 2);

    }
}
