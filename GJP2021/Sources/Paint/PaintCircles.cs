using System.Collections.Generic;
using LilyPath;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.Paint
{
    public class PaintCircles
    {
        private readonly List<PaintCircle> _paintCircles;

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

            _paintCircles.RemoveAll(pc => pc.IsDone());
        }

        public void Draw(DrawBatch batch)
        {
            batch.Begin();
            foreach (var paintCircle in _paintCircles)
            {
                paintCircle.Draw(batch);
            }

            batch.End();
        }

        public void Add(PaintCircle paintCircle)
        {
            _paintCircles.Add(paintCircle);
        }
    }
}