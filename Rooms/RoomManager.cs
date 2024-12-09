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
        private MiniMap _miniMap;

        public List<Item> coins {get; set;}
        public RoomManager(ContentManager content, MiniMap miniMap)
        {
            _rooms = new List<Room>();
            random = new Random();
            Content = content;
            _miniMap = miniMap;
            coins = new List<Item>();
        }

        public void LoadContent(ContentManager content)
        {
            var enemy0Texture = content.Load<Texture2D>("enemy0");
            var enemy1Texture = content.Load<Texture2D>("Enemy1");
            var boss0Texture = content.Load<Texture2D>("Boss0");
            var proyectilEnemy1Texture = content.Load<Texture2D>("proyEnemy1");
            var coinTexture = content.Load<Texture2D>("coin1");

            Queue<Room> habitaciones_disponibles = new Queue<Room>();

            Room r = new Room(0);
            r.LoadContent(content);
            _rooms.Add(r);
            habitaciones_disponibles.Enqueue(r);
            _currentRoom = _rooms[0]; // Comenzar en la primera habitación
            _currentRoom.isDiscovered = true;
            for (int i = 1; i < 7; i++)
            {
                r = new Room(i);
                r.LoadContent(content);
                for (int j = 0; j < 1 + random.Next() % 5; j++)
                {
                    if (random.NextDouble() < 0.5)
                    {
                        Enemy1 enemy1 = new Enemy1(this, enemy1Texture, new Vector2(400 + random.Next() % 400, 200 + random.Next() % 400));
                        enemy1.ProyectilTexture = proyectilEnemy1Texture;
                        enemy1.CoinTexture = coinTexture;
                        r.AddEnemy(enemy1);
                    }
                    r.AddEnemy(new Enemy0(this, enemy0Texture, new Vector2(200 + random.Next() % 800, 200 + random.Next() % 400)));
                }
                _rooms.Add(r);
                habitaciones_disponibles.Enqueue(r);
            }
            while (habitaciones_disponibles.Count() > 0)
            {
                Room room = habitaciones_disponibles.Dequeue();
                if (habitaciones_disponibles.Count == 0)
                {
                    room.AddEnemy(new Boss0(this, boss0Texture, new Vector2(0,0)));
                }
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

            Console.WriteLine($"{_rooms.Count} habitaciones creadas en LoadContent.");
            Console.WriteLine("La habitación actual es: " + (_currentRoom != null ? "No es null" : "Es null"));
        }

        public void Update(GameTime gameTime, Player player)
        {
            _currentRoom.Update(gameTime, player);

            foreach (var coin in coins)
                coin.Update(gameTime, player);

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
                            _currentRoom.isDiscovered = true;
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
            foreach (var coin in coins)
            {
                coin.Draw(spriteBatch);
            }
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
        public List<Room> GetRooms()
        {
            return _rooms;
        }

        public Room GetCurrentRoom()
        {
            return _currentRoom;
        }
    }
}