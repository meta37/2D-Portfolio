using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed = 5f; // 총알 설정
    public int damage;
    private void Start()
    {
        Destroy(gameObject, 2f); // 총알 2초 후 사라짐
    }

    public void Update()
    {
        transform.Translate(Vector2.up * (Speed * Time.deltaTime), Space.Self); // 총알 위로 발사
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // 적이랑 충돌 시 사라짐
        {
            Destroy(gameObject);
        }
    }
}