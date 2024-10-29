using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : PlayerInherit, IShieldable
{
    #region shieldAble

    public Action ShieldStart { get; set; }
    public Action ShieldCollision { get; set; }

    #endregion
    protected override void Init()
    {

    }

    public void Shield()
    {

    }


}
