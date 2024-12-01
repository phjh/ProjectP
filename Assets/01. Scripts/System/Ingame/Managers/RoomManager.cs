using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoSingleton<RoomManager>
{
	[SerializeField]
	private RectTransform RoomScroll;
	public Dictionary<int, Rooms> _roomLists = new Dictionary<int, Rooms>();
	private Rooms selectedRoom;
	

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void CreateRoomUI(int roomid, string name, int userlimit, int users, bool usePw)
	{
		PoolableMono mono = PoolManager.Instance.Pop(PoolUIListEnum.Rooms);
		mono.transform.SetParent(RoomScroll);
		Rooms room = mono.GetComponent<Rooms>();
		room.SetRoomUI(name, userlimit, users, usePw, roomid);
		_roomLists.Add(roomid, room);
		selectedRoom = room;
	}

	public void JoinRoom()
	{

	}

	public void RemoveRoomUI(int roomid)
	{
		PoolableMono mono = _roomLists[roomid].gameObject.GetComponent<PoolableMono>();
		PoolManager.Instance.Push(mono, PoolUIListEnum.Rooms);
		_roomLists.Remove(roomid);
	}

	public void SelectRoom(Rooms room) => selectedRoom = room;

	public int GetRoomId() => selectedRoom.roomId;

	public Rooms GetSelectedRoom() => selectedRoom;

	public void ResetRoomUI()
	{
		foreach(var room in _roomLists)
		{
			PoolableMono mono = room.Value.gameObject.GetComponent<PoolableMono>();
			PoolManager.Instance.Push(mono, PoolUIListEnum.Rooms);
		}
		Debug.Log("Roomlist was cleared");
		_roomLists.Clear();
	}
}
