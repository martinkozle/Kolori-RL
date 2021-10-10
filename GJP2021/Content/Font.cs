/*
Copyright (c) 2021 João Godinho

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Content
{
    internal class Font
    {
        private static SpriteBatch _spriteBatch;
        private static ContentManager _contentManager;
        private static List<Font> _fonts;

        // Initialize font manager
        public static void Initialize(SpriteBatch batch, ContentManager content)
        {
            _spriteBatch = batch;
            _contentManager = content;
            _fonts = new List<Font>();
        }

        // Load an array of font sizes for a given font family
        public static void LoadSizes(string fontName, IEnumerable<int> sizes)
        {
            foreach (var size in sizes)
            {
                _fonts.Add(new Font(fontName, size));
            }
        }
        
        // Regular DrawString function
        public static void DrawString(string fontName, int size, string text, Vector2 position, Color color)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color);
            }
        }

        // Regular DrawString function with automaticScale parameter as fallback
        public static void DrawString(string fontName, int size, string text, Vector2 position, Color color, bool automaticScale)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color);
            }
            else if (automaticScale)
            {
                var closerFont = _fonts.Aggregate((x, y) => Math.Abs(x.Size - size) < Math.Abs(y.Size - size) ? x : y);
                var scalingFactor = (float)size / closerFont.Size;

                _spriteBatch.DrawString(closerFont.SpriteFont, text, position, color, 0, new Vector2(0,0), scalingFactor, SpriteEffects.None, 0);
            }
        }

        // Extended DrawString function with Vector2 scale
        public static void DrawString(string fontName, int size, string text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, scale, fx, depth);
            }
        }

        // Extended DrawString function with Single scale
        public static void DrawString(string fontName, int size, string text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, scale, fx, depth);
            }
        }

        // Extended DrawString function with automaticScale parameter as fallback
        public static void DrawString(string fontName, int size, string text, Vector2 position, Color color, bool automaticScale, Single rotation, Vector2 origin, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, 1, fx, depth);
            }
            else if (automaticScale)
            {
                var closerFont = _fonts.Aggregate((x, y) => Math.Abs(x.Size - size) < Math.Abs(y.Size - size) ? x : y);
                var scalingFactor = (float)size / closerFont.Size;

                _spriteBatch.DrawString(closerFont.SpriteFont, text, position, color, rotation, origin, scalingFactor, fx, depth);
            }
        }

        // Regular DrawString function with StringBuilder text
        public static void DrawString(string fontName, int size, StringBuilder text, Vector2 position, Color color)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color);
            }
        }

        // Regular DrawString function with StringBuilder text and automaticScale parameter as fallback
        public static void DrawString(string fontName, int size, StringBuilder text, Vector2 position, Color color, bool automaticScale)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color);
            }
            else if (automaticScale)
            {
                var closerFont = _fonts.Aggregate((x, y) => Math.Abs(x.Size - size) < Math.Abs(y.Size - size) ? x : y);
                var scalingFactor = (float)size / closerFont.Size;

                _spriteBatch.DrawString(closerFont.SpriteFont, text, position, color, 0, new Vector2(0, 0), scalingFactor, SpriteEffects.None, 0);
            }
        }

        // Extended DrawString function with StringBuilder text and Vector2 scale
        public static void DrawString(string fontName, int size, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Vector2 scale, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, scale, fx, depth);
            }
        }

        // Extended DrawString function with String Builder text and Single scale
        public static void DrawString(string fontName, int size, StringBuilder text, Vector2 position, Color color, Single rotation, Vector2 origin, Single scale, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, scale, fx, depth);
            }
        }

        // Extended DrawString function with String Builder text and automaticScale as fallback
        public static void DrawString(string fontName, int size, StringBuilder text, Vector2 position, Color color, bool automaticScale, Single rotation, Vector2 origin, SpriteEffects fx, Single depth)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            if (font != null)
            {
                _spriteBatch.DrawString(font.SpriteFont, text, position, color, rotation, origin, 1, fx, depth);
            }
            else if (automaticScale)
            {
                var closerFont = _fonts.Aggregate((x, y) => Math.Abs(x.Size - size) < Math.Abs(y.Size - size) ? x : y);
                var scalingFactor = (float)size / closerFont.Size;

                _spriteBatch.DrawString(closerFont.SpriteFont, text, position, color, rotation, origin, scalingFactor, fx, depth);
            }
        }

        // Retrieve all available sizes for a given font family
        public static List<Font> GetAvailableSizes(string fontName)
        {
            return _fonts.FindAll(f => f.FontName == fontName);
        }

        // Regular measure string by Font parameter
        public static Vector2 MeasureString(Font font, string text)
        {
            return font.SpriteFont.MeasureString(text);
        }

        // Regular measure string by font name and size
        public static Vector2 MeasureString(string fontName, int size, string text)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            return font?.SpriteFont.MeasureString(text) ?? new Vector2(0, 0);
        }

        // Regular measure string by font name and size with StringBuilder text
        public static Vector2 MeasureString(string fontName, int size, StringBuilder text)
        {
            var font = _fonts.Find(f => f.FontName == fontName && f.Size == size);

            return font?.SpriteFont.MeasureString(text) ?? new Vector2(0, 0);
        }

        // Font constructor
        public Font(string fontName, int size)
        {
            SpriteFont = _contentManager.Load<SpriteFont>($"{fontName}_{size}");
            Size = size;
            FontName = fontName;
        }

        // Font getters
        public string FontName { get; }
        public int Size { get; }
        public SpriteFont SpriteFont { get; }
    }
}