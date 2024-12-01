using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableMono
{
    private Rigidbody2D rb;

    private Team team;
    private Action collisionevent;
    private int bounceCount = 0;
    private float damage = 0;
    public float speed = 1;
    private float mass = 1;

    private Vector2 direction(Vector2 pos, Vector2 target) => 
        new Vector2(target.x - pos.x, target.y - pos.y);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.useFullKinematicContacts = true;
    }

    public void Init(Action start, Action collision, UserStatus stat, Vector2 mousePos, Team team)
    {
        start?.Invoke();
        collisionevent = collision;
        bounceCount = stat.bounceCount;
        mass = stat.bulletSpeed * stat.damage;
        damage = stat.damage;
        speed = stat.bulletSpeed;
        this.team = team;
        SetDir(mousePos);
        //Testing();
    }

    private void Testing()
    {
        GameManager.TestingDebug("Bullet");
        Color color = UnityEngine.Random.ColorHSV();
        GetComponent<SpriteRenderer>().color = color;
        Debug.Log(color);
    }

    private void SetDir(Vector2 mousePos)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = new Vector2
            (
                mousePosition.x - transform.position.x,
                mousePosition.y - transform.position.y
            );
    
        rb.velocity = direction.normalized * speed;
    }

    public override void PoolInit()
    {

    }

    public void SetSpeed(float speed) => this.speed = speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("idamageable");
            if (damageable.team == team)
            {
                Debug.Log("team");
                rb.useFullKinematicContacts = false;
                return;
            }
            damageable.GetDamage(damage, collision: collision);
            collisionevent?.Invoke();
            PoolManager.Instance.Push(this, pair.enumtype);
            return;
        }
        else if (bounceCount <= 0)
        {
            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                if (mass <= bullet.mass)
                {
                    PoolManager.Instance.Push(this, pair.enumtype);
                    bullet.mass -= mass;
                }
            }
            else
            {
                collisionevent?.Invoke();
                PoolManager.Instance.Push(this, pair.enumtype);
            }
        }
        else
        {
            bounceCount--;
            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                Vector2 dir = bullet.rb.velocity;
                bullet.rb.velocity = (dir - rb.velocity + direction(transform.position, bullet.transform.position)).normalized * bullet.speed;
                rb.velocity = (rb.velocity - dir + direction(bullet.transform.position, transform.position)).normalized * speed;
            }
            else
            {
                collisionevent?.Invoke();
                BounceBullet(collision);
            }
        }
    }

    private void BounceBullet(Collision2D collision)
    {
        Vector2 dir = rb.velocity;
        rb.velocity = Vector2.Reflect(dir.normalized, -collision.GetContact(0).normal) * speed;
    }

}
