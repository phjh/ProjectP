using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoSingleton<InGameScene>
{
	public Dictionary<int, UserInfo> playerData = new Dictionary<int, UserInfo>();
	public Dictionary<UserInfo, OtherPlayers> otherPlayers = new Dictionary<UserInfo, OtherPlayers>();
	public int playerId = 0;

	[SerializeField]
	private List<Transform> SpawnPoints = new();
	[SerializeField]
	private GameObject _player;
	[SerializeField]
	private GameObject _otherPlayer;

	public InGameFlow gameFlow;

	private void Awake()
	{
		C_Loadingend end = new C_Loadingend();
		end.Roomid = RoomManager.Instance.GetRoomId();
		end.UserInfo = DBManager.Instance.GetUserInfo();
		NetworkManager.Instance.Send(end);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			Debug.Log($"now players : {playerData.Count}, other player count : {otherPlayers.Count}");
		}
	}

	public void StartGame()
	{

	}

	public void AddPlayer(int num, UserInfo info)
	{
		Debug.Log(info);
		if(num == DBManager.Instance.usernum)
		{
			Instantiate(_player, SpawnPoints[DBManager.Instance.usernum - 1].position, Quaternion.identity);
			playerData.Add(num, info);
			playerId = num;

			DBManager.Instance.team = info.NowTeam;
			GameManager.Instance.player.team = info.NowTeam;
		}
		else
		{
			GameObject newPlayerObj = Instantiate(_otherPlayer, SpawnPoints[num - 1].position, Quaternion.identity);
			OtherPlayers newPlayer = newPlayerObj.GetComponent<OtherPlayers>();
			playerData.Add(num, info);
			otherPlayers.Add(info, newPlayer);
			newPlayer.team = info.NowTeam;
		}
	}

	public void MovePacket(int usernum, Vector2 dir)
	{
		if (usernum == DBManager.Instance.usernum)
			return;

		otherPlayers[playerData[usernum]].Move(dir);
	}

	public void ShootPacket(int usernum)
	{
		if (usernum == DBManager.Instance.usernum)
			return;

		otherPlayers[playerData[usernum]].GetPlayer().GetOtherPlayerAttack().Attack();
	}

	public void AimPacket(int usernum, Vector3 dir)
	{
		if (usernum == DBManager.Instance.usernum)
			return;

		if (playerData.TryGetValue(usernum, out UserInfo info))
		{
			Debug.Log("aim num : " + usernum);

			otherPlayers[info].GetPlayer().GetOtherPlayerAttack().SetPlayerAttackDir(dir);
		}
	}

}
