using GJP2021.Sources.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.GameStates
{
    public class IngameState : IGameState
    {
        public static readonly IngameState Instance = new();
        private Kolori _game;
        private Player _player;
        private static readonly Color BgColor = new(115F/255F, 190F/255F, 211F/255F);
        private Enemy _enemy;
        

        public void Update(GameTime gameTime)
        {
            _enemy.Update(gameTime,Mouse.GetState().X,Mouse.GetState().Y);
            _player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(BgColor);

            _game.SpriteBatch.Begin();
            
            _enemy.Draw(gameTime);
            //TODO DRAWING GOES HERE
            _player.DrawPositioned(_game.SpriteBatch);
            
            _game.SpriteBatch.End();
        }

        public void Initialize(Kolori game)
        {
            _game = game;
            _enemy=new Enemy(200,200,3F,game,0.025F);
            _player = Player.Builder()
                            .SetPosition(0, 0)
                            .SetSpeed(2f)
                            .SetMaxAcceleration(6f)
                            .SetBounds(new Vector2(_game.Graphics.PreferredBackBufferWidth, _game.Graphics.PreferredBackBufferHeight))
                            .Build();
        }
        
    }
    
}