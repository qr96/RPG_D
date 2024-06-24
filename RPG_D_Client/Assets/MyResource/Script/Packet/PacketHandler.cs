using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class PacketHandler
{
	public static void S_LoginGameHandler(PacketSession session, IMessage message)
	{
		S_LoginGame packet = message as S_LoginGame;

	}
}
