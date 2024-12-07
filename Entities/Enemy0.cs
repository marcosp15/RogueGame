using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Entities;

namespace RogueGame.Entities
{
    public class Enemy0 : Enemy
    {
        private Vector2 direction;

        public Enemy0(Texture2D texture, Vector2 startPosition) 
            : base(texture, startPosition, health: 3, damage: 1, speed: 100f)
        {
            direction = new Vector2(1, 0);  // Comienza moviéndose hacia la derecha
        }

        protected override void Move(GameTime gameTime)
        {
            // Movimiento de izquierda a derecha y rebote
            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Lógica para cambiar de dirección al tocar los bordes de la pantalla
            if (Position.X <= 0 || Position.X + Width >= Data.ScreenW)
            {
                direction.X *= -1;  // Invierte la dirección horizontal
            }
        }

        // Sobrescribir el método de muerte para efectos específicos
        protected override void OnDeath()
        {
            base.OnDeath();
        }
    }
}
