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

	public void CreateRoom(int roomid, string name, int userlimit, int users, bool usePw)
	{
		PoolableMono mono = PoolManager.Instance.Pop(PoolUIListEnum.Rooms);
		mono.transform.SetParent(RoomScroll);
		Rooms room = mono.GetComponent<Rooms>();
		room.SetRoomUI(name, userlimit, users, usePw, roomid);
		_roomLists.Add(roomid, room);
	}

	public void JoinRoom()
	{

	}

	public void RemoveRoom(int roomid)
	{
		PoolableMono mono = _roomLists[roomid].gameObject.GetComponent<PoolableMono>();
		PoolManager.Instance.Push(mono, PoolUIListEnum.Rooms);
		_roomLists.Remove(roomid);
	}

	public void SelectRoom(Rooms room) => selectedRoom = room;

	public Rooms GetSelectedRoom() => selectedRoom;
}
