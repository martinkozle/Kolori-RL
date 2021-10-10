using System.Collections.Generic;
using GJP2021.Sources.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GJP2021.Sources.GameStates
{
    public class MenuState : IGameState
    {
        public static readonly MenuState Instance = new();
        private static readonly Color BgColor = new(9F / 255F, 10F / 255F, 20F / 255F);
        private readonly List<Button> _buttons = new();
        private static Song _song;

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

            foreach (var button in _buttons)
            {
                button.Draw();
            }

            Kolori.Instance.SpriteBatch.End();
        }

        private static Texture2D GetLogoTexture()
        {
            return Kolori.Instance.TextureMap["logo"];
        }

        public void Initialize()
        {
            _song = Kolori.Instance.SongMap["bgm_start"];
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;

            var logoTexture = GetLogoTexture();
            //Start
            _buttons.Add(Button.Builder()
                .SetPosition(0, logoTexture.Height + 64 + 64)
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
                .SetPosition(0, logoTexture.Height + 64 + 64 + _buttons[^1].CurrentTexture.Height + 32)
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