using System.Collections.Generic;
using GJP2021.Content;
using GJP2021.Sources.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GJP2021.Sources.GameStates
{
    public class GameOverState : IGameState
    {
        public static readonly GameOverState Instance = new();
        private static readonly Color BgColor = new(9F / 255F, 10F / 255F, 20F / 255F);
        private List<Button> _buttons;
        private string _text;
        private float _textX;
        private float _textY;

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

            var logoTexture = GetLogoTexture();

            var logoX = (Kolori.Instance.GetWindowWidth() - logoTexture.Width) / 2;
            Kolori.Instance.SpriteBatch.Draw(logoTexture, new Vector2(logoX, 64), Color.White);

            Utils.DrawOutlinedText("Fonts/lunchds", 48, _text, new Vector2(_textX, _textY), Color.Crimson, Color.Black);

            foreach (var button in _buttons)
            {
                button.Draw();
            }

            Kolori.Instance.SpriteBatch.End();
        }

        public void Initialize()
        {
            _buttons = new()
            {
                //Start
                Button.Builder()
                    .SetPosition(0, Kolori.Instance.GetWindowHeight() - 128 - 64 - 16 - 64)
                    .CenterHorizontally(Kolori.Instance.GetWindowWidth)
                    //.CenterVertically(() => Kolori.Instance.GetWindowHeight() - 96 + 192)
                    .SetSound("button")
                    .SetTexture("restart")
                    .SetAction(
                        () => Kolori.Instance.GameStateManager.SetGameState(IngameState.Instance))
                    .Build(),
                //Exit
                Button.Builder()
                    .SetPosition(0, Kolori.Instance.GetWindowHeight() - 128 - 64)
                    .CenterHorizontally(Kolori.Instance.GetWindowWidth)
                    //.CenterVertically(() => Kolori.Instance.GetWindowHeight() + 96 + 192)
                    .SetSound("button")
                    .SetTexture("exit")
                    .SetAction(
                        () => Kolori.Instance.Exit())
                    .Build()
            };
        }

        private static Texture2D GetLogoTexture()
        {
            return Kolori.Instance.TextureMap["game_over"];
        }

        public void SetFinalScore(int playerScore)
        {
            var logoTexture = GetLogoTexture();
            _text = "Final Score: " + playerScore;
            var textSize = Font.MeasureString("Fonts/lunchds", 48, _text);

            _textX = (Kolori.Instance.GetWindowWidth() - textSize.X) / 2;
            _textY = 64 + logoTexture.Height + 64;
        }
    }
}