using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Entities;
using RogueGame.Rooms;

namespace JuegoHorda.Entities
{
    public class Proyectil
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        private int Damage;

        public Proyectil(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage)
        {
            Texture = texture;
            Position = position;
            Direction = direction;
            Speed = speed;
            Damage = damage;
        }

        // Método para mover el proyectil
        public int Update(GameTime gameTime, List<Entity> enemigos)
        {
            Position += Direction * Speed;
            foreach (var enemigo in enemigos)
            {
                if (enemigo.IsAlive() && Hitbox.Intersects(enemigo.GetBounds()))
                {
                    enemigo.TakeDamage(Damage);
                    return 1;
                }
            }
            if (Position.X < 0 || Position.X > Data.ScreenW || Position.Y < 0 || Position.Y > Data.ScreenH)
            {
                // Si el proyectil está fuera de los límites, se destruye
                return 1;
            }
            return 0;
        }
    }
}
