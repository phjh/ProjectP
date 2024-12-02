using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Google.Protobuf.Protocol;

public class PlayerAttack : PlayerInherit, IAttackable
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
        _input.AttackEvent += Attack;
    }
    
    private bool CanAttack() => true;

    public void Attack()
    {
        if(CanAttack())
        {
            StartCoroutine(FireBullet());
            //FireBullet();
        }
    }

	private void FixedUpdate()
	{
        if (_input == null)
            return;

		SetPlayerAttackDir(_input.mousePos);
    }

    public void SetPlayerAttackDir(Vector2 mousePos, bool isOther = false)
    {
		if (_input == null)
			return;

		Vector3 position = transform.position;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);

		Vector2 dir = new Vector2
		(
			mousePosition.x - position.x,
			mousePosition.y - position.y
		);

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = rotation;

        
	}

	private IEnumerator FireBullet()
    {
        Vector3 position = firePos.position;
        Vector3 mousePosition = _input.mousePos;
        for (int i = 0; i < _status.fireBulletCount; i++)
        {
            SendShootPacket();

            Bullet bullet = PoolManager.Instance.Pop(PoolObjectListEnum.Bullet) as Bullet;
            bullet.transform.position = position;
            bullet.Init(FireEvent, CollisionEvent, _status, mousePosition, _player.team);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SendShootPacket()
    {
        C_Shoot shoot = new C_Shoot() { Info = new BulletInfo() };

        shoot.Info.Team = _player.team;


        NetworkManager.Instance.Send(shoot);
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
