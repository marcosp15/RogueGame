using System;
using System.Collections.Generic;
using JuegoHorda.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueGame.Core;
using RogueGame.Entities;

namespace RogueGame.Entities
{
    public class Player : Entity
    {
        public float Speed { get; set; } = 500f;
        public int Health { get; set; } = 3;
        public bool IsAlive => Health > 0;
        public int Damage {get; set;} = 1;
        public float DamageCooldown {get; set;} = 0.2f;
        private List<Proyectil> proyectiles;
        public Texture2D ProyectilTexture {get; set;}
        public float ProyectilSpeed {get; set;} = 10f;
        private float _shootCooldownTimer;

        public Player(Texture2D texture, Vector2 startPosition) : base(texture, startPosition)
        {
            proyectiles = new List<Proyectil>();
        }

        public override void Update(GameTime gameTime, Player player)
        {
            HandleInput(gameTime);
        }

        public void UpdateProyectiles(GameTime gameTime, List<Enemy> enemigos)
        {
            List<Proyectil> proyToRemove = new List<Proyectil>();
            foreach (var proy in proyectiles)
            {
                if (proy.Update(gameTime,enemigos) == 1) proyToRemove.Add(proy);
            }
            foreach (var proy in proyToRemove)
            {
                proyectiles.Remove(proy);
            }
            _shootCooldownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void HandleInput(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W)) movement.Y = -1;
            if (keyboardState.IsKeyDown(Keys.S)) movement.Y = 1;
            if (keyboardState.IsKeyDown(Keys.A)) movement.X = -1;
            if (keyboardState.IsKeyDown(Keys.D)) movement.X = 1;

            if (movement != Vector2.Zero)
                movement.Normalize();  // Para evitar moverse más rápido en diagonal
            Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Que no se salga de pantalla
            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, Data.ScreenW - Texture.Width),
                MathHelper.Clamp(Position.Y, 0, Data.ScreenH - Texture.Height)
            );

            //Proyectiles
            if (_shootCooldownTimer >= DamageCooldown)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Left)) Shoot (new Vector2(-1,-1));
                else if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyDown(Keys.Right)) Shoot (new Vector2(1,-1));
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Left)) Shoot (new Vector2(-1,1));
                else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyDown(Keys.Right)) Shoot (new Vector2(1,1));
                else if (keyboardState.IsKeyDown(Keys.Up)) Shoot(new Vector2(0,-1));
                else if (keyboardState.IsKeyDown(Keys.Down)) Shoot(new Vector2(0,1));
                else if (keyboardState.IsKeyDown(Keys.Left)) Shoot(new Vector2(-1,0));
                else if (keyboardState.IsKeyDown(Keys.Right)) Shoot(new Vector2(1,0));
                _shootCooldownTimer = 0f;
            }

        }

        private void Shoot(Vector2 direction)
        {
            Proyectil proyectil = new Proyectil(ProyectilTexture,Position,direction,ProyectilSpeed, Damage);
            proyectiles.Add(proyectil);
        }

        internal void TakeDamage(int damage)
        {
            Health = Math.Max(Health - damage, 0);
        }

         public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawStats(spriteBatch);
            DrawProyectiles(spriteBatch);
        }

        private void DrawProyectiles(SpriteBatch spriteBatch)
        {
            foreach (var proy in proyectiles)
            {
                spriteBatch.Draw(proy.Texture, proy.Position, Color.White);
            }
        }

        private void DrawStats(SpriteBatch spriteBatch)
        {
            // Definir las posiciones para mostrar las estadísticas a la derecha de la pantalla
            float x = Data.ScreenW - 100; // Posición X a la derecha
            float y = 10; // Comienza a mostrar desde la parte superior
            float inc = 20;

            // Dibujar las estadísticas
            spriteBatch.DrawString(Game1.font, $"HP: {Health}", new Vector2(x, y), Color.White);
            y += inc;  // Separar las estadísticas

            spriteBatch.DrawString(Game1.font, $"Spd: {Speed}", new Vector2(x, y), Color.White);
            y += inc;

            spriteBatch.DrawString(Game1.font, $"Dmg: {Damage}", new Vector2(x, y), Color.White);
            y += inc;

            // Velocidad de disparo
            spriteBatch.DrawString(Game1.font, $"Cad: {DamageCooldown}", new Vector2(x, y), Color.White);
            y += inc;

            spriteBatch.DrawString(Game1.font, $"DPS: {Math.Truncate(100 * (1 / DamageCooldown))/100}", new Vector2(x,y), Color.White);
        }

        public void SetPositionToRoomEntrance(int pos)
        {
            if (pos == 0)
                Position = new Vector2(Data.ScreenW / 2, Data.ScreenH - 32);
            else if (pos == 1)
                Position = new Vector2(32, Data.ScreenH / 2);
            else if (pos == 2)
                Position = new Vector2(Data.ScreenW / 2, 32);
            else if (pos == 3)
                Position = new Vector2(Data.ScreenW - 32, Data.ScreenH / 2);
        }
    }
}
