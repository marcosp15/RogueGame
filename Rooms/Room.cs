using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Entities;

namespace RogueGame.Rooms
{
    public class Room
    {
        private Room[] habitaciones;
        private List<Enemy> enemigos;
        public bool IsCleared => enemigos.TrueForAll(e => !e.IsAlive);
        public Room()
        {
            habitaciones = new Room[4];
            enemigos = new List<Enemy>();
        }

        public void AttachRoom(Room newRoom, int pos)
        {
            if (pos >= 0 && pos < 4 && habitaciones[pos] == null)
                habitaciones[pos] = newRoom;
        }

        public void AddEnemy(Enemy enemy)
        {
            enemigos.Add(enemy);
        }

        public void Update(GameTime gameTime, Player player)
        {
            foreach (var enemy in enemigos)
            {
                if (enemy.IsAlive)
                    enemy.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemy in enemigos)
            {
                if (enemy.IsAlive)
                    enemy.Draw(spriteBatch);
            }
        }
    }
}