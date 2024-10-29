using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rooms : MonoBehaviour
{
	public TextMeshProUGUI roomname;
	public TextMeshProUGUI userLimit;
	public Image isLocked;
	public int nowUserCount = 1;
	public int roomId = 0;

	public void SetRoomUI(string name, int userlimit, int users, bool locked, int roomid)
	{
		roomname.text = name;
		userLimit.text = $"{users} / {userlimit}";
		isLocked.gameObject.SetActive(locked);
		roomId = roomid;
	}

	public void SelectRoom()
	{
		RoomManager.Instance.SelectRoom(this);
	}
}
