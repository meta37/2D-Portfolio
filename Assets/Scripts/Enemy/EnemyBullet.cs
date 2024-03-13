using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] public float Speed = 3f; // �Ѿ��� �ӵ�
    [SerializeField] public int damage; // �Ѿ��� ������ ��
    [SerializeField] public float lifetime = 2f; // �Ѿ��� �ı��Ǳ� ������ �����ϴ� �ð� (�� ����)

    private void Start()
    {
        Destroy(gameObject, lifetime); // �Ѿ� ���� �� ������ �ð�(lifetime)�� ������ �ڵ����� �ı�
    }
}