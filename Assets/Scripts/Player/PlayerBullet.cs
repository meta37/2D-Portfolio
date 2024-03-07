using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed = 10f; // �Ѿ� ����
    public int damage;
    private void Start()
    {
        Destroy(gameObject, 2f); // 2�� �� �����
    }

    public void Update()
    {
        transform.Translate(Vector2.up * (Speed * Time.deltaTime), Space.Self); // �Ѿ� ���� �߻�
        if (transform.position.y > 3.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}