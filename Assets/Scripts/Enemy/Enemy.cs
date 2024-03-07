using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int speed;

    // [SerializeField] Rigidbody2D rigid;

    private void Awake()
    {
        // rigid = GetComponent<Rigidbody2D>();
        // rigid.velocity = Vector2.down * speed;
    }

    private void OnTakeHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
            Destroy(gameObject);
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>();
            OnTakeHit(bullet.damage);
        }
    }
}
