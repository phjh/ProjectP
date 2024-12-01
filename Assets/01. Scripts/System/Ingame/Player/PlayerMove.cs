using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerInherit
{
    private Rigidbody2D _rb;

    protected override void Init()
    {
        if(!TryGetComponent(out _rb))
        {
            Debug.Log("Failed to Get rigidbody2D at PlayerMove");
        }
        _input.MovementEvent += Move;
        _input.MovementEvent += SendMovePacket;
    }

    private void Move(Vector2 dir)
    {
        _rb.velocity = dir * _status.moveSpeed;
	}

    private void SendMovePacket(Vector2 dir)
    {
		C_Move move = new C_Move();
		move.RoomId = RoomManager.Instance.GetRoomId();
		move.PlayerId = InGameScene.Instance.playerId;

        PositionInfo info = new PositionInfo();
        info.MoveDir = new Direction();
		info.MoveDir.PosX = dir.x;
		info.MoveDir.PosY = dir.y;
        move.PosInfo = info;

		NetworkManager.Instance.Send(move);
	}

}
