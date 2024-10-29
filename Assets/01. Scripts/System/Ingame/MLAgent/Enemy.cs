using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Enemy : Agent, IDamageable, IAttackable, IShieldable
{
    private Player enemy;

    public Action ShieldStart { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action ShieldCollision { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action FireEvent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action CollisionEvent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float maxHp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float nowHp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ObjectTeam team { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Attack()
    {
        throw new NotImplementedException();
    }

    public void GetDamage(float damage, bool isPercent = false, Collision2D collision = null)
    {
        throw new NotImplementedException();
    }

    public void Shield()
    {
        throw new NotImplementedException();
    }
}
