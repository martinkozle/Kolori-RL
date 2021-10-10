using System;
using GJP2021.Content;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources
{
    public static class Utils
    {
        public static void DrawOutlinedText(string font, int size, string text, Vector2 position, Color frontColor,
            Color backColor, HorizontalFontAlignment horizontalFontAlignment = HorizontalFontAlignment.LEFT, VerticalFontAlignment verticalFontAlignment = VerticalFontAlignment.TOP)
        {
            var (x, y) = Font.MeasureString(font, size, text);

            x = horizontalFontAlignment switch
            {
                HorizontalFontAlignment.CENTER => -x / 2F,
                HorizontalFontAlignment.RIGHT => -x,
                _ => 0
            };

            y = verticalFontAlignment switch
            {
                VerticalFontAlignment.CENTER => y / 2F,
                VerticalFontAlignment.BOTTOM => y,
                _ => 0
            };

            position += new Vector2(x, y);

            Font.DrawString(font, size, text, position + new Vector2(1, 1), backColor);
            Font.DrawString(font, size, text, position + new Vector2(-1, 1), backColor);
            Font.DrawString(font, size, text, position + new Vector2(1, -1), backColor);
            Font.DrawString(font, size, text, position + new Vector2(1, -1), backColor);

            Font.DrawString(font, size, text, position, frontColor);
        }

        public static bool IsInsideBox(Point point, Vector2 position, Vector2 size)
        {
            var (x, y) = point;
            var (boxX, boxY) = position;
            var (boxWidth, boxHeight) = size;
            return x >= boxX && y >= boxY && x <= boxX + boxWidth && y <= boxY + boxHeight;
        }
        
        public static bool IsInsideBox(Point point, int x, int y, int width, int height)
        {
            return IsInsideBox(point, new Vector2(x, y), new Vector2(width, height));
        }

        public enum HorizontalFontAlignment
        {
            LEFT,
            CENTER,
            RIGHT
        }

        public enum VerticalFontAlignment
        {
            TOP,
            CENTER,
            BOTTOM
        }
    }
}