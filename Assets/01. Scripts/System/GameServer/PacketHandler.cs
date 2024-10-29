using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using Debug = UnityEngine.Debug;

public class PacketHandler 
{
	public static void S_ConnectHandler(PacketSession session, IMessage packet)
	{
		S_Connect enter = packet as S_Connect;
		Debug.Log("enter Packet");
		SceneManager.Instance.LoadNextScene();
	}

	internal static void S_CardselectHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_ChangeHpHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_ChatHandler(PacketSession session, IMessage message)
	{
		S_Chat chat = message as S_Chat;
		ChattingManager.Instance.SetChatting($"{chat.Name} : {chat.Chat}");
	}

	internal static void S_DbdebugHandler(PacketSession session, IMessage message)
	{
		S_Dbdebug debug = message as S_Dbdebug;
		if (debug.Success)
		{
			Debug.Log($"name: {debug.Name}, score: {debug.Score}");
			DBManager.Instance.userName = debug.Name;
			DBManager.Instance.userScore = debug.Score;
			DBManager.Instance.Success?.Invoke();
		}
		else
		{
			Debug.Log($"name: {debug.Name}, score: {debug.Score}");
			DBManager.Instance.DBError(debug.Name, debug.Score);
		}
	}

	internal static void S_DestroyHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_DisconnectHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_LeaveHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_LobbydebugHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_MatchmakedHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_MoveHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_NewroomcreatedHandler(PacketSession session, IMessage message)
	{
		S_Newroomcreated newRoom = message as S_Newroomcreated;

		RoomManager.Instance.CreateRoom(newRoom.Info.RoomId, newRoom.Info.RoomName, newRoom.Info.Userlimit, newRoom.Info.UserNames.Count, newRoom.Info.UsePw);

	}

	internal static void S_RefreshHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoomcreateHandler(PacketSession session, IMessage message)
	{
		S_Roomcreate create = message as S_Roomcreate;

		StandRoomScene.Instance.SetRoomUI(create.Roomname, create.Roomlimit, create.Usepw);
	}

	internal static void S_RoomjoinerHandler(PacketSession session, IMessage message)
	{
		//여기서 플레이어 리스트 받아와준다
		StandRoomScene.Instance.JoinRoomInfos(message as S_Roomjoiner);
	}

	internal static void S_RoomjoinHandler(PacketSession session, IMessage message)
	{
		StandRoomScene.Instance.JoinRoomOthers(message as S_Roomjoin);
	}

	internal static void S_RoomleftHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoomrefreshHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoundendHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoundstartHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_ShootHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_SpawnHandler(PacketSession session, IMessage message)
	{

	}
}
