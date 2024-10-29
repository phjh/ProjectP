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
    }

    private void Move(Vector2 dir)
    {
        _rb.velocity = dir * _status.moveSpeed;
    }

}
