using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Entities;

namespace RogueGame.Rooms
{
    public class RoomManager
    {
        private List<Room> _rooms;
        private int _currentRoomIndex;
        private Room spawnRoom;
        Random random;

        public RoomManager()
        {
            spawnRoom = new Room();
            _currentRoomIndex = 0;
            _rooms = new List<Room>();
            random = new Random();
        }

        public void LoadContent(ContentManager content)
        {
            var enemyTexture = content.Load<Texture2D>("enemy0");

            // Crear habitaciones con enemigos aleatorios
            for (int i = 0; i < random.Next(3, 6); i++)
            {
                Room room = new Room();
                room.AddEnemy(new Enemy0(enemyTexture, new Vector2(200, 100)));
                room.AddEnemy(new Enemy0(enemyTexture, new Vector2(400, 150)));
                _rooms.Add(room);
            }

            // Conectar las habitaciones a `spawnRoom`
            for (int i = 0; i < _rooms.Count; i++)
            {
                spawnRoom.AttachRoom(_rooms[i], i % 4);
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (_currentRoomIndex >= _rooms.Count) return;

            var currentRoom = _rooms[_currentRoomIndex];
            currentRoom.Update(gameTime, player);

            if (currentRoom.IsCleared)
            {
                _currentRoomIndex++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentRoomIndex < _rooms.Count)
            {
                _rooms[_currentRoomIndex].Draw(spriteBatch);
            }
        }
    }
}