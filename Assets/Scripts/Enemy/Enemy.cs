using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp; // ���� ü��
    public int enemyScore;
    public float speed; // ���� �ӵ�

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject EnemybulletA;
    public GameObject EnemybulletB;
    public GameObject player;

    private void Update()
    {
        Fire();
        Reload();
    }

    private void Awake()
    {
        // �ʱ�ȭ �ڵ�� ���⿡ �ۼ� (����� ��� ����)
    }

    private void Fire()
    {
        if (curShotDelay < maxShotDelay)
            return;

        if (enemyName == "Enemy01")
        {
            GameObject bullet = Instantiate(EnemybulletA, transform.position,transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "Enemy02")
        {
            GameObject bulletA = Instantiate(EnemybulletA, transform.position + Vector3.right * 0.3f,transform.rotation);
            Rigidbody2D rigidA = bulletA.GetComponent <Rigidbody2D>();
            Vector3 dirVecA = player.transform.position - (transform.position + Vector3.right * 0.3f);
            rigidA.AddForce(dirVecA.normalized * 4, ForceMode2D.Impulse);

            GameObject bulletB = Instantiate(EnemybulletA, transform.position + Vector3.left * 0.3f , transform.rotation);
            Rigidbody2D rigidB = bulletB.GetComponent<Rigidbody2D>();
            Vector3 dirVecB = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigidB.AddForce(dirVecB.normalized * 4, ForceMode2D.Impulse);
        }
        curShotDelay = 0;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    // ���� ���ظ� �޾��� �� ȣ��
    private void OnTakeHit(int damage)
    {
        hp -= damage; // ���� ���ظ�ŭ ü���� ����

        if (hp <= 0) // ü���� 0 ���ϸ� ���� �ı�
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            playerLogic.score += enemyScore;
            Destroy(gameObject);
        }
    }

    // �ٸ� Collider�� �浹���� �� �ڵ����� ȣ��
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet") // ���� �Ѿ˿� �¾��� ���
            Destroy(gameObject); // ���� �ٷ� �ı�
        else if (collision.gameObject.tag == "PlayerBullet") // �÷��̾��� �Ѿ˿� �¾��� ���
        {
            PlayerBullet bullet = collision.gameObject.GetComponent<PlayerBullet>(); // �浹�� �Ѿ��� ��ũ��Ʈ�� ������
            OnTakeHit(bullet.damage); // ���� ó�� �Լ� ȣ��
            // �� �ı��� OnTakeHit ������ ü�� �˻� �� ó��
        }
    }
}
