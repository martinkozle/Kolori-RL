using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintCircles
    {
        private static List<PaintCircle> _paintCircles;

        public PaintCircles()
        {
            _paintCircles = new List<PaintCircle>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var paintCircle in _paintCircles)
            {
                paintCircle.Update(gameTime);
            }

            _paintCircles = _paintCircles.Where(pc => !pc.IsDone()).ToList();
        }

        public void Draw(Kolori game)
        {
            game.ShapeBatch.Begin();
            foreach (var paintCircle in _paintCircles)
            {
                paintCircle.Draw(game);
            }

            game.ShapeBatch.End();
        }

        public void Add(PaintCircle paintCircle)
        {
            _paintCircles.Add(paintCircle);
        }
    }
}