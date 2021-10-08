using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Characters
{
    public class Enemy
    {
        private Enemy _enemy;
        private float xPos;
        private float yPos;
        private float speed;
        private Kolori game;

        public Enemy(float xPos, float yPos, float speed, Kolori game)
        {
            this.speed = speed;
            this.xPos = xPos;
            this.yPos = yPos;
            this.game = game;
        }

        public void Update(GameTime gameTime, float playerPosX, float playerPosY)
        {
            var delta = gameTime.ElapsedGameTime.Milliseconds/1000F *60;
            var width = Math.Abs(this.xPos - playerPosX);
            var height = Math.Abs(this.yPos - playerPosY);
            var h = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            if (playerPosX == this.xPos && playerPosY == this.yPos)
            {
                return;
            }
            if (playerPosX > this.xPos)
            {
                this.xPos = (this.xPos + this.speed * (width / h)*delta);
            }
            //this.yPos += this.speed * (width / (width + height));
            //this.xPos += this.speed * (height / (width + height));

            else if (playerPosX < this.xPos)
            {
                this.xPos = (this.xPos - this.speed * (width / h)*delta);
            }

//
            if (playerPosY > this.yPos)
            {
                this.yPos = (this.yPos + this.speed * (height / h)*delta);
            }
//
            else if (playerPosY < this.yPos)
            {
                this.yPos = (this.yPos - this.speed * (height / h)*delta);
            }

            //this.xPos = MathHelper.Lerp(this.xPos, playerPosX, this.lerp);
            //this.yPos = MathHelper.Lerp(this.yPos, playerPosY, this.lerp);
        }

        public void Draw(GameTime gameTime)
        {
            this.game.SpriteBatch.Draw(Kolori.TextureMap["enemy"], new Vector2(this.xPos, this.yPos), null,
                Color.White);
        }

        public void Initialize(Enemy enemy)
        {
            _enemy = enemy;
        }
    }
}