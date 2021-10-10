using Microsoft.Xna.Framework;

namespace GJP2021.Sources.GameStates
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void Initialize();
    }
}