using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeStatus", menuName = "SO/Combat/AbilityCards")]
public abstract class AbilityCards : ScriptableObject
{
	protected IAttackable damageable;
	protected IShieldable shieldable;

	public void Init(IAttackable damage, IShieldable shield)
	{
		damageable = damage;
		shieldable = shield;
	}

	public abstract void CardInit();

	public virtual void AddAttackStart() { }
	public virtual void AddAttackCollision() { }
	public virtual void AddShieldStart() { }
	public virtual void AddShieldCollision() { }
}
