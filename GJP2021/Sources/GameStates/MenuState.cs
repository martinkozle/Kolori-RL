using GJP2021.Sources.GUI;
using Microsoft.Xna.Framework;

namespace GJP2021.Sources.GameStates
{
    public class MenuState : IGameState
    {
        public static readonly MenuState Instance = new();
        private Kolori _game;
        private static readonly Color BgColor = new(9F/255F, 10F/255F, 20F/255F);
        private Button _startButton;
        
        public void Update(GameTime gameTime)
        {
            _startButton.Update();
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _game.SpriteBatch.Begin();
            
            _startButton.DrawPositioned(_game.SpriteBatch);            

            _game.SpriteBatch.End();
        }
        
        public void Initialize(Kolori game)
        {
            _game = game;

            _startButton = Button.Builder()
                .SetPosition(16, 16)
                .CenterHorizontally(_game.GetWindowWidth)
                .SetSound("button")
                .SetAction(
                    () =>
                        {
                            _game.GameStateManager.SetGameState(IngameState.Instance);
                        })
                .Build();
        }

    }
    
}