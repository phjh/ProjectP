using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInherit : MonoBehaviour
{
    protected Player _player;

    protected InputReader _input;

    protected UserStatus _status;

    public void Init(Player player, InputReader input, UserStatus status)
    {
        _player = player;
        _input = input;
        _status = status;
        Init();
    }

    protected abstract void Init();

}
