using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueGame.Entities
{
    public abstract class Entity : IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Width { get; protected set; }
        public float Height { get; protected set; }

        protected Texture2D Texture;  // Textura de la entidad

        public Entity(Texture2D texture, Vector2 startPosition)
        {
            Texture = texture;
            Position = startPosition;
            Width = texture.Width;
            Height = texture.Height;
        }

        // Método abstracto para obligar a las subclases a definir su lógica de actualización
        public abstract void Update(GameTime gameTime, Player player);
        public abstract bool IsAlive();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
        }

        public abstract void TakeDamage(int damage);
        public void SetPosition(Vector2 pos)
        {
            Position = pos;
        }

    }

}