using System.Collections.Generic;
using GJP2021.Sources.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.GameStates
{
    public class MenuState : IGameState
    {
        public static readonly MenuState Instance = new();
        private Kolori _game;
        private static readonly Color BgColor = new(9F / 255F, 10F / 255F, 20F / 255F);
        private readonly List<Button> _buttons = new();
        private Texture2D _logoTexture;

        public void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _game.SpriteBatch.Begin();

            var logoX = (_game.GetWindowWidth() - _logoTexture.Width) / 2;
            _game.SpriteBatch.Draw(_logoTexture, new Vector2(logoX, 64), Color.White);

            foreach (var button in _buttons)
            {
                button.DrawPositioned(_game.SpriteBatch);
            }

            _game.SpriteBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _game = game;
            _logoTexture = Kolori.Instance.TextureMap["logo"];

            //Start
            _buttons.Add(Button.Builder()
                .SetPosition(0, _logoTexture.Height + 64 + 64)
                .CenterHorizontally(_game.GetWindowWidth)
                //.CenterVertically(() => _game.GetWindowHeight() - 96 + 192)
                .SetSound("button")
                .SetTexture("start")
                .SetAction(
                    () => { _game.GameStateManager.SetGameState(IngameState.Instance); })
                .Build()
            );

            //Exit
            _buttons.Add(Button.Builder()
                .SetPosition(0, _logoTexture.Height + 64 + 64 + _buttons[^1].CurrentTexture.Height + 32)
                .CenterHorizontally(_game.GetWindowWidth)
                //.CenterVertically(() => _game.GetWindowHeight() + 96 + 192)
                .SetSound("button")
                .SetTexture("exit")
                .SetAction(
                    () => { _game.Exit(); })
                .Build()
            );
        }
    }
}