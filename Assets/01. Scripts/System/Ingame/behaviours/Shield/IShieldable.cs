using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldable
{
    public Action ShieldStart { get; set; }
    public Action ShieldCollision { get; set; }
    public abstract void Shield();
}
