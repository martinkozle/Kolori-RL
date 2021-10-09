using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources
{
    public static class Utils
    {
        public static void DrawOutlinedText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position,
            Color backColor, Color frontColor, float scale = 1F)
        {
            var origin = Vector2.Zero;

            spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, 1 * scale), backColor, 0, origin,
                scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, 1 * scale), backColor, 0, origin,
                scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, -1 * scale), backColor, 0, origin,
                scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, -1 * scale), backColor, 0, origin,
                scale, SpriteEffects.None, 1f);

            spriteBatch.DrawString(font, text, position, frontColor, 0, origin, scale, SpriteEffects.None, 0f);
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