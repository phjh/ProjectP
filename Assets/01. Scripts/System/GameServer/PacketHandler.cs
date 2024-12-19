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

	internal static void S_LoadingstatusHandler(PacketSession session, IMessage message)
	{
		S_Loadingstatus status = message as S_Loadingstatus;

		Debug.Log($"is all loaded : {status.IsAllLoaded}, now loaded user count : {status.LoadedUsers.Count}");

		if (status.IsAllLoaded)
		{
			//게임스타트 코루틴 실행
			Debug.Log("all user loaded, game start");

			foreach(var player in status.LoadedUsers)
			{
				InGameScene.Instance.AddPlayer(player.Usernum, player);
			}

			InGameScene.Instance.gameFlow.GameStarting();
		}
		else
		{
			//현재 로딩된 인원 출력
			if(status.LoadedUsers.Contains(DBManager.Instance.GetUserInfo()))
			{
				InGameScene.Instance.gameFlow.SetLoadingProcess(status.LoadedUsers.Count, status.Userlimit);
			}
			else
			{
				Debug.Log(status.LoadedUsers.Count);
			}
		}

	}

	internal static void S_LobbydebugHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_MatchmakedHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_MoveHandler(PacketSession session, IMessage message)
	{
		S_Move move = message as S_Move;
		if (InGameScene.Instance == null)
			return;

		InGameScene.Instance.MovePacket(move.PlayerId, new Vector2(move.PosInfo.MoveDir.PosX, move.PosInfo.MoveDir.PosY));
	}

	internal static void S_NewroomcreatedHandler(PacketSession session, IMessage message)
	{
		if (RoomManager.Instance == null)
			return;

		S_Newroomcreated newRoom = message as S_Newroomcreated;

		RoomManager.Instance.CreateRoomUI(newRoom.Info.RoomId, newRoom.Info.RoomName, newRoom.Info.Userlimit, newRoom.Info.Userinfo.Count, newRoom.Info.UsePw);

	}

	internal static void S_PlayeraimHandler(PacketSession session, IMessage message)
	{
		S_Playeraim aim = message as S_Playeraim;
		if (InGameScene.Instance == null)
			return;

		InGameScene.Instance.AimPacket(aim.PlayerId, new Vector3(aim.Dir.PosX, aim.Dir.PosY, aim.Dir.PosZ));
	}

	internal static void S_RefreshHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoomcreateHandler(PacketSession session, IMessage message)
	{
		S_Roomcreate create = message as S_Roomcreate;

		StandRoomScene.Instance.SetRoomUI(create.Roomname, create.Roomlimit, create.Usepw);
	}

	internal static async void S_RoomgamestartHandler(PacketSession session, IMessage message)
	{
		DBManager.Instance.SetUserInfo((message as S_Roomgamestart).UserInfo);
		SceneManager.Instance.LoadScene(SceneManager.SceneList.InGame);
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
		if (RoomManager.Instance == null)
			return;

		RoomManager.Instance.ResetRoomUI();

		S_Roomrefresh refresh = message as S_Roomrefresh;

		foreach(var i in refresh.Info)
		{
			RoomManager.Instance.CreateRoomUI(i.RoomId, i.RoomName, i.Userlimit, i.Userinfo.Count, i.UsePw);
		}

	}

	internal static void S_RoundendHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_RoundstartHandler(PacketSession session, IMessage message)
	{

	}

	internal static void S_ShootHandler(PacketSession session, IMessage message)
	{
		S_Shoot shoot = message as S_Shoot;
		if (InGameScene.Instance == null)
			return;

		InGameScene.Instance.ShootPacket(shoot.ObjectId);
	}

	internal static void S_SpawnHandler(PacketSession session, IMessage message)
	{

	}
}
