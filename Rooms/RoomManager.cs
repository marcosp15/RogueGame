using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Entities;

namespace RogueGame.Rooms
{
    public class RoomManager
    {
        private List<Room> _rooms;
        private Room _currentRoom;
        private ContentManager Content;
        private Random random;

        public RoomManager(ContentManager content)
        {
            _rooms = new List<Room>();
            random = new Random();
            Content = content;
        }

        public void LoadContent(ContentManager content)
        {
            var enemyTexture = content.Load<Texture2D>("enemy0");
            Queue<Room> habitaciones_disponibles = new Queue<Room>();

            // Crear habitaciones y conectarlas
            /*int numero_habitacion = 1;
            Room room1 = new Room(numero_habitacion++);
            Room room2 = new Room(numero_habitacion++);
            Room room3 = new Room(numero_habitacion++);

            room1.AttachRoom(room2, 1); // Conectar room1 a room2 hacia la derecha (Este)
            room2.AttachRoom(room3, 2); // Conectar room2 a room3 hacia abajo (Sur)

            room1.LoadContent(content);
            room2.LoadContent(content);
            room3.LoadContent(content);

            room1.AddEnemy(new Enemy0(enemyTexture, new Vector2(200, 100)));
            room2.AddEnemy(new Enemy0(enemyTexture, new Vector2(300, 200)));
            room2.AddEnemy(new Enemy0(enemyTexture, new Vector2(300, 600)));
            room2.AddEnemy(new Enemy0(enemyTexture, new Vector2(800, 300)));
            room3.AddEnemy(new Enemy0(enemyTexture, new Vector2(400, 300)));

            _rooms.Add(room1);
            _rooms.Add(room2);
            _rooms.Add(room3);*/
            Room r;
            for (int i = 0; i < 11; i++)
            {
                r = new Room(i);
                r.LoadContent(content);
                for (int j = 0; j < 1 + random.Next() % 5; j++)
                {
                    r.AddEnemy(new Enemy0(enemyTexture, new Vector2(200 + random.Next() % 800, 200 + random.Next() % 400)));
                }
                _rooms.Add(r);
                habitaciones_disponibles.Enqueue(r);
            }
            while (habitaciones_disponibles.Count() > 0)
            {
                Room room = habitaciones_disponibles.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    if (random.NextDouble() < 0.5) // 50% de chances de conectar una habitacion en la puerta i
                    {
                        Room otraRoom = _rooms[random.Next(_rooms.Count())];
                        if (room.GetConnectedRoom(i) == null && otraRoom.GetConnectedRoom((i+2)%4) == null)
                        {
                            // Que no sean la misma habitacion
                            if (!room.nombre.Equals(otraRoom.nombre))
                            room.AttachRoom(otraRoom,i);
                        }
                    }
                }
            }

            _currentRoom = _rooms[0]; // Comenzar en la primera habitación
            Console.WriteLine($"{_rooms.Count} habitaciones creadas en LoadContent.");
            Console.WriteLine("La habitación actual es: " + (_currentRoom != null ? "No es null" : "Es null"));
        }

        public void Update(GameTime gameTime, Player player)
        {
             if (_currentRoom == null)
            {
                Console.WriteLine("Error: _currentRoom es null al entrar a Update");
                return;
            }

            _currentRoom.Update(gameTime, player);

            // Chequear si el jugador se acerca a una pared
            if (_currentRoom.IsCleared)
            {
                int nextRoom = CheckForRoomTransition(player);
                if (nextRoom != -1)
                {
                    if (_currentRoom == null)
                    {
                        Console.WriteLine("Error: _currentRoom es null al intentar hacer la transición.");
                    }
                    else
                    {
                        Room next = _currentRoom.GetConnectedRoom(nextRoom);
                        if (next == null)
                        {
                            Console.WriteLine($"Error: No se pudo encontrar la habitación conectada con el índice {nextRoom}.");
                        }
                        else
                        {
                            _currentRoom = next;
                            player.SetPositionToRoomEntrance(nextRoom);
                        }
                    }
                }
            }
        }

        private int CheckForRoomTransition(Player player)
        {
            float x = player.Position.X;
            float y = player.Position.Y;
            float inc = 32;
            float ejeY = Data.ScreenH/2;
            float ejeX = Data.ScreenW/2;
            if (x <= inc/2 && y >= ejeY-inc && y <= ejeY+inc) // Lado izquierdo
                return 3;
            if (x >= Data.ScreenW-inc && y >= ejeY-inc && y <= ejeY+inc) // Lado derecho
                return 1;
            if (y <= inc/2 && x <= ejeX+inc && x >= ejeX-inc) // Arriba
                return 0;
            if (y >= Data.ScreenH-inc && x <= ejeX+inc && x >= ejeX-inc) // Abajo
                return 2;

            return -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentRoom.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, $"HABITACION: {_currentRoom.nombre}", new Vector2(100, 20), Color.Green);
        }

        public void ResetRoom(Player player)
        {
             _rooms.Clear();
            LoadContent(Content);

            if (_currentRoom == null)
                Console.WriteLine("Error: _currentRoom es null después de ResetRoom.");

            if (_currentRoom != null)
                player.Position = new Vector2(Data.ScreenW / 2, Data.ScreenH / 2);
        }
    }
}