using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EditStatusCard : PowerupCards
{
    [SerializeField]
    private float HP = 0;
    [SerializeField]
    private float damage = 0;
    [SerializeField]
    private float moveSpeed = 0;
    [SerializeField]
    private float bulletSpeed = 0;
    [SerializeField]
    private float attackSpeed = 0;
    [SerializeField]
    private float reloadSpeed = 0;

    [SerializeField]
    private int MaxBulletCount = 0;
    [SerializeField]
    private int FireBulletCount = 0;

    public override void CardInit()
    {
        status += new UserStatus(HP, damage, moveSpeed, bulletSpeed, attackSpeed, reloadSpeed, MaxBulletCount, FireBulletCount);
    }
}
