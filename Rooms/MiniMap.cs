using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RogueGame.Rooms
{
    public class MiniMap
    {
        private List<Room> _rooms; // Habitaciones descubiertas
        private Texture2D _roomTexture;
        private Vector2 _mapPosition;
        private int _gridSize = 11;
        private int _roomSize = 12; // Tamaño de cada habitación en el minimapa

        public MiniMap(Texture2D roomTexture, Vector2 mapPosition, List<Room> rooms)
        {
            _rooms = rooms;
            _roomTexture = roomTexture;
            _mapPosition = mapPosition;
        }

        public void Draw(SpriteBatch spriteBatch, Room currentRoom)
        {
            foreach (var room in _rooms)
            {
                if (room.isDiscovered)
                {
                    Vector2 position = new Vector2(
                        room.MapPosition.X * _roomSize, 
                        -room.MapPosition.Y * _roomSize // Invertimos el eje Y para que coincida con la orientación del juego
                    ) + _mapPosition;

                    // Habitación actual será blanca, las demás grises
                    Color color = (room == currentRoom) ? Color.White : Color.LightGray;

                    spriteBatch.Draw(_roomTexture, position, color);
                }
            }
        }
    }
}