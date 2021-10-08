using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new();
        private Kolori _game;
        private static readonly Color BgColor = Color.White;
        private static PaintCircles _paintCircles;
        private static PaintPeriodicSpawner _periodicPaintSpawner;

        public void Update(GameTime gameTime)
        {
            _paintCircles.Update(gameTime);
            var mouseX = Mouse.GetState().X;
            var mouseY = Mouse.GetState().Y;
            _periodicPaintSpawner.Update(gameTime, _paintCircles, mouseX, mouseY);
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            //TODO DRAWING GOES HERE

            _paintCircles.Draw(_game);
        }

        public void Initialize(Kolori game)
        {
            _game = game;
            _paintCircles = new PaintCircles();
            _periodicPaintSpawner = new PaintPeriodicSpawner(PaintCircle.Red, 25, 5, 20, 0.05F, 0.1F,5);
        }
    }
}