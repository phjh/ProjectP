using UnityEngine;

public enum StatID
{
    MaxHP = 0,
    Strength = 1,
    MoveSpeed = 2,
    BulletSpeed = 3,
    AttackSpeed = 4,
    ReloadSpeed = 5,
}


[CreateAssetMenu(fileName = "New Status", menuName = "SO/UserStatus")]
public class UserStatus : ScriptableObject
{
    public float _maxHp;

    public float damage;
    public float moveSpeed;
    public float bulletSpeed;

    public float attackCoolTime;
    public float reloadTime;

    public int maxBulletCount;
    public int currentBulletCount;
    public int fireBulletCount;
    public int bounceCount;

    public UserStatus(float maxHp = 0, float damage = 0, float moveSpeed = 0, float bulletSpeed = 0, float attackCoolTime = 0, float reloadTime = 0, int maxBulletCount = 0, int fireBulletCount = 0, int bounceCount = 0)
    {
        _maxHp = maxHp;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.bulletSpeed = bulletSpeed;
        this.attackCoolTime = attackCoolTime;
        this.reloadTime = reloadTime;
        this.maxBulletCount = maxBulletCount;
        this.fireBulletCount = fireBulletCount;
        this.bounceCount = bounceCount;
    }

    //두번째 스텟만큼의 퍼센트로 오른다
    public static UserStatus operator+(UserStatus stat1, UserStatus stat2)
    {
        stat1._maxHp = stat1._maxHp * (1 + stat2._maxHp / 100);
        stat1.damage = stat1.damage * (1 + stat2.damage / 100);
        stat1.moveSpeed = stat1.moveSpeed * (1 + stat2.moveSpeed / 100);
        stat1.bulletSpeed = stat1.bulletSpeed * (1 + stat2.bulletSpeed / 100);
        stat1.attackCoolTime = stat1.attackCoolTime * (1 + stat2.attackCoolTime / 100);
        stat1.reloadTime = stat1.reloadTime * (1 + stat2.reloadTime / 100);
        stat1.bounceCount = stat1.bounceCount + stat2.bounceCount;
        stat1.maxBulletCount = stat1.maxBulletCount + stat2.maxBulletCount;
        stat1.fireBulletCount = stat1.fireBulletCount + stat2.fireBulletCount;
        return stat1;
    }

}
