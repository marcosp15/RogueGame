using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueGame.Entities
{
    public interface IEntity
    {
        Vector2 Position {get; set;}
        Vector2 Velocity {get; set;}
        float Width {get;}
        float Height {get;}

        void Update(GameTime gameTime, Player player);
        void Draw(SpriteBatch spriteBatch);

        Rectangle GetBounds();
    }
}