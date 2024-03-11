using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed = 5f; // �Ѿ��� �ӵ�
    public int damage; // �Ѿ��� ������ ��
    public float lifetime = 2f; // �Ѿ��� �ı��Ǳ� ������ �����ϴ� �ð� (�� ����)

    private void Start()
    {
        Destroy(gameObject, lifetime); // �Ѿ� ���� �� ������ �ð�(lifetime)�� ������ �ڵ����� �ı�
    }

    void Update()
    {
        transform.Translate(Vector2.up * (Speed * Time.deltaTime), Space.Self); // �Ѿ��� �� �����Ӹ��� ���� �̵���Ŵ
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // �Ѿ��� ���� �浹�� ���
        {
            Destroy(gameObject); // �Ѿ� �ı�
        }
    }
}