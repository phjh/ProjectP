using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Google.Protobuf.Protocol;
using UnityEngine.UIElements;

public class OtherPlayerAttack : PlayerInherit, IAttackable
{
	#region IAttackable

	public Action FireEvent { get; set; }
	public Action CollisionEvent { get; set; }

	#endregion

	[SerializeField]
	private Transform firePos;

	protected bool needReload => (_status.currentBulletCount - _status.fireBulletCount) <= 0;

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
		if (_input == null)
			return;
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
		Vector3 dir = transform.rotation * Vector3.right;
		for (int i = 0; i < _status.fireBulletCount; i++)
		{
			Bullet bullet = PoolManager.Instance.Pop(PoolObjectListEnum.Bullet) as Bullet;
			bullet.transform.position = position;
			bullet.Init(FireEvent, CollisionEvent, _status, dir, _player.team);

			yield return new WaitForSeconds(0.1f);
		}
	}

	private void SetAttackCooltime()
	{

	}

	private IEnumerator Reload(float reloadTime)
	{
		float t = 0;
		while (t < reloadTime)
		{
			//���⼭ �߰��� ������ ����� �־��ش�.
			t += Time.fixedDeltaTime;
			yield return new WaitForSeconds(Time.fixedDeltaTime);
		}
		_status.currentBulletCount = _status.maxBulletCount;
	}
}
