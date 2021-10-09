using System.Collections.Generic;
using GJP2021.Sources.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.GameStates
{
    public class MenuState : IGameState
    {
        public static readonly MenuState Instance = new();
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
            Kolori.Instance.GraphicsDevice.Clear(BgColor);

            Kolori.Instance.SpriteBatch.Begin();

            var logoX = (Kolori.Instance.GetWindowWidth() - _logoTexture.Width) / 2;
            Kolori.Instance.SpriteBatch.Draw(_logoTexture, new Vector2(logoX, 64), Color.White);

            foreach (var button in _buttons)
            {
                button.DrawPositioned(Kolori.Instance.SpriteBatch);
            }

            Kolori.Instance.SpriteBatch.End();
        }

        public void Initialize()
        {
            _logoTexture = Kolori.Instance.TextureMap["logo"];

            //Start
            _buttons.Add(Button.Builder()
                .SetPosition(0, _logoTexture.Height + 64 + 64)
                .CenterHorizontally(Kolori.Instance.GetWindowWidth)
                //.CenterVertically(() => Kolori.Instance.GetWindowHeight() - 96 + 192)
                .SetSound("button")
                .SetTexture("start")
                .SetAction(
                    () => { Kolori.Instance.GameStateManager.SetGameState(IngameState.Instance); })
                .Build()
            );

            //Exit
            _buttons.Add(Button.Builder()
                .SetPosition(0, _logoTexture.Height + 64 + 64 + _buttons[^1].CurrentTexture.Height + 32)
                .CenterHorizontally(Kolori.Instance.GetWindowWidth)
                //.CenterVertically(() => Kolori.Instance.GetWindowHeight() + 96 + 192)
                .SetSound("button")
                .SetTexture("exit")
                .SetAction(
                    () => { Kolori.Instance.Exit(); })
                .Build()
            );
        }
    }
}