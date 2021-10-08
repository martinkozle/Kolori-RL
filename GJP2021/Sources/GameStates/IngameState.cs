using Microsoft.Xna.Framework;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new();
        private Kolori _game;
        private static readonly Color BgColor = new(115F/255F, 190F/255F, 211F/255F);

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _game.SpriteBatch.Begin();

            //TODO DRAWING GOES HERE
            
            _game.SpriteBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _game = game;
        }
        
    }
    
}