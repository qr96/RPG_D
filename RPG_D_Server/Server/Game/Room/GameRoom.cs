﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Server.Game
{
	public class GameRoom : JobSerializer
	{
		public int RoomId { get; set; }

		Dictionary<int, Player> _players = new Dictionary<int, Player>();

		public void Init(int mapId)
		{
			// TODO
        }

		// 누군가 주기적으로 호출해줘야 한다
		public void Update()
		{
			foreach(var player in _players.Values)
                player.Update();

			Flush();
		}

		public void EnterGame(GameObject gameObject)
		{
			if (gameObject == null)
				return;

			GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

			if (type == GameObjectType.Player)
			{
				Player player = gameObject as Player;
				_players.Add(gameObject.Id, player);
				player.Room = this;

				// 본인한테 정보 전송
				{
					//S_EnterGame enterPacket = new S_EnterGame();
					//enterPacket.Player = player.Info;
					//player.Session.Send(enterPacket);

					//S_Spawn spawnPacket = new S_Spawn();
					//foreach (Player p in _players.Values)
					//{
					//	if (player != p)
					//		spawnPacket.Objects.Add(p.Info);
					//}

					//foreach (var m in _monsters.Values)
					//{
     //                   if (m.IsAlive())
     //                       spawnPacket.Objects.Add(m.Info);
     //               }

     //               player.Session.Send(spawnPacket);
					//player.Spawn();
					//player.SendStatInfo();
				}
			}

			// 타인한테 정보 전송
			{
                //S_Spawn spawnPacket = new S_Spawn();
                //spawnPacket.Objects.Add(gameObject.Info);
                //foreach (Player p in _players.Values)
                //{
                //    if (p.Id != gameObject.Id)
                //        p.Session.Send(spawnPacket);
                //}
            }
        }

		public void LeaveGame(int objectId)
		{
			GameObjectType type = ObjectManager.GetObjectTypeById(objectId);

			if (type == GameObjectType.Player)
			{
				Player player = null;
				if (_players.Remove(objectId, out player) == false)
					return;

				player.Room = null;

				// 본인한테 정보 전송
				{
					//S_LeaveGame leavePacket = new S_LeaveGame();
					//player.Session.Send(leavePacket);
				}
			}
		}

		public Player FindPlayer(Func<Player, bool> condition)
		{
			foreach (Player player in _players.Values)
			{
				if (condition.Invoke(player))
					return player;
			}

			return null;
		}

		public void Broadcast(IMessage packet)
		{
			foreach (Player p in _players.Values)
			{
				p.Session.Send(packet);
			}
		}
	}
}
