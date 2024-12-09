using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RogueGame.Entities
{
    public abstract class Item : Entity
    {
        public bool IsCollected {get; set;}
        public Item(Texture2D texture, Microsoft.Xna.Framework.Vector2 startPosition) : base(texture, startPosition)
        {
            IsCollected = false;
        }
        
        public override void Update(GameTime gameTime, Player player)
        {
            if (IsCollected) return;

            float oscilacion = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds*5)/2;
            Position = new Vector2(Position.X, Position.Y + oscilacion);

            if (GetBounds().Intersects(player.GetBounds()))
            {
                IsCollected = true;
                ApllyEffect(player);
            }
        }

        public abstract void ApllyEffect(Player player);
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                spriteBatch.Draw(Texture,Position,Microsoft.Xna.Framework.Color.White);
            }
        }
    }
}