using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Entities;

namespace RogueGame.Rooms
{
    public class Room
    {
        private Room[] habitaciones;
        private List<Enemy> enemigos;
        private Texture2D _gameAreaTexture;
        public bool IsCleared => enemigos.TrueForAll(e => !e.IsAlive);
        public string nombre;
        public Room(int numero)
        {
            habitaciones = new Room[4];
            enemigos = new List<Enemy>();
            nombre = numero.ToString();
        }

        public void AttachRoom(Room newRoom, int pos)
        {
            if (pos >= 0 && pos < 4 && habitaciones[pos] == null)
            {
                habitaciones[pos] = newRoom;
                newRoom.habitaciones[(pos+2)%4] = this;
            }
        }

        public Room GetConnectedRoom(int pos)
        {
            if (pos >= 0 && pos < 4)
                return habitaciones[pos];
            return null;
        }

        public void AddEnemy(Enemy enemy)
        {
            enemigos.Add(enemy);
        }

        public void LoadContent(ContentManager content)
        {
            _gameAreaTexture = content.Load<Texture2D>("arenabackground");
        }

        public void Update(GameTime gameTime, Player player)
        {
            foreach (var enemy in enemigos)
            {
                if (enemy.IsAlive)
                    enemy.Update(gameTime,player);
                foreach (var otherEnemy in enemigos)
                {
                    if (enemy != otherEnemy && enemy.GetBounds().Intersects(otherEnemy.GetBounds()))
                    {
                        AvoidEnemyOverlap(enemy, otherEnemy);
                    }
                }
            }
            player.UpdateProyectiles(gameTime,enemigos);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dibujar el área de juego
            spriteBatch.Draw(_gameAreaTexture, new Rectangle(0, 0, Data.ScreenW, Data.ScreenH), Color.White);

            foreach (var enemy in enemigos)
            {
                if (enemy.IsAlive)
                    enemy.Draw(spriteBatch);
            }
        }

        private void AvoidEnemyOverlap(Enemy enemy, Enemy otherEnemy)
        {
            Vector2 enemyCenter = enemy.Position + new Vector2(enemy.Width / 2, enemy.Height / 2);
            Vector2 otherEnemyCenter = otherEnemy.Position + new Vector2(otherEnemy.Width / 2, otherEnemy.Height / 2);
            
            Vector2 direction = enemyCenter - otherEnemyCenter;
            float distance = direction.Length(); // Distancia entre los centros

            float collisionRadius = (enemy.Width / 2 + otherEnemy.Width / 2);

            if (distance < collisionRadius)
            {
                direction.Normalize();

                // Desplazar los enemigos fuera de la zona de solapamiento
                float overlap = collisionRadius - distance;
                Vector2 pushBack = direction * overlap;

                enemy.SetPosition(enemy.Position + pushBack);
                otherEnemy.SetPosition(otherEnemy.Position - pushBack);
            }
        }

        public List<Enemy> getEnemies()
        {
            return enemigos;
        }

        public Room GetRoom(int i)
        {
            return habitaciones[i];
        }
    }
}