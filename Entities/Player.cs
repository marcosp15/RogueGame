using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueGame.Entities;

namespace RogueGame.Entities
{
    public class Player : Entity
    {
        public float Speed { get; set; } = 3f;
        public int Health { get; set; } = 3;
        public bool IsAlive => Health > 0;

        public Player(Texture2D texture, Vector2 startPosition) 
            : base(texture, startPosition) { }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D)) movement.X += 1;

            if (movement != Vector2.Zero)
            {
                movement.Normalize();  // Para evitar moverse más rápido en diagonal
                Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        internal void TakeDamage(int damage)
        {
            Health = Math.Max(Health - damage, 0);
        }

         public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawHealthBar(spriteBatch);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            Texture2D healthBar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            healthBar.SetData(new[] { Color.White });

            Rectangle healthRect = new Rectangle((int)Position.X, (int)Position.Y - 10, Health * 20, 5);
            spriteBatch.Draw(healthBar, healthRect, Color.Red);
        }
    }
}
