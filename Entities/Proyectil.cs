using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoHorda.Entities
{
    public class Proyectil
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public Proyectil(Texture2D texture, Vector2 position, Vector2 direction, float speed)
        {
            Texture = texture;
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        // Método para mover el proyectil
        public void Update()
        {
            Position += Direction * Speed;
        }
    }
}
