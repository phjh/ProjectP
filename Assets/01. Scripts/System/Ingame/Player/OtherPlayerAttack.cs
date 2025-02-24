using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Google.Protobuf.Protocol;
using UnityEngine.UIElements;
using UnityEditor.Rendering;

public class OtherPlayerAttack : PlayerInherit, IAttackable
{
	#region IAttackable

	public Action FireEvent { get; set; }
	public Action CollisionEvent { get; set; }

	#endregion

	[SerializeField]
	private Transform firePos;


	public bool updateAiming { get; set; } = true;

	protected bool needReload => (_status.currentBulletCount - _status.fireBulletCount) <= 0;

	public Vector2 mousePos;

	protected override void Init()
	{
	}

	private bool CanAttack() => true;

	public void Attack()
	{
		if (CanAttack())
		{
			StartCoroutine(FireBullet());
			//FireBullet();
		}
	}

	public void SetPlayerAttackDir(Vector3 dir, bool isOther = false)
	{
		if (_input == null || updateAiming == true)
			return;

		mousePos = dir;
		
		Vector2 setdir = new Vector2
		(
			dir.x - transform.position.x,
			dir.y - transform.position.y
		);

		float angle = Mathf.Atan2(setdir.y, setdir.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = rotation;
	}

	private IEnumerator FireBullet()
	{
		Vector3 position = firePos.position;
		Vector3 dir = mousePos;
		for (int i = 0; i < _status.fireBulletCount; i++)
		{
			Bullet bullet = PoolManager.Instance.Pop(PoolObjectListEnum.Bullet) as Bullet;
			bullet.transform.position = position;
			bullet.Init(FireEvent, CollisionEvent, _status, dir, _player.team, true);

			yield return new WaitForSeconds(0.1f);
		}
		updateAiming = true;
	}

	private void SetAttackCooltime()
	{

	}

	private IEnumerator Reload(float reloadTime)
	{
		float t = 0;
		while (t < reloadTime)
		{
			//여기서 추가로 재장전 모션을 넣어준다.
			t += Time.fixedDeltaTime;
			yield return new WaitForSeconds(Time.fixedDeltaTime);
		}
		_status.currentBulletCount = _status.maxBulletCount;
	}
}
