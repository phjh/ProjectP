using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTeam
{
    None = 0,
    Blue = 1,
    Red = 2,
}

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : PoolableMono, IDamageable
{
    #region IDamageAble

    [SerializeField]
    private float _maxhp;
    private float _nowhp;

    public Team team { get; set; }
    public float maxHp { get { return _maxhp; } set { _maxhp = value; } }
    public float nowHp { get { return _nowhp; } set { _nowhp = value; } }
    public bool isAlive { get { return nowHp > 0; } }

    #endregion

    [SerializeField]
    protected Team TestTeam;

    protected Rigidbody2D _rb;

    public override void PoolInit()
    {
        _rb = GetComponent<Rigidbody2D>();
        GameScoreinit();
        RoundInit();
    }

    //
    public virtual void RoundInit()
    {
        nowHp = _maxhp;

    }

    public virtual void GameScoreinit()
    {
        maxHp = _maxhp;
        nowHp = _maxhp;

    }

    private void Knonkback(float damage, Collision2D collision)
    {
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - collision.contacts[0].point;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir.normalized * damage);
    }

    protected virtual void GetDamage(float damage, bool isPercent, Collision2D collision)
    {
        if(collision != null)
        {
            Knonkback(damage, collision);
        }

        if (isPercent)
        {
            nowHp -= maxHp / 100f * damage;
        }
        else 
        { 
            nowHp -= damage;
        }

        if (!isAlive)
        {
            OnDead();
        }
        
    }

    protected void GiveDamage(IDamageable damageAble)
    {
        //damageAble.GetDamage(status.damage);
    }

    protected abstract void OnDead();

#pragma warning disable CS1066 // 지정된 기본값은 선택적 인수를 허용하지 않는 컨텍스트에서 사용되는 멤버에 적용되므로 효과가 없습니다.
    void IDamageable.GetDamage(float damage, bool isPercent = false, Collision2D collision = null) => GetDamage(damage, isPercent, collision);
#pragma warning restore CS1066 // 지정된 기본값은 선택적 인수를 허용하지 않는 컨텍스트에서 사용되는 멤버에 적용되므로 효과가 없습니다.

}
