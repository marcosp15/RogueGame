using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueGame.Entities
{
    public class Coin : Item
    {
        public int Value {get; set;}
        public Coin(Texture2D texture, Vector2 position, int value) : base(texture,position)
        {
            Value = value;
        }

        public override void ApllyEffect(Player player)
        {
            player.Money += Value;
        }

        public override bool IsAlive()
        {
            return !IsCollected;
        }

        public override void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }
    }
}