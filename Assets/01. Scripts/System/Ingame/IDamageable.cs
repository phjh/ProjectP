using Google.Protobuf.Protocol;
using UnityEngine;

public interface IDamageable
{
    public Team team {  get; set; }

    public float maxHp { get; set; }
    public float nowHp { get; set; }
    public bool isAlive { get { return nowHp > 0; } }

    public void GetDamage(float damage, bool isPercent = false, Collision2D collision = null);

}
