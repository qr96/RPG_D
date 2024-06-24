using Google.Protobuf.Protocol;
using Server.Data;
using Server.Game.Collider;
using System;
using System.Collections.Generic;

namespace Server.Game
{
	public class Player : GameObject
	{
		public ClientSession Session { get; set; }

        public Player()
		{
			ObjectType = GameObjectType.Player;
		}

        public override void OnDamaged(GameObject attacker, long damage)
		{

		}

		public override void OnDead(GameObject attacker)
		{
			base.OnDead(attacker);
		}
    }
}
