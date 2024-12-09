using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JuegoHorda.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Core;
using RogueGame.Rooms;

namespace RogueGame.Entities
{
    public class Enemy1 : Enemy
    {
        private List<Proyectil> proyectiles;
        public Texture2D ProyectilTexture {get; set;}
        public float ProyectilSpeed {get; set;} = 8f;
        private float _shootCooldownTimer;
        private Random _random;
        private float _cambiaDirTimer;
        private float _actualintervalo;
        private Vector2 _direction;

        public Enemy1(RoomManager roomManager, Texture2D texture, Vector2 startPosition) 
            : base(roomManager, texture, startPosition, health: 6, damage: 1, speed: 50f)
        {
            proyectiles = new List<Proyectil>();
            _random = new Random();
            SetRandomDirection();
            SetRandomInterval();
            DamageCooldown = .7f;
            }

        protected override void Move(GameTime gameTime, Player player)
        {
            if (!IsAlive()) return;

            Position += _direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Verificar colisión con el jugador
            if (GetBounds().Intersects(player.GetBounds()))
            {
                Attack(player);
            }
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, Data.ScreenW - Texture.Width),
                MathHelper.Clamp(Position.Y, 0, Data.ScreenH - Texture.Height)
            );
        }
        public override void Update(GameTime gameTime, Player player)
        {
            Move(gameTime, player);
            _cambiaDirTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            _shootCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cambiaDirTimer <= 0f)
            {
                SetRandomDirection();
                SetRandomInterval();
            }

            CheckBounds();

            UpdateProyectiles(gameTime, player);
                
            base.Update(gameTime, player);
        }

        public void UpdateProyectiles(GameTime gameTime, Player player)
        {
            if (_shootCooldownTimer <= 0f)
            {
                Shoot(new Vector2(1,0));
                Shoot(new Vector2(0,1));
                Shoot(new Vector2(-1,0));
                Shoot(new Vector2(0,-1));
                _shootCooldownTimer = DamageCooldown;
            }

            List<Entity> jugador = new List<Entity>();
            jugador.Add(player);
            List<Proyectil> proyToRemove = new List<Proyectil>();
            foreach (var proy in proyectiles)
            {
                if (proy.Update(gameTime, jugador) == 1)
                    proyToRemove.Add(proy);
            }
            foreach (var proy in proyToRemove)
            {
                proyectiles.Remove(proy);
            }
        }
        private void Shoot(Vector2 direction)
        {
            direction.Normalize();
            Vector2 proyectilPosition = new Vector2(Position.X + Texture.Width / 2, Position.Y + Texture.Height / 2);
            proyectiles.Add(new Proyectil(ProyectilTexture, proyectilPosition, direction, ProyectilSpeed, Damage));
            Console.WriteLine($"Proyectil disparado en direccion: {direction}");
        }

        // Sobrescribir el método de muerte para efectos específicos
        protected override void OnDeath()
        {
            base.OnDeath();
        }

         private void SetRandomDirection()
        {
            // Direcciones posibles: arriba, abajo, izquierda, derecha
            Vector2[] directions = new Vector2[]
            {
                new Vector2(0, -1), // Arriba
                new Vector2(0, 1),  // Abajo
                new Vector2(-1, 0), // Izquierda
                new Vector2(1, 0)   // Derecha
            };

            // Seleccionar una dirección aleatoria
            _direction = directions[_random.Next(directions.Length)];
        }

        private void SetRandomInterval()
        {
            _actualintervalo = (float)_random.NextDouble() * 2f + 1f; // Entre 1 y 3 segundos
            _cambiaDirTimer = _actualintervalo;
        }
        private void CheckBounds()
        {
            // Rebote horizontal
            if (Position.X <= 0 || Position.X >= Data.ScreenW - Texture.Width)
            {
                _direction.X *= -1;  // Invierte la dirección horizontal
                float x = MathHelper.Clamp(Position.X, 0, Data.ScreenW - Texture.Width);
                Position = new Vector2(x,Position.Y);
            }

            // Rebote vertical
            if (Position.Y <= 0 || Position.Y >= Data.ScreenH - Texture.Height)
            {
                _direction.Y *= -1;  // Invierte la dirección vertical
                float y = MathHelper.Clamp(Position.Y, 0, Data.ScreenH - Texture.Height);
                Position = new Vector2(Position.X,y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Dibujar cada proyectil en la lista
            foreach (var proyectil in proyectiles)
            {
                spriteBatch.Draw(proyectil.Texture, proyectil.Position, Color.White);
            }
        }
    }
    
}