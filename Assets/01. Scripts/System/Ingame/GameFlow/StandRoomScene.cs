using Google.Protobuf;
using Google.Protobuf.Protocol;
using Microsoft.Win32;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StandRoomScene : MonoSingleton<StandRoomScene>
{
	[SerializeField]
	private TMP_InputField _chatInputField;
	[SerializeField]
	private TMP_Dropdown _chatRegion;

	[SerializeField]
	private TextMeshProUGUI _name;
	[SerializeField]
	private TextMeshProUGUI _score;

	#region roominfo

	[SerializeField]
	private RectTransform _gameRoom;
	[SerializeField]
	private TextMeshProUGUI _gameRoomName;
	[SerializeField]
	private RectTransform _isgameLocked;

	[SerializeField]
	private List<PlayerUIs> _players;

	[SerializeField]
	private RectTransform _roominfo;

	[SerializeField]
	private TMP_InputField roomname;
	[SerializeField]
	private TMP_Dropdown userlimit;
	[SerializeField]
	private Toggle usePw;

	[SerializeField]
	private TMP_InputField password;
	[SerializeField]
	private RectTransform _passwordUI;


	[SerializeField]
	private RectTransform enterPasswordUI;
	[SerializeField]
	private TMP_InputField enterPassword;

	private C_Roomjoin tempJoin;

	#endregion

	private void Start()
	{
		_chatInputField.onEndEdit.AddListener(INPUTFIELD_SendChat);
		//SetData();
	}

	private void SetData()
	{
		_name.text = DBManager.Instance.userName;
		_score.text = " Rating : " + DBManager.Instance.userScore.ToString();
	}

	public void INPUTFIELD_SendChat(string input)
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			TrySendChat(input);
	}

	private void TrySendChat(string input)
	{
		if (input.Length == 0)
			return;

		C_Chat chat = new C_Chat()
		{
			Chat = input,
			Name = DBManager.Instance.userName,
			Type = ChatType.Lobbychat,
		};
		NetworkManager.Instance.Send(chat);

		Debug.Log($"Àü¼ÛµÊ: {input}");

		_chatInputField.text = "";
	}

	public void SetRoomUI(string roomname, int roomlimit, bool usePw)
	{
		_gameRoomName.text = roomname;
		_isgameLocked.gameObject.SetActive(usePw);
		for(int i = 0; i < 10; i++)
			_players[i].gameObject.SetActive(i < roomlimit);
	}

	public void CreateRoomUI(bool value) => _roominfo.gameObject.SetActive(value);
	public void SetGameRoomUI(bool value) => _gameRoom.gameObject.SetActive(value);
	public void SetEnterPasswordUI(bool value) => enterPasswordUI.gameObject.SetActive(value);
	public void usePassword() => _passwordUI.gameObject.SetActive(usePw.isOn);

	public void CreateRoom()
	{
		CreateRoomUI(false);
		SetGameRoomUI(true);
		C_Roomcreate create = new C_Roomcreate();
		UserInfo userinfo = new UserInfo();
		create.Roomname = roomname.text;
		create.Roomlimit = userlimit.value + 2;
		create.Usepw = usePw.isOn;
		create.Pw = password.text;
		userinfo.UserName = DBManager.Instance.userName;
		userinfo.UserScore = DBManager.Instance.userScore;
		create.UserInfo = userinfo;

		Debug.Log("send Create room packet");
		Debug.Log($"name : {create.Roomname}, limit:{create.Roomlimit}, usepw : {create.Usepw}");
		NetworkManager.Instance.Send(create);
	}

	public void LeftRoom()
	{
		_gameRoom.gameObject.SetActive(false);

		//Å»Ãâ ÆÐÅ¶ Àü¼Û
	}

	public void StartGame()
	{

	}

	public void JoinRoomButton()
	{
		C_Roomjoin join = new C_Roomjoin();
		join.Userinfo = new UserInfo();
		Rooms room = RoomManager.Instance.GetSelectedRoom();

		join.Userinfo.UserName = DBManager.Instance.userName;
		join.Userinfo.UserScore = DBManager.Instance.userScore;
		join.Roomid = room.roomId;
		if (room.isLocked)
		{
			tempJoin = join;
			SetEnterPasswordUI(true);
			return;
		}

		NetworkManager.Instance.Send(join);
	}

	public void JoinRoomPasswordEnter()
	{
		SetEnterPasswordUI(false);
		C_Roomjoin join = tempJoin;
		join.Pw = enterPassword.text;
		NetworkManager.Instance.Send(join);
		tempJoin = null;
	}

	public void JoinRoomOthers(S_Roomjoin join)
	{
		SetGameRoomUI(true);

		Rooms room = RoomManager.Instance.GetSelectedRoom();
		_gameRoomName.text = room.roomname.text;
		_isgameLocked.gameObject.SetActive(room.isLocked);

		_players[room.nowUserCount].SetUI(join.Userinfo.UserName, join.Userinfo.UserScore);
		room.nowUserCount++;

	}

	public void JoinRoomInfos(S_Roomjoiner join)
	{
		int usercount = join.Users.Count;
		for (int i = 0; i < usercount; i++) 
		{
			_players[i].SetUI(join.Users[i].UserName, join.Users[i].UserScore);
		}
	}


}
