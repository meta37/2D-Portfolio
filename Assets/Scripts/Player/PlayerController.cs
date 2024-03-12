using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; // �÷��̾� �̵� �ӵ�
    public bool isTouchLeft, isTouchRight, isTouchBottom; // �÷��̾ ������ ��迡 ��Ҵ��� ����
    public bool isHit;

    public PooledObject bulletA; // �Ѿ� ������Ʈ Ǯ ����
    public PooledObject bulletB; // ��ü �Ѿ� ������

    public float maxShotDelay; // �Ѿ� �߻� ������ �ִ� ��� �ð�
    public float curShotDelay; // ���� �Ѿ� �߻���� ���� ��� �ð�
    public int life;
    public int score;

    public GameManager manager;

    void Start()
    {
        Manager.Pool.CreatePool(bulletA, 100, 20); // ���� ���� �� �Ѿ� ������Ʈ Ǯ ����
        Manager.Pool.CreatePool(bulletB, 100, 20);
    }

    void Update()
    {
        Move(); // �÷��̾� �̵� ó��
        Fire(); // �Ѿ� �߻� ó��
        Reload(); // �Ѿ� ������(��� �ð� ������Ʈ) ó��
    }

    void Move()
    {
        // Ű���� �Է��� ���� �̵� ���� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �÷��̾ ��迡 ��Ҵٸ� �ش� �������δ� �̵����� ����
        if ((isTouchLeft && h == -1) || (isTouchRight && h == 1)) h = 0;

        Vector3 curPos = transform.position; // ���� ��ġ
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; // ���� ��ġ ���

        transform.position = curPos + nextPos; // �÷��̾� ��ġ ������Ʈ
    }

    void Fire()
    {
        // �߻� ��ư�� ������ �ʾҰų�, �������� �Ϸ���� �ʾҴٸ� �߻����� ����
        if (!Input.GetButton("Fire1") || curShotDelay < maxShotDelay) return;

        Manager.Pool.GetPool(bulletA, transform.position, Quaternion.Euler(0, 0, 0)); // �Ѿ� �߻�
        curShotDelay = 0; // �Ѿ� �߻� �� ��� �ð��� 0���� ����
        Manager.Pool.GetPool(bulletB, transform.position, Quaternion.Euler(0, 0, 0));
        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime; // ��� �ð� ������Ʈ
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ���� ��迡 ����� ���� ó��
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left": isTouchLeft = true;
                    break;
                case "Right": isTouchRight = true;
                    break;
                case "Bottom": isTouchBottom = true;
                    break;
            }
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isHit)
                return;
            isHit = true;


            manager.RespawnPlayer();
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ ���� ��迡�� ����� ���� ó��
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Left": isTouchLeft = false;
                    break;
                case "Right": isTouchRight = false;
                    break;
                case "Bottom": isTouchBottom = false;
                    break;
            }
        }
    }
}