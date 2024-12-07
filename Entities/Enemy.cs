using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueGame.Entities
{
    public abstract class Enemy : Entity
    {
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsAlive => Health > 0;

        protected float Speed;  // Velocidad del enemigo
        protected float DamageCooldown;  // Tiempo de espera entre ataques
        protected float DamageTimer;

        public Enemy(Texture2D texture, Vector2 startPosition, int health, int damage, float speed)
            : base(texture, startPosition)
        {
            Health = health;
            Damage = damage;
            Speed = speed;
            DamageCooldown = 1.0f;  // Ejemplo: 1 segundo de cooldown
            DamageTimer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive) return;

            Move(gameTime);
            DamageTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                base.Draw(spriteBatch);
            }
        }

        // Método para recibir daño
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
                OnDeath();
        }

        protected virtual void OnDeath()
        {
            // Lógica al morir (drop de ítems, efectos, etc.)
            Console.WriteLine("Enemy has been defeated!");
        }

        // Método abstracto que define el movimiento específico del enemigo
        protected abstract void Move(GameTime gameTime);

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