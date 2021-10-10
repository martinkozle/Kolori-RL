using GJP2021.Content;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources
{
    public static class Utils
    {
        public static void DrawOutlinedText(string font, int size, string text, Vector2 position, Color frontColor,
            Color backColor)
        {
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
    }
}