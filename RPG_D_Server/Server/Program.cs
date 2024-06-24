﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Data;
using Server.Game;
using Server.Game.Collider;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();
		static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

		static void TickRoom(GameRoom room, int tick = 100)
		{
			var timer = new System.Timers.Timer();
			timer.Interval = tick;
			timer.Elapsed += ((s, e) => { room.Update(); });
			timer.AutoReset = true;
			timer.Enabled = true;

			_timers.Add(timer);
		}

		static void Main(string[] args)
		{
			Console.WriteLine("RPG_C Server Started");

            ConfigManager.LoadConfig();
			DataManager.LoadData();

			GameRoom room = RoomManager.Instance.Add(1);
			TickRoom(room, 100);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);

            if (endPoint == null)
				Console.WriteLine("End point is Null");

            _listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Server Start...");

            // TODO
            while (true)
			{
				Thread.Sleep(100);
			}
		}
	}
}
