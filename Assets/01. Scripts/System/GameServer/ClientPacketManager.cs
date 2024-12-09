using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }


	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.SConnect, MakePacket<S_Connect>);
		_handler.Add((ushort)MsgId.SConnect, PacketHandler.S_ConnectHandler);		
		_onRecv.Add((ushort)MsgId.SDisconnect, MakePacket<S_Disconnect>);
		_handler.Add((ushort)MsgId.SDisconnect, PacketHandler.S_DisconnectHandler);		
		_onRecv.Add((ushort)MsgId.SDbdebug, MakePacket<S_Dbdebug>);
		_handler.Add((ushort)MsgId.SDbdebug, PacketHandler.S_DbdebugHandler);		
		_onRecv.Add((ushort)MsgId.SChat, MakePacket<S_Chat>);
		_handler.Add((ushort)MsgId.SChat, PacketHandler.S_ChatHandler);		
		_onRecv.Add((ushort)MsgId.SRoomcreate, MakePacket<S_Roomcreate>);
		_handler.Add((ushort)MsgId.SRoomcreate, PacketHandler.S_RoomcreateHandler);		
		_onRecv.Add((ushort)MsgId.SRoomjoin, MakePacket<S_Roomjoin>);
		_handler.Add((ushort)MsgId.SRoomjoin, PacketHandler.S_RoomjoinHandler);		
		_onRecv.Add((ushort)MsgId.SRoomleft, MakePacket<S_Roomleft>);
		_handler.Add((ushort)MsgId.SRoomleft, PacketHandler.S_RoomleftHandler);		
		_onRecv.Add((ushort)MsgId.SMatchmaked, MakePacket<S_Matchmaked>);
		_handler.Add((ushort)MsgId.SMatchmaked, PacketHandler.S_MatchmakedHandler);		
		_onRecv.Add((ushort)MsgId.SRefresh, MakePacket<S_Refresh>);
		_handler.Add((ushort)MsgId.SRefresh, PacketHandler.S_RefreshHandler);		
		_onRecv.Add((ushort)MsgId.SRoomrefresh, MakePacket<S_Roomrefresh>);
		_handler.Add((ushort)MsgId.SRoomrefresh, PacketHandler.S_RoomrefreshHandler);		
		_onRecv.Add((ushort)MsgId.SNewroomcreated, MakePacket<S_Newroomcreated>);
		_handler.Add((ushort)MsgId.SNewroomcreated, PacketHandler.S_NewroomcreatedHandler);		
		_onRecv.Add((ushort)MsgId.SRoomjoiner, MakePacket<S_Roomjoiner>);
		_handler.Add((ushort)MsgId.SRoomjoiner, PacketHandler.S_RoomjoinerHandler);		
		_onRecv.Add((ushort)MsgId.SLobbydebug, MakePacket<S_Lobbydebug>);
		_handler.Add((ushort)MsgId.SLobbydebug, PacketHandler.S_LobbydebugHandler);		
		_onRecv.Add((ushort)MsgId.SRoomgamestart, MakePacket<S_Roomgamestart>);
		_handler.Add((ushort)MsgId.SRoomgamestart, PacketHandler.S_RoomgamestartHandler);		
		_onRecv.Add((ushort)MsgId.SSpawn, MakePacket<S_Spawn>);
		_handler.Add((ushort)MsgId.SSpawn, PacketHandler.S_SpawnHandler);		
		_onRecv.Add((ushort)MsgId.SDestroy, MakePacket<S_Destroy>);
		_handler.Add((ushort)MsgId.SDestroy, PacketHandler.S_DestroyHandler);		
		_onRecv.Add((ushort)MsgId.SRoundstart, MakePacket<S_Roundstart>);
		_handler.Add((ushort)MsgId.SRoundstart, PacketHandler.S_RoundstartHandler);		
		_onRecv.Add((ushort)MsgId.SRoundend, MakePacket<S_Roundend>);
		_handler.Add((ushort)MsgId.SRoundend, PacketHandler.S_RoundendHandler);		
		_onRecv.Add((ushort)MsgId.SCardselect, MakePacket<S_Cardselect>);
		_handler.Add((ushort)MsgId.SCardselect, PacketHandler.S_CardselectHandler);		
		_onRecv.Add((ushort)MsgId.SLoadingstatus, MakePacket<S_Loadingstatus>);
		_handler.Add((ushort)MsgId.SLoadingstatus, PacketHandler.S_LoadingstatusHandler);		
		_onRecv.Add((ushort)MsgId.SMove, MakePacket<S_Move>);
		_handler.Add((ushort)MsgId.SMove, PacketHandler.S_MoveHandler);		
		_onRecv.Add((ushort)MsgId.SShoot, MakePacket<S_Shoot>);
		_handler.Add((ushort)MsgId.SShoot, PacketHandler.S_ShootHandler);		
		_onRecv.Add((ushort)MsgId.SChangeHp, MakePacket<S_ChangeHp>);
		_handler.Add((ushort)MsgId.SChangeHp, PacketHandler.S_ChangeHpHandler);		
		_onRecv.Add((ushort)MsgId.SPlayeraim, MakePacket<S_Playeraim>);
		_handler.Add((ushort)MsgId.SPlayeraim, PacketHandler.S_PlayeraimHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
            if (_handler.TryGetValue(id, out action))
                action.Invoke(session, pkt);
		}

	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}