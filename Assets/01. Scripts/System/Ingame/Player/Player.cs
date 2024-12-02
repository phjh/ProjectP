using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None = 0,
    AttackStart = 1,
    AttackCollision = 2,
    ShieldStart = 3,
    ShieldCollision = 4,
}

public class Player : Entity
{
    [SerializeField]
    private PlayerUI playerUI;

    [SerializeField] //0에 move, 1에 attack
    private List<PlayerInherit> playerComponents;

    public InputReader _input;
    public UserStatus status;

    public IAttackable attackComponent = null;
    public IShieldable shieldComponent = null;

    public Dictionary<EventType, Dictionary<int, Action>> CardEvents = new();

	private void Awake()
	{
        if (GameManager.Instance.player == null)
        {
            GameManager.Instance.player = this;
            Debug.Log("new player set");
        }
        var stat = Instantiate(status);
        status = stat;
	}

	public override void RoundInit()
    {
        base.RoundInit();
        maxHp = status._maxHp;
        playerUI.SetHpBar(maxHp, nowHp);
        _rb.isKinematic = true;
        //_rb.useFullKinematicContacts = true;

        foreach(var comp in playerComponents)
        {
            comp.Init(this, _input, status);
        }

    }

    private void FindComponent()
    {
        Debug.Log("Finding Component");

        if (attackComponent == null)
        {
            Debug.Log("Attack Compnent is null");
            foreach(var comp in playerComponents)
            {
                comp.TryGetComponent(out attackComponent);
            }
        }
        if(shieldComponent == null)
        {
            Debug.Log("Shield Compnent is null");
            foreach(var comp in playerComponents)
            {
                comp.TryGetComponent(out shieldComponent);
            }
        }
    }

    public void AddCardEvent(Action act = null, int cardID = 0, EventType type = EventType.None)
    {
        if(attackComponent == null || shieldComponent == null)
            FindComponent();

        if (cardID == 0)
            return;

		switch (type)
        {
            case EventType.AttackStart:
                attackComponent.FireEvent += act;
                break;
            case EventType.AttackCollision:
                attackComponent.CollisionEvent += act;
                break;
            case EventType.ShieldStart:
                shieldComponent.ShieldStart += act;
                break;
            case EventType.ShieldCollision:
                shieldComponent.ShieldCollision += act;
                break;
            default:
                Debug.LogWarning("No type Selected");
                return;
        }
        CardEvents[type].Add(cardID, act);
    }

    public void DeleteCardEvent(int cardID = 0, EventType type = EventType.None)
    {
        if (attackComponent == null || shieldComponent == null)
            FindComponent();

        CardEvents[type].TryGetValue(cardID, out Action act);
        CardEvents[type].Remove(cardID);

        switch (type)
        {
            case EventType.AttackStart:
                attackComponent.FireEvent -= act;
                break;
            case EventType.AttackCollision:
                attackComponent.CollisionEvent -= act;
                break;
            case EventType.ShieldStart:
                shieldComponent.ShieldStart -= act;
                break;
            case EventType.ShieldCollision:
                shieldComponent.ShieldCollision -= act;
                break;
            default:
                Debug.LogWarning("No type Selected");
                return;
        }
    }


    //총알 위치등 세팅을 해주면서 추가 액션들을 넣어 세팅해준다. 
    //mass로 총알의 힘을 파악하고 힘은 파워랑 속도에 따라 커지게 해준다.

    protected override void OnDead()
    {
        PoolManager.Instance.Push(this, pair.enumtype);
    }

    protected override void GetDamage(float damage, bool isPercent, Collision2D collision)
    {
        base.GetDamage(damage, isPercent, collision);
        playerUI.SetHpBar(maxHp, nowHp);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            GetDamage(10, false, null);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GetDamage(-10, false, null);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            AddCardEvent();
        }
    }

    public PlayerAttack GetPlayerAttack() => playerComponents[1] as PlayerAttack;
}
