using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueGame.Rooms;

namespace RogueGame.Entities
{
    public abstract class Enemy : Entity
    {
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        protected float Speed;  // Velocidad del enemigo
        protected float DamageCooldown;  // Tiempo de espera entre ataques
        protected float DamageTimer;
        public Texture2D CoinTexture {get; set;}
        private Random _random;
        private RoomManager _roomManager;

        public Enemy(RoomManager roomManager, Texture2D texture, Vector2 startPosition, int health, int damage, float speed)
            : base(texture, startPosition)
        {
            Health = health;
            Damage = damage;
            Speed = speed;
            DamageCooldown = 1.0f;  // Ejemplo: 1 segundo de cooldown
            DamageTimer = 0;
            _random = new Random();
            _roomManager = roomManager;
        }
        override public bool IsAlive()
        {
            return Health > 0;
        }
        override public void TakeDamage(int damage)
        {
            Health = Math.Max(Health - damage, 0);
            if (!IsAlive()) OnDeath();
        }

        public override void Update(GameTime gameTime, Player player)
        {
            if (!IsAlive()) return;

            Move(gameTime,player);
            DamageTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive())
            {
                base.Draw(spriteBatch);
            }
        }

        protected virtual void OnDeath()
        {
            DropItem();
            Console.WriteLine("Enemy has been defeated!");
        }

        private void DropItem()
        {
            if (_random.NextDouble() < 1)
            {
                Vector2 DropPosition = new Vector2(Position.X+Width/2, Position.Y+Height/2);
                Coin coin = new Coin(CoinTexture, DropPosition, 1);
                _roomManager.coins.Add(coin);
            }
        }
        // Método abstracto que define el movimiento específico del enemigo
        protected abstract void Move(GameTime gameTime, Player player);

        // Método para atacar (puede ser sobrescrito en subclases)
        public virtual void Attack(Player player)
        {
            if (DamageTimer >= DamageCooldown)
            {
                player.TakeDamage(Damage);
                DamageTimer = 0;
            }
        }
    
    }
}