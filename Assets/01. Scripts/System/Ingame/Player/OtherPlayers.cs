using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayers : PlayerInherit
{
	private Rigidbody2D _rb;

	public Team team;

	protected override void Init()
	{
		if (!TryGetComponent(out _rb))
		{
			Debug.Log("Failed to Get rigidbody2D at PlayerMove");
		}

		_player.team = team;
	}

	public void Move(Vector2 dir)
	{
		_rb.velocity = dir * _status.moveSpeed;
	}

	public Player GetPlayer() => _player;

}
