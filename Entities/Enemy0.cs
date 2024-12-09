using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Entities;
using RogueGame.Rooms;

namespace RogueGame.Entities
{
    public class Enemy0 : Enemy
    {
        public Enemy0(RoomManager roomManager, Texture2D texture, Vector2 startPosition) 
            : base(roomManager, texture, startPosition, health: 3, damage: 1, speed: 50f)
        {

        }
        override public bool IsAlive()
        {
            return Health > 0;
        }

        public override void TakeDamage(int damage)
        {
            Health = System.Math.Max(Health - damage, 0);
        }

        protected override void Move(GameTime gameTime, Player player)
        {
            if (!IsAlive()) return;

            Vector2 direction = player.Position - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // Verificar colisión con el jugador
            if (GetBounds().Intersects(player.GetBounds()))
            {
                Attack(player);
            }
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, Data.ScreenW - Texture.Width),
                MathHelper.Clamp(Position.Y, 0, Data.ScreenH - Texture.Height)
            );
        }
        public override void Update(GameTime gameTime, Player player)
        {
            Move(gameTime, player);
            base.Update(gameTime, player);
        }

        // Sobrescribir el método de muerte para efectos específicos
        protected override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
