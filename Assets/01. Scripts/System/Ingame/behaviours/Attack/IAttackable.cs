using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public Action FireEvent { get; set; }
    public Action CollisionEvent { get; set; }
    public abstract void Attack();
}
