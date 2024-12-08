using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;

namespace RogueGame.Entities
{
    public class Boss0 : Enemy
    {
        private Vector2 _direction;
        public Boss0(Texture2D texture, Vector2 startPosition) 
            : base(texture, startPosition, health: 100, damage: 1, speed: 150f)
        {
            _direction = new Vector2(1,1);
            _direction.Normalize();
        }
        public override void Update(GameTime gameTime, Player player)
        {
            Move(gameTime, player);
            if (Health < 25) Speed = 250f;
            base.Update(gameTime, player);
        }

        protected override void Move(GameTime gameTime, Player player)
        {
            Position += _direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.X <= 0 || Position.X >= Data.ScreenW - Texture.Width)
            {
                _direction.X *= -1;  // Invierte la dirección horizontal
                float x = MathHelper.Clamp(Position.X, 0, Data.ScreenW - Texture.Width);
                Position = new Vector2(x,Position.Y);
                
            }

            if (Position.Y <= 0 || Position.Y >= Data.ScreenH - Texture.Height)
            {
                _direction.Y *= -1;  // Invierte la dirección vertical
                float y = MathHelper.Clamp(Position.Y, 0, Data.ScreenH - Texture.Height);
                Position = new Vector2(Position.X,y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}