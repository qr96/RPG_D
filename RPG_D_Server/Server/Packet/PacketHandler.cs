using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Data;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class PacketHandler
{
    public static void C_LoginGameHandler(PacketSession session, IMessage message)
    {
        var packet = message as C_LoginGame;
        var clientSession = session as ClientSession;

        var player = clientSession.MyPlayer;
        if (player == null)
            return;


        var playerName = packet.Name;

        var sendPacket = new S_LoginGame();
        sendPacket.Name = playerName;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.Push(room.HandleMove, player, movePacket);
        
        clientSession.Send(sendPacket);
    }
}
