using Microsoft.Xna.Framework;

namespace GJP2021.Sources.GameStates
{
    public class GameStateManager
    {
        private IGameState _currentGameState;

        public GameStateManager()
        {
            SetGameState(MenuState.Instance);
        }

        public void Update(GameTime gameTime)
        {
            _currentGameState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentGameState.Draw(gameTime);
        }

        public void SetGameState(IGameState state, bool reset = true)
        {
            _currentGameState = state;
            if (reset)
            {
                _currentGameState.Initialize();
            }
        }
    }
}